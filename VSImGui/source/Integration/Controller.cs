using ImGuiController_OpenTK;
using ImGuiNET;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Vintagestory.API.Client;
using Vintagestory.API.Config;
using Vintagestory.Client.NoObf;
using VSImGui.API;

namespace VSImGui;

/// <summary>
/// Initializes ImGui, updates inputs for it, manages classes that render ImGui and manage native windows
/// </summary>
internal sealed class Controller : ImGuiController
{
    /// <summary>
    /// Initializes ImGui
    /// </summary>
    /// <param name="window">Main game window wrapped in specialized class</param>
    public Controller(MainGameWindowWrapper window) : base(window)
    {
        SetConfigFolder();

        mWindowsManager.OnWindowDestroyed += OnWindowDestroyed;

        ImGui.NewFrame();
    }

    /// <summary>
    /// Invoked when secondary window is dragged into main one and destroyed
    /// </summary>
    public event Action? OnWindowMergedIntoMain;

    /// <summary>
    /// Updates ImGui inputs and windows parameters
    /// </summary>
    /// <param name="deltaSeconds">Time it took to render last frame</param>
    /// <param name="captureInputs">If ImGUi should capture inputs from main window</param>
    public void Update(float deltaSeconds, bool captureInputs)
    {
        ImGui.UpdatePlatformWindows();

        List<IImGuiWindow> windows = GetWindows(true);

        foreach (IImGuiWindow window in windows)
        {
            MonitorInfo monitor = Monitors.GetMonitorFromWindow(window.Native);
            window.Viewport.Pos = new System.Numerics.Vector2(monitor.ClientArea.Min.X, monitor.ClientArea.Min.Y);
            window.Viewport.Size = new System.Numerics.Vector2(monitor.ClientArea.Size.X, monitor.ClientArea.Size.Y);
            window.OnUpdate(deltaSeconds);
        }

        UpdateImGuiInput(captureInputs);
        SetPerFrameImGuiData(deltaSeconds, mMainWindow.Native);

        UpdateMonitors();
        ImGui.NewFrame();

        foreach (IImGuiWindow window in windows)
        {
            window.OnDraw(deltaSeconds);
        }

        ImGui.Render();
    }

    /// <summary>
    /// Renders ImGui into secondary windows
    /// </summary>
    /// <param name="deltaSeconds">Time it took to render last frame</param>
    public void RenderOffWindow(float deltaSeconds)
    {
        List<IImGuiWindow> windows = GetWindows(false);

        foreach (IImGuiWindow window in windows)
        {
            window.ContextMakeCurrent();
            window.OnRender(deltaSeconds);
            window.ImGuiRenderer.Render();
            window.SwapBuffers();
        }

        mMainWindow.Native.MakeCurrent();
    }

    /// <summary>
    /// Renders ImGui into main game windows
    /// </summary>
    /// <param name="deltaSeconds">Time it took to render last frame</param>
    public void RenderMainWindow(float deltaSeconds)
    {
        mMainWindow.ContextMakeCurrent();
        mMainWindow.OnRender(deltaSeconds);
        mMainWindow.ImGuiRenderer.Render();
        mMainWindow.SwapBuffers();
    }

    /// <summary>
    /// Returns whether ImGui wants to capture keyboard
    /// </summary>
    /// <returns>True if ImGui handled keyboard inputs</returns>
    public static bool KeyboardCaptured()
    {
        return ImGui.GetIO().WantCaptureKeyboard;
    }
    /// <summary>
    /// Returns whether ImGui wants to capture mouse buttons
    /// </summary>
    /// <returns>True if ImGui handled mouse buttons inputs</returns>
    public static bool MouseCaptured()
    {
        return ImGui.GetIO().WantCaptureMouse;
    }
    /// <summary>
    /// Returns whether ImGui wants to capture mouse movement
    /// </summary>
    /// <returns>True if ImGui handled mouse movement inputs</returns>
    public static bool MouseMovesCaptured()
    {
        return ImGui.IsAnyItemHovered();
    }

    /// <summary>
    /// Called when secondary window is dragged into main and destroyed
    /// </summary>
    /// <param name="window"></param>
    private void OnWindowDestroyed(NativeWindow window)
    {
        if (mMainWindow.Native.IsFocused) OnWindowMergedIntoMain?.Invoke();
    }

    /// <summary>
    /// Called at the right time to load fonts. Uses FontManager to load fonts
    /// </summary>
    protected override void LoadFonts()
    {
        FontManager.Load();
    }

    /// <summary>
    /// Sets ModConfig folder as path to ImGui config file that store all the info about ImGui windows arrangement
    /// </summary>
    private void SetConfigFolder()
    {
        ImGuiIOPtr io = ImGui.GetIO();
        string config = Path.Combine(GamePaths.ModConfig, "imgui.ini");
        unsafe
        {
            IntPtr ptr = Marshal.StringToHGlobalAnsi(config);
            io.NativePtr->IniFilename = (byte*)ptr.ToPointer();
        }
    }
}

/// <summary>
/// Wraps main game window for use in <see cref="Controller"/>
/// </summary>
internal sealed class MainGameWindowWrapper : IWindow
{
    public NativeWindow Native => _window;
    internal event Action<float>? Draw;

    public MainGameWindowWrapper(ICoreClientAPI clientApi)
    {
        FieldInfo? field = typeof(ClientMain).GetField("Platform", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
        ClientPlatformWindows? platform = (ClientPlatformWindows?)field?.GetValue(clientApi.World as ClientMain);
        _window = platform?.window ?? throw new InvalidOperationException("Unable to get game native window from client api");
    }

    public void ContextMakeCurrent()
    {
        // Should be set at this point
    }
    public void Dispose()
    {
        // Not owning window itself
    }
    public void OnDraw(float deltaSeconds) => Draw?.Invoke(deltaSeconds);
    public void OnRender(float deltaSeconds)
    {
        // nothing to render
    }
    public void OnUpdate(float deltaSeconds)
    {
        // nothing to update
    }
    public void SwapBuffers()
    {
        // swapped by game itself
    }

    private readonly GameWindowNative _window;
}