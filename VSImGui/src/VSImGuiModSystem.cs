using ImGuiNET;
using Newtonsoft.Json;
using System;
using System.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.Client.NoObf;

namespace VSImGui;

public class VSImGuiModSystem : ModSystem
{
    public Style? DefaultStyle { get; set; }

    public event Action SetUpImGuiWindows
    {
        add { if (mMainWindowWrapper != null) mMainWindowWrapper.Draw += value; }
        remove { if (mMainWindowWrapper != null)  mMainWindowWrapper.Draw -= value; }
    }

    private VSImGuiController? mController;
    private VSGameWindowWrapper? mMainWindowWrapper;
    private ICoreClientAPI? mApi;
    private VSImGuiDialog? mDialog;

    public override void StartPre(ICoreAPI api)
    {
        if (api is not ICoreClientAPI clientApi) return;
        EmbeddedDllClass.ExtractEmbeddedDlls(api.Logger);
        EmbeddedDllClass.LoadImGui(api.Logger);

        mApi = clientApi;
        
        GameWindowNative? window = GetWindow(clientApi);
        if (window == null) return;

        mMainWindowWrapper = new(window);
        mController = new(mMainWindowWrapper);
        mDialog = new(clientApi, mController, mMainWindowWrapper);
        mDialog.TryOpen();
        mController.OnWindowMergedIntoMain += () => mDialog.TryOpen();
        clientApi.Event.RegisterRenderer(new OffWindowRenderer(mDialog), EnumRenderStage.Ortho);
    }
    public override void StartClientSide(ICoreClientAPI api)
    {
        mApi = api;

        api.Input.RegisterHotKey("imguitoggle", Lang.Get("vsimgui:imgui-toggle"), GlKeys.P, HotkeyType.GUIOrOtherControls, false, true, false);

        SetUpImGuiWindows += ImGui.ShowDemoWindow;

        SetUpImGuiWindows += DebugWindow.Draw;
    }

    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Client;

    public override void AssetsLoaded(ICoreAPI api)
    {
        if (api is not ICoreClientAPI clientApi) return;
        DefaultStyle = JsonConvert.DeserializeObject<Style>(LoadDefaultStyle(clientApi));
        DefaultStyle?.Push();
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

    private static GameWindowNative? GetWindow(ICoreClientAPI client)
    {
        if (client.World is not ClientMain main) return null;
        FieldInfo? field = typeof(ClientMain).GetField("Platform", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
        ClientPlatformWindows? platform = (ClientPlatformWindows?)field?.GetValue(main);
        return platform?.window;
    }
}