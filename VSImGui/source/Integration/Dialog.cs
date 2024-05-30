using Vintagestory.API.Client;

namespace VSImGui;

/// <summary>
/// Used to layer ImGui windows with VS GUI dialogs, to handle inputs, to call ImGui draw and render methods for main game window
/// </summary>
internal class VSImGuiDialog : GuiDialog
{
    public VSImGuiDialog(ICoreClientAPI api, Controller controller, MainGameWindowWrapper windowWrapper, DrawCallbacksManager manager) : base(api)
    {
        _controller = controller;
        _manager = manager;

        windowWrapper.Draw += Draw;
    }

    public override void Dispose()
    {
        base.Dispose();
        capi = null;
    }


    #region Controller calls
    /// <summary>
    /// Updates ImGui inputs, called only by <see cref="OffWindowRenderer"/>
    /// </summary>
    /// <param name="deltaTime">Time it took to render previous frame</param>
    public void Update(float deltaTime)
    {
        _controller.Update(deltaTime, opened);
    }
    /// <summary>
    /// Renders ImGui windows and widgets into secondary native windows, called only by <see cref="OffWindowRenderer"/>
    /// </summary>
    /// <param name="deltaTime"></param>
    public void RenderOffWindow(float deltaTime)
    {
        _controller.RenderOffWindow(deltaTime);
    }
    public override void OnRenderGUI(float deltaTime)
    {
        _controller.RenderMainWindow(deltaTime);
    }
    #endregion

    #region Vanilla dialoug settings
    public override string ToggleKeyCombinationCode => "imguitoggle";
    public override bool ShouldReceiveRenderEvents() => true;
    public override bool PrefersUngrabbedMouse => _grabMouse; // In case of ImGui: grab means 'grab by GUI', opposite of game's meaning
    public override EnumDialogType DialogType => _grabMouse ? EnumDialogType.Dialog : EnumDialogType.HUD;
    #endregion

    #region Input handling
    public override void OnMouseDown(MouseEvent args) => HandleMouse(args);
    public override void OnMouseUp(MouseEvent args) => HandleMouse(args);
    public override void OnMouseWheel(MouseWheelEventArgs args) => HandleMouseWheel(args);
    public override void OnMouseMove(MouseEvent args) => HandleMouseMovement(args);
    public override void OnKeyDown(KeyEvent args) => HandleKeyboard(args);
    public override void OnKeyPress(KeyEvent args) => HandleKeyboard(args);
    public override void OnKeyUp(KeyEvent args) => HandleKeyboard(args);

    /// <summary>
    /// Determines if mouse keys inputs were handled
    /// </summary>
    /// <param name="args"></param>
    private static void HandleMouse(MouseEvent args)
    {
        if (!args.Handled) args.Handled = Controller.MouseCaptured();
    }
    /// <summary>
    /// Determines if mouse movement inputs were handled
    /// </summary>
    /// <param name="args"></param>
    private static void HandleMouseMovement(MouseEvent args)
    {
        if (!args.Handled) args.Handled = Controller.MouseMovesCaptured();
    }
    /// <summary>
    /// Determines if mouse wheel inputs were handled
    /// </summary>
    /// <param name="args"></param>
    private static void HandleMouseWheel(MouseWheelEventArgs args)
    {
        if (!args.IsHandled) args.SetHandled(Controller.MouseMovesCaptured());
    }
    /// <summary>
    /// Determines if keyboard inputs were handled
    /// </summary>
    /// <param name="args"></param>
    private void HandleKeyboard(KeyEvent args)
    {
        if (!args.Handled || (args.KeyCode == (int)GlKeys.Escape && !focused))
        {
            args.Handled = Controller.KeyboardCaptured();
        }
    }
    #endregion

    /// <summary>
    /// ImGui controller used to update and render ImGui
    /// </summary>
    private readonly Controller _controller;
    /// <summary>
    /// Manager used to draw ImGui windows and widgets
    /// </summary>
    private readonly DrawCallbacksManager _manager;
    /// <summary>
    /// Whether this dialog should unlock mouse from camera (works only in Immersive Mouse mode)
    /// </summary>
    private bool _grabMouse = false;
    /// <summary>
    /// Draw callback that uses <see cref="_manager"/> to draw ImGui windows and widgets, also determines if dialog should be opened/closed.<br/>
    /// Currently open functionality does not work, because when dialog is closed, it is not rendered, which means that this method is not called.
    /// </summary>
    /// <param name="deltaSeconds">Time it took to render last frame</param>
    private void Draw(float deltaSeconds)
    {
        (bool open, bool grab, bool close) = _manager.Draw(deltaSeconds);

        if (open) TryOpen();
        if (close) TryClose();

        _grabMouse = grab;
    }
}

/// <summary>
/// Used to untie rendering of secondary windows from rendering VS GUI dialog in case of it being closed
/// </summary>
internal sealed class OffWindowRenderer : IRenderer
{
    public OffWindowRenderer(VSImGuiDialog dialog)
    {
        _dialog = dialog;
    }

    /// <summary>
    /// Does not matter for this renderer, cause it is used to render in secondary windows
    /// </summary>
    public double RenderOrder => 0;
    public int RenderRange => 0;

    public void Dispose()
    {
        _disposed = true;
    }
    public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
    {
        if (!_disposed && stage == EnumRenderStage.Ortho)
        {
            _dialog.Update(deltaTime);
            _dialog.RenderOffWindow(deltaTime);
        }
    }

    /// <summary>
    /// VS GUI dialog that is used to integrate ImGui
    /// </summary>
    private readonly VSImGuiDialog _dialog;
    private bool _disposed = false;
}