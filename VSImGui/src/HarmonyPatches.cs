using HarmonyLib;
using ImGuiNET;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using Vintagestory.Client;
using Vintagestory.Client.NoObf;

namespace VSImGui
{
    static class HarmonyPatches
    {
        static public event Action SwapBuffersEvent;
        static public event Action<NativeWindow> OnResizeEvent;
        static public event Action<GameWindow, FrameEventArgs> OnUpdateFrameEvent;
        static public event Action<TextInputEventArgs> OnTextInputFrameEvent;
        static public event Action<MouseWheelEventArgs> OnMouseWheelEvent;
        static public event Action<ClientMain> OnUpdateCameraYawPitchEvent;

        public static void Patch(string harmonyId)
        {
            {
                var OriginalMethod = AccessTools.Method(typeof(GameWindow), nameof(GameWindow.SwapBuffers));
                var PrefixMethod = AccessTools.Method(typeof(HarmonyPatches), nameof(SwapBuffers));
                new Harmony(harmonyId).Patch(OriginalMethod, prefix: new HarmonyMethod(PrefixMethod));
            }

            {
                var OriginalMethod = typeof(NativeWindow).GetMethod("OnResize", AccessTools.all);
                var PrefixMethod = AccessTools.Method(typeof(HarmonyPatches), nameof(OnResize));
                new Harmony(harmonyId).Patch(OriginalMethod, postfix: new HarmonyMethod(PrefixMethod));
            }

            {
                var OriginalMethod = typeof(GameWindow).GetMethod("OnUpdateFrame", AccessTools.all);
                var PrefixMethod = AccessTools.Method(typeof(HarmonyPatches), nameof(OnUpdateFrame));
                new Harmony(harmonyId).Patch(OriginalMethod, prefix: new HarmonyMethod(PrefixMethod));
            }
            {
                var OriginalMethod = typeof(NativeWindow).GetMethod("OnTextInput", AccessTools.all);
                var PrefixMethod = AccessTools.Method(typeof(HarmonyPatches), nameof(OnTextInputFrame));
                new Harmony(harmonyId).Patch(OriginalMethod, prefix: new HarmonyMethod(PrefixMethod));
            }
            {
                var OriginalMethod = typeof(NativeWindow).GetMethod("OnMouseWheel", AccessTools.all);
                var PrefixMethod = AccessTools.Method(typeof(HarmonyPatches), nameof(OnMouseWheel));
                new Harmony(harmonyId).Patch(OriginalMethod, prefix: new HarmonyMethod(PrefixMethod));
            }

            new Harmony(harmonyId).Patch(typeof(ClientPlatformWindows).GetMethod("Mouse_WheelChanged", AccessTools.all), prefix: AccessTools.Method(typeof(HarmonyPatches), nameof(MouseHandled)));
            new Harmony(harmonyId).Patch(typeof(ClientPlatformWindows).GetMethod("Mouse_ButtonDown", AccessTools.all), prefix: AccessTools.Method(typeof(HarmonyPatches), nameof(MouseHandled)));
            //new Harmony(harmonyId).Patch(typeof(ClientPlatformWindows).GetMethod("Mouse_ButtonUp", AccessTools.all), prefix: AccessTools.Method(typeof(HarmonyPatches), nameof(MouseHandled)));
            new Harmony(harmonyId).Patch(typeof(ClientPlatformWindows).GetMethod("game_KeyPress", AccessTools.all), prefix: AccessTools.Method(typeof(HarmonyPatches), nameof(KeyHandled)));
            new Harmony(harmonyId).Patch(typeof(ClientPlatformWindows).GetMethod("game_KeyDown", AccessTools.all), prefix: AccessTools.Method(typeof(HarmonyPatches), nameof(KeyHandled)));
            //new Harmony(harmonyId).Patch(typeof(ClientPlatformWindows).GetMethod("game_KeyUp", AccessTools.all), prefix: AccessTools.Method(typeof(HarmonyPatches), nameof(KeyHandled)));

            new Harmony(harmonyId).Patch(typeof(ClientMain).GetMethod("UpdateCameraYawPitch", AccessTools.all), prefix: AccessTools.Method(typeof(HarmonyPatches), nameof(ResetMousePosition)));

            new Harmony(harmonyId).Patch(typeof(ClientPlatformWindows).GetMethod("window_RenderFrame", AccessTools.all), prefix: AccessTools.Method(typeof(HarmonyPatches), nameof(RenderFrame)));

        }

        public static void Unpatch(string harmonyId)
        {
            new Harmony(harmonyId).Unpatch(AccessTools.Method(typeof(GameWindow), nameof(GameWindow.SwapBuffers)), HarmonyPatchType.Prefix, harmonyId);
            new Harmony(harmonyId).Unpatch(typeof(NativeWindow).GetMethod("OnResize", AccessTools.all), HarmonyPatchType.Postfix, harmonyId);
            new Harmony(harmonyId).Unpatch(typeof(GameWindow).GetMethod("OnUpdateFrame", AccessTools.all), HarmonyPatchType.Prefix, harmonyId);
            new Harmony(harmonyId).Unpatch(typeof(NativeWindow).GetMethod("OnTextInput", AccessTools.all), HarmonyPatchType.Prefix, harmonyId);
            new Harmony(harmonyId).Unpatch(typeof(NativeWindow).GetMethod("OnMouseWheel", AccessTools.all), HarmonyPatchType.Prefix, harmonyId);

            new Harmony(harmonyId).Unpatch(typeof(ClientPlatformWindows).GetMethod("Mouse_WheelChanged", AccessTools.all), HarmonyPatchType.Prefix, harmonyId);
            new Harmony(harmonyId).Unpatch(typeof(ClientPlatformWindows).GetMethod("Mouse_ButtonDown", AccessTools.all), HarmonyPatchType.Prefix, harmonyId);
            //new Harmony(harmonyId).Unpatch(typeof(ClientPlatformWindows).GetMethod("Mouse_ButtonUp", AccessTools.all), HarmonyPatchType.Prefix, harmonyId);
            new Harmony(harmonyId).Unpatch(typeof(ClientPlatformWindows).GetMethod("game_KeyPress", AccessTools.all), HarmonyPatchType.Prefix, harmonyId);
            new Harmony(harmonyId).Unpatch(typeof(ClientPlatformWindows).GetMethod("game_KeyDown", AccessTools.all), HarmonyPatchType.Prefix, harmonyId);
            //new Harmony(harmonyId).Unpatch(typeof(ClientPlatformWindows).GetMethod("game_KeyUp", AccessTools.all), HarmonyPatchType.Prefix, harmonyId);

            new Harmony(harmonyId).Unpatch(typeof(ClientPlatformWindows).GetMethod("window_RenderFrame", AccessTools.all), HarmonyPatchType.Prefix, harmonyId);
        }

        public static void RenderFrame()
        {
            //ImGui.DockSpaceOverViewport();
        }

        public static void SwapBuffers()
        {
            SwapBuffersEvent?.Invoke();
        }
        public static void OnResize(NativeWindow __instance)
        {
            OnResizeEvent?.Invoke(__instance);
        }
        public static void OnUpdateFrame(GameWindow __instance, FrameEventArgs args)
        {
            OnUpdateFrameEvent?.Invoke(__instance, args);
        }
        public static void OnTextInputFrame(TextInputEventArgs e)
        {
            OnTextInputFrameEvent?.Invoke(e);
        }
        public static void OnMouseWheel(MouseWheelEventArgs e)
        {
            OnMouseWheelEvent?.Invoke(e);
        }

        public static bool KeyHandled()
        {
            return !ImGui.GetIO().WantCaptureKeyboard || !ImGui.GetIO().WantCaptureMouse;
        }
        public static bool MouseHandled()
        {
            return !ImGui.GetIO().WantCaptureMouse;
        }

        public static void ResetMousePosition(ClientMain __instance)
        {
            OnUpdateCameraYawPitchEvent?.Invoke(__instance);
        }
    }
}
 