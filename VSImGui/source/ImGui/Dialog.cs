using System;
using Vintagestory.API.Client;

namespace VSImGui;

internal class VSImGuiDialog : GuiDialog
{
    public VSImGuiDialog(ICoreClientAPI capi, Controller controller, MainGameWindowWrapper windowWrapper, DrawCallbacksManager manager) : base(capi)
    {
        mController = controller;
        mManager = manager;

        windowWrapper.Draw += Draw;
    }

    #region Controller calls
    public void Update(float deltaTime)
    {
        mController.Update(deltaTime, opened);
    }
    public void RenderOffWindow(float deltaTime)
    {
        mController.RenderOffWindow(deltaTime);
    }
    public override void OnRenderGUI(float deltaTime)
    {
        mController.RenderMainWindow(deltaTime);
    }
    #endregion

    #region Vanilla dialoug settings
    public override string ToggleKeyCombinationCode => "imguitoggle";
    public override bool ShouldReceiveRenderEvents() => true;
    public override bool PrefersUngrabbedMouse => mGrabMouse; // In case of ImGui: grab means 'grab by GUI', opposite of game's meaning
    #endregion

    #region Input handling
    public override void OnMouseDown(MouseEvent args) => HandleMouse(args);
    public override void OnMouseUp(MouseEvent args) => HandleMouse(args);
    public override void OnMouseWheel(MouseWheelEventArgs args) => HandleMouseWheel(args);
    public override void OnMouseMove(MouseEvent args) => HandleMouseMovement(args);
    public override void OnKeyDown(KeyEvent args) => HandleKeyboard(args);
    public override void OnKeyPress(KeyEvent args) => HandleKeyboard(args);
    public override void OnKeyUp(KeyEvent args) => HandleKeyboard(args);

    private void HandleMouse(MouseEvent args)
    {
        if (!args.Handled) args.Handled = Controller.MouseCaptured();
    }
    private void HandleMouseMovement(MouseEvent args)
    {
        if (!args.Handled) args.Handled = Controller.MouseMovesCaptured();
    }
    private void HandleMouseWheel(MouseWheelEventArgs args)
    {
        if (!args.IsHandled) args.SetHandled(Controller.MouseMovesCaptured());
    }
    private void HandleKeyboard(KeyEvent args)
    {
        if (!args.Handled || (args.KeyCode == (int)GlKeys.Escape && !focused))
        {
            args.Handled = Controller.KeyboardCaptured();
        }
    }
    #endregion

    private readonly Controller mController;
    private readonly DrawCallbacksManager mManager;

    private bool mGrabMouse = false;

    private void Draw(float deltaSeconds)
    {
        (bool open, bool grab, bool close) = mManager.Draw(deltaSeconds);

        if (open) TryOpen();
        if (close) TryClose();

        mGrabMouse = grab;
    }
}

internal sealed class OffWindowRenderer : IRenderer
{

    public OffWindowRenderer(VSImGuiDialog dialog)
    {
        mDialog = dialog;
    }
    public double RenderOrder => 0;

    public int RenderRange => 0;

    public void Dispose()
    {
        mDisposed = true;
    }
    public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
    {
        if (!mDisposed && stage == EnumRenderStage.Ortho)
        {
            mDialog.Update(deltaTime);
            mDialog.RenderOffWindow(deltaTime);
        }
    }

    private readonly VSImGuiDialog mDialog;
    private bool mDisposed = false;
}