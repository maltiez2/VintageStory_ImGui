using Newtonsoft.Json;
using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using VSImGui.API;

namespace VSImGui;

/// <summary>
/// Initializes ImGui integration into VS and provides interface for drawing ImGui dialogs
/// </summary>
public class ImGuiModSystem : ModSystem, IImGuiRenderer
{
    #region IImGuiRenderer
    public Style? DefaultStyle { get; private set; }
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
    public void Show() => _dialog?.TryOpen();
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
        clientApi.Event.RegisterRenderer(new OffWindowRenderer(_dialog), EnumRenderStage.Ortho);
        clientApi.Input.RegisterHotKey("imguitoggle", Lang.Get("vsimgui:imgui-toggle"), GlKeys.P, HotkeyType.GUIOrOtherControls, false, true, false);

        Draw += DebugWindowsManager.Draw;
    }
    public override double ExecuteOrder() => 0;
    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Client;
    public override void AssetsLoaded(ICoreAPI api)
    {
        if (api is not ICoreClientAPI clientApi) return;
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

        base.Dispose();
    }

    private Controller? _controller;
    private MainGameWindowWrapper? _mainWindowWrapper;
    private VSImGuiDialog? _dialog;
    private DrawCallbacksManager? _guiManager;
    #endregion
}