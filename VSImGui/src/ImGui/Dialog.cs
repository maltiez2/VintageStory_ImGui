using Vintagestory.API.Client;

namespace VSImGui;

public class VSImGuiDialog : GuiDialog
{
    public VSImGuiDialog(ICoreClientAPI capi, VSImGuiController controller, VSGameWindowWrapper windowWrapper) : base(capi)
    {
        mController = controller;
        mWindowWrapper = windowWrapper;
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
    #endregion

    #region Input handling
    public override void OnMouseDown(MouseEvent args) => HandleMouse(args);
    public override void OnMouseUp(MouseEvent args) => HandleMouse(args);
    public override void OnMouseWheel(MouseWheelEventArgs args) => HandleMouse(args);
    public override void OnMouseMove(MouseEvent args) => HandleMouseMovement(args);
    public override void OnKeyDown(KeyEvent args) => HandleKeyboard(args);
    public override void OnKeyPress(KeyEvent args) => HandleKeyboard(args);
    public override void OnKeyUp(KeyEvent args) => HandleKeyboard(args);

    private void HandleMouse(MouseEvent args)
    {
        if (!args.Handled) args.Handled = mController.MouseCaptured();
    }
    private void HandleMouseMovement(MouseEvent args)
    {
        if (!args.Handled) args.Handled = mController.MouseMovesCaptured();
    }
    private void HandleMouse(MouseWheelEventArgs args)
    {
        // nothing to handle
    }
    private void HandleKeyboard(KeyEvent args)
    {
        if (!args.Handled || (args.KeyCode == (int)GlKeys.Escape && !focused))
        {
            args.Handled = mController.KeyboardCaptured();
        }
    }
    #endregion

    private VSImGuiController mController;
    private VSGameWindowWrapper mWindowWrapper;
}

public sealed class OffWindowRenderer : IRenderer
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