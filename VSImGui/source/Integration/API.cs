using ImGuiNET;
using Vintagestory.API.Client;
using Vintagestory.API.Config;


namespace VSImGui.API;

/// <summary>
/// Is used to determine behavior of vintage story GUI dialog that is used for layering and managing inputs
/// </summary>
public enum CallbackGUIStatus
{
    /// <summary>
    /// All ImGUI windows in callback are closed
    /// </summary>
    Closed,
    /// <summary>
    /// Require cursor to shot and lock camera
    /// </summary>
    GrabMouse,
    /// <summary>
    /// Do not require cursor to shot and lock camera (works only with immersive mouse mode)
    /// </summary>
    DontGrabMouse
}

/// <summary>
/// Should contain all the ImGui draw related calls and return status of all ImGui windows managed by this callback
/// </summary>
/// <param name="deltaSeconds">Straight from the Render call to VS GUI dialog used to manage ImGui rendering and inputs</param>
/// <returns>Aggregated status of all ImGui windows managed by this callback</returns>
public delegate CallbackGUIStatus DrawCallbackDelegate(float deltaSeconds);

/// <summary>
/// Provides a way to interact with ImGui integration into VS
/// </summary>
public interface IImGuiRenderer
{
    /// <summary>
    /// Should contain all the ImGui calls to draw windows and widgets.<br/>
    /// Each imgui 'Begin' method should be closed with 'End' inside callback. Same for 'Push' and 'Pop', and using Styles.
    /// </summary>
    event DrawCallbackDelegate Draw;
    /// <summary>
    /// Invoked when player tries to close all ImGui windows at once (pressing escape or using hotkey)
    /// </summary>
    event Action Closed;
    /// <summary>
    /// Style used for all ImGui windows and widgets be default
    /// </summary>
    Style? DefaultStyle { get; }
    /// <summary>
    /// Forces to show all ImGui windows, should be called when new ImGui window is created so it will be shown and be able to capture inputs
    /// </summary>
    void Show();
}

/// <summary>
/// Same as <see cref="ImGuiDialogBase"/> but also handles ImGui window.<br/>
/// You should not call <see cref="ImGui.Begin"/> and <see cref="ImGui.End"/> methods in <see cref="ImGuiDialogBase.OnDraw"/>.
/// </summary>
public abstract class ImGuiDialogWindow : ImGuiDialogBase
{
    /// <summary>
    /// Displayed window title
    /// </summary>
    protected string WindowTitle { get; }
    /// <summary>
    /// Depending on whether title included into id (defined in constructor argument) or not pair <see cref="WindowId"/> + <see cref="WindowTitle"/> or just <see cref="WindowId"/> should be unique among all ImGui windows.<br/>
    /// ImGui window position and size stored in imgui.ini are mapped per window id, so if you will have same id between game sessions, mentioned window parameters will persist.
    /// </summary>
    protected string WindowId { get; }
    /// <summary>
    /// Additional info displayed in title that do not affect window id
    /// </summary>
    protected string TitleInfo { get; set; } = "";
    /// <summary>
    /// Flags supplied to <see cref="ImGui.Begin"/>.
    /// </summary>
    protected ImGuiWindowFlags Flags { get; set; }

    /// <summary>
    /// Constructs ids and saves flags
    /// </summary>
    /// <param name="api"></param>
    /// <param name="windowTitle">Displayed title.<br/> See <see cref="WindowId"/> documentation for more info.</param>
    /// <param name="windowId">Window id.<br/> See <see cref="WindowId"/> documentation for more info.</param>
    /// <param name="includeTitleIntoId">Whether title is part of window id.<br/> See <see cref="WindowId"/> documentation for more info.</param>
    /// <param name="flags">Flags supplied to <see cref="ImGui.Begin"/>.</param>
    protected ImGuiDialogWindow(ICoreClientAPI api, string windowTitle, string windowId = "", bool includeTitleIntoId = true, ImGuiWindowFlags flags = ImGuiWindowFlags.None) : base(api)
    {
        WindowTitle = windowTitle;
        if (includeTitleIntoId)
        {
            WindowId = $"{windowTitle}.{windowId}";
        }
        else
        {
            WindowId = windowId;
        }

        _title = $"##{WindowId}###{WindowTitle}";
        Flags = flags;
    }
    /// <summary>
    /// Constructs ids and saves flags
    /// </summary>
    /// <param name="api"></param>
    /// <param name="windowTitle">Displayed title.<br/> See <see cref="WindowId"/> documentation for more info.</param>
    /// <param name="windowId">Window id.<br/> See <see cref="WindowId"/> documentation for more info.</param>
    /// <param name="includeTitleIntoId">Whether title is part of window id.<br/> See <see cref="WindowId"/> documentation for more info.</param>
    /// <param name="flags">Flags supplied to <see cref="ImGui.Begin"/>.</param>
    protected ImGuiDialogWindow(ICoreClientAPI api, ImGuiWindowFlags flags, string windowTitle, string windowId = "", bool includeTitleIntoId = true) : this(api, windowTitle, windowId, includeTitleIntoId, flags)
    {
    }

    protected override CallbackGUIStatus Draw(float deltaSeconds)
    {
        bool opened = Opened;

        if (!opened) return DrawStatus();

        if (ImGui.Begin($"{_title}{TitleInfo}", ref opened, Flags))
        {
            if (!OnDraw() && opened) Close();
            ImGui.End();
        }

        if (!opened) Close();

        return DrawStatus();
    }

    private readonly string _title;
}

/// <summary>
/// Convenience class for drawing ImGui dialogs.<br/>
/// It handles return value required from Draw method and adds/removes itself from drawing callback list on construction/disposing.<br/>
/// Also handles close event <see cref="ImGuiModSystem.Closed"/> and calls show method <see cref="ImGuiModSystem.Show"/>.
/// </summary>
public abstract class ImGuiDialogBase : IDisposable
{
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    /// <summary>
    /// Hides dialog
    /// </summary>
    public void Close()
    {
        if (OnClose())
        {
            Opened = false;
        }
    }
    /// <summary>
    /// Shows dialog
    /// </summary>
    public void Open()
    {
        if (OnOpen())
        {
            Opened = true;
            _system.Show();
        }
    }

    /// <summary>
    /// Adds itself to ImGui draw callbacks list.
    /// </summary>
    /// <param name="api"></param>
    protected ImGuiDialogBase(ICoreClientAPI api)
    {
        ClientApi = api;
        _system = api.ModLoader.GetModSystem<ImGuiModSystem>();
        _system.Draw += Draw;
        _system.Closed += Close;
    }

    protected ICoreClientAPI ClientApi { get; }
    /// <summary>
    /// Whether dialog is shown. Use <see cref="Open"/> and <see cref="Close"/> methods to show/hide dialog.
    /// </summary>
    protected bool Opened { get; private set; } = false;
    /// <summary>
    /// Whether mouse should be unlocked in Immersive Mouse mode.
    /// </summary>
    protected bool GrabMouse { get; private set; }

    /// <summary>
    /// Your <see cref="ImGui"/> methods to begin windows and draw widgets should be here.
    /// </summary>
    /// <returns>Whether dialog should be closed.</returns>
    protected abstract bool OnDraw();
    /// <summary>
    /// Called before dialog is closed.
    /// </summary>
    /// <returns>Whether dialog should be closed.</returns>
    protected virtual bool OnClose()
    {
        return true;
    }
    /// <summary>
    /// Called before dialog is opened.
    /// </summary>
    /// <returns>Whether dialog should be opened.</returns>
    protected virtual bool OnOpen()
    {
        return true;
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _system.Draw -= Draw;
                _system.Closed -= Close;
            }

            _disposed = true;
        }
    }
    /// <summary>
    /// Methods supplied to <see cref="ImGuiModSystem.Draw"/>.<br/> Should call <see cref="OnDraw"/>.
    /// </summary>
    /// <param name="deltaSeconds">Time it took to render last frame</param>
    /// <returns>Dialog status that indicates if it is closed and if mouse should be grabbed.</returns>
    protected virtual CallbackGUIStatus Draw(float deltaSeconds)
    {
        if (!Opened) return DrawStatus();

        if (!OnDraw()) Close();

        return DrawStatus();
    }
    /// <summary>
    /// Returns dialog status based on <see cref="Opened"/> and <see cref="GrabMouse"/>.
    /// </summary>
    /// <returns>Dialog status that indicates if it is closed and if mouse should be grabbed.</returns>
    protected CallbackGUIStatus DrawStatus()
    {
        if (Opened)
        {
            return GrabMouse ? CallbackGUIStatus.GrabMouse : CallbackGUIStatus.DontGrabMouse;
        }
        else
        {
            return CallbackGUIStatus.Closed;
        }
    }

    private bool _disposed;
    private readonly ImGuiModSystem _system;
}
