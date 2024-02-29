using ImGuiNET;
using Newtonsoft.Json;
using System;
using System.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.Client.NoObf;
using VSImGui.API;

namespace VSImGui;

public class ImGuiModSystem : ModSystem, IImGuiRenderer
{
    public Style? DefaultStyle { get; set; }

    public event DrawCallbackDelegate Draw
    {
        add
        {
            if (mGuiManager != null) mGuiManager.DrawCallback += value;
        }
        remove
        {
            if (mGuiManager != null) mGuiManager.DrawCallback -= value;
        }
    }

    public void Show() => mDialog?.TryOpen();

    public event Action? Closed;

    private Controller? mController;
    private MainGameWindowWrapper? mMainWindowWrapper;
    private ICoreClientAPI? mApi;
    private VSImGuiDialog? mDialog;
    private DrawCallbacksManager? mGuiManager;
    private StyleEditor? mStyleEditor;

    public override void StartPre(ICoreAPI api)
    {
        if (api is not ICoreClientAPI clientApi) return;
        bool loaded = NativesLoader.Load(api.Logger, this);
        if (!loaded) return;

        mApi = clientApi;

        mGuiManager = new();
        mMainWindowWrapper = new(clientApi);
        mController = new(mMainWindowWrapper);
        mDialog = new(clientApi, mController, mMainWindowWrapper, mGuiManager);
        mDialog.TryOpen();
        mDialog.OnClosed += OnGuiClosed;
        mController.OnWindowMergedIntoMain += () => mDialog.TryOpen();
        clientApi.Event.RegisterRenderer(new OffWindowRenderer(mDialog), EnumRenderStage.Ortho);
        clientApi.Input.RegisterHotKey("imguitoggle", Lang.Get("vsimgui:imgui-toggle"), GlKeys.P, HotkeyType.GUIOrOtherControls, false, true, false);

        Draw += DebugWindow.Draw;

#if DEBUG
        //Draw += DemoWindow;
#endif
    }


    private void OnGuiClosed()
    {
        Closed?.Invoke();
    }

    private CallbackGUIStatus DemoWindow(float dt)
    {
        if (DefaultStyle == null || mStyleEditor == null) return CallbackGUIStatus.Closed;

        using (new StyleApplier(DefaultStyle))
        {
            mStyleEditor.Draw();

            ImGui.Begin("TEST");

            ImGui.Text("test test test");

            ImGui.BeginChild("test", new(300, 300), true);

            ImGui.Text("test test test");

            ImGui.EndChild();

            ImGui.End();
        }

        return CallbackGUIStatus.GrabMouse;
    }

    public override double ExecuteOrder() => 0;

    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Client;

    public override void AssetsLoaded(ICoreAPI api)
    {
        if (api is not ICoreClientAPI clientApi) return;
        DefaultStyle = JsonConvert.DeserializeObject<Style>(LoadDefaultStyle(clientApi));
        DefaultStyle?.Push();
        if (DefaultStyle != null) mStyleEditor = new(DefaultStyle);
    }

    public override void Dispose()
    {
        DebugWindow.Clear();

        base.Dispose();
    }

    private static string LoadDefaultStyle(ICoreClientAPI api)
    {
        byte[] data = api.Assets.Get("vsimgui:config/defaultstyle.json").Data;
        return System.Text.Encoding.UTF8.GetString(data);
    }
}