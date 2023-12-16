using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

namespace VSImGui
{
    public class VSImGuiModSystem : ModSystem, IRenderer
    {
        public event Action SetUpImGuiWindows;

        private bool mImGuiInitialized = false;
        private ImGuiController mController;
        private ICoreClientAPI mApi;
        private bool mMouseGrabbed;

        public override void StartPre(ICoreAPI api)
        {
            if (api is not ICoreClientAPI clientApi) return;
            EmbeddedDllClass.ExtractEmbeddedDlls();
            EmbeddedDllClass.LoadDll("cimgui.dll");
            mApi = clientApi;
            mImGuiInitialized = InitImGui();
            clientApi.Event.RegisterRenderer(this, EnumRenderStage.Ortho);
        }
        public override void StartClientSide(ICoreClientAPI api)
        {
            mApi = api;

            HarmonyPatches.Patch("vsimgui");
            //HarmonyPatches.SwapBuffersEvent += OnSwapBuffers;
            HarmonyPatches.OnResizeEvent += OnResize;
            HarmonyPatches.OnUpdateFrameEvent += OnUpdateFrame;
            HarmonyPatches.OnTextInputFrameEvent += OnTextInput;
            HarmonyPatches.OnMouseWheelEvent += OnMouseWheel;
            HarmonyPatches.OnUpdateCameraYawPitchEvent += ResetMousePosition;
        }

        public override void Dispose()
        {
            if (mApi != null)
            {
                HarmonyPatches.SwapBuffersEvent -= OnSwapBuffers;
                HarmonyPatches.OnResizeEvent -= OnResize;
                HarmonyPatches.OnUpdateFrameEvent -= OnUpdateFrame;
                HarmonyPatches.OnTextInputFrameEvent -= OnTextInput;
                HarmonyPatches.OnMouseWheelEvent -= OnMouseWheel;
                HarmonyPatches.OnUpdateCameraYawPitchEvent -= ResetMousePosition;

                HarmonyPatches.Unpatch("vsimgui");
            }

            base.Dispose();
        }

        private void OnSwapBuffers()
        {
            if (!mImGuiInitialized)
            {
                mImGuiInitialized = InitImGui();
                if (!mImGuiInitialized) return;
            }

            SetUpImGuiWindows?.Invoke();

            mController.Render();
            ImGuiController.CheckGLError("End of frame");
        }

        private void OnResize(NativeWindow window)
        {
            if (mImGuiInitialized) mController.WindowResized(window.ClientSize.X, window.ClientSize.Y);
        }

        private void OnUpdateFrame(GameWindow window, FrameEventArgs eventData)
        {
            if (mImGuiInitialized) mController.Update(window, (float)eventData.Time, !mMouseGrabbed);
        }

        private void OnTextInput(TextInputEventArgs eventData)
        {
            if (mImGuiInitialized) mController.PressChar((char)eventData.Unicode);
        }

        private void OnMouseWheel(OpenTK.Windowing.Common.MouseWheelEventArgs eventData)
        {
            if (mImGuiInitialized) mController.MouseScroll(eventData.Offset);
        }

        private bool InitImGui()
        {
            if (mApi == null) return false;
            mController = new ImGuiController(mApi.Render.FrameWidth, mApi.Render.FrameHeight);
            return true;
        }

        private void ResetMousePosition(ClientMain client)
        {
            mMouseGrabbed = client.MouseGrabbed;
        }



        public double RenderOrder => 1.01;

        public int RenderRange => 0;

        public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
        {
            OnSwapBuffers();
        }
    }
}
