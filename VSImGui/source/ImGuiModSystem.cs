using ImGuiNET;
using Newtonsoft.Json;
using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using VSImGui.API;
using VSImGui.Debug;

namespace VSImGui;

/// <summary>
/// Initializes ImGui integration into VS and provides interface for drawing ImGui dialogs
/// </summary>
public class ImGuiModSystem : ModSystem, IImGuiRenderer
{
    #region IImGuiRenderer
    /// <summary>
    /// Style used pushed after loading as default
    /// </summary>
    public Style? DefaultStyle { get; private set; }
    /// <summary>
    /// Draw callback that should contain ImGui windows with widgets.<br/>
    /// Each imgui 'Begin' method should be closed with 'End' inside callback. Same for 'Push' and 'Pop', and using Styles.
    /// </summary>
    public event DrawCallbackDelegate Draw
    {
        add
        {
            if (_guiManager != null) _guiManager.DrawCallback += value;
        }
        remove
        {
            if (_guiManager != null) _guiManager.DrawCallback -= value;
        }
    }
    /// <summary>
    /// Shows all currently opened ImGui windows in main window<br/>
    /// Tries to open VS dialog that is used to integrate ImGui into VS.
    /// </summary>
    public void Show() => _dialog?.TryOpen();
    /// <summary>
    /// Is called when all ImGui windows are closed by 'Esc' button or hotkey.
    /// </summary>
    public event Action? Closed;
    #endregion

    #region Initialisation and disposing
    public override void StartPre(ICoreAPI api)
    {
        if (api is not ICoreClientAPI clientApi) return;
        bool loaded = NativesLoader.Load(api.Logger, this);
        if (!loaded) return;

        _guiManager = new();
        _mainWindowWrapper = new(clientApi);
        _controller = new(_mainWindowWrapper);
        _dialog = new(clientApi, _controller, _mainWindowWrapper, _guiManager);
        _dialog.TryOpen();
        _dialog.OnClosed += () => Closed?.Invoke();
        _controller.OnWindowMergedIntoMain += () => _dialog.TryOpen();
        _offWindowRenderer = new OffWindowRenderer(_dialog);
        clientApi.Event.RegisterRenderer(_offWindowRenderer, EnumRenderStage.Ortho);
        clientApi.Input.RegisterHotKey("imguitoggle", Lang.Get("vsimgui:imgui-toggle"), GlKeys.P, HotkeyType.GUIOrOtherControls, false, true, false);

        Draw += DrawDebugWindow;
    }
    public override double ExecuteOrder() => 0;
    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Client;
    public override void AssetsLoaded(ICoreAPI api)
    {
        if (api is not ICoreClientAPI) return;
        byte[] data = api.Assets.Get("vsimgui:config/defaultstyle.json").Data;
        string serializedStyle = System.Text.Encoding.UTF8.GetString(data);
        DefaultStyle = JsonConvert.DeserializeObject<Style>(serializedStyle);
        DefaultStyle?.Push();
    }
    public override void Dispose()
    {
        DebugWindowsManager.Clear();
        _controller?.Dispose();
        _mainWindowWrapper?.Dispose();
        _dialog?.Dispose();
        _offWindowRenderer?.Dispose();

        base.Dispose();
    }

    private Controller? _controller;
    private MainGameWindowWrapper? _mainWindowWrapper;
    private VSImGuiDialog? _dialog;
    private DrawCallbacksManager? _guiManager;
    private OffWindowRenderer? _offWindowRenderer;

    private CallbackGUIStatus DrawDebugWindow(float timeSeconds)
    {
        return DebugWindowsManager.Draw() ? CallbackGUIStatus.DontGrabMouse : CallbackGUIStatus.Closed;
    }
    #endregion
}