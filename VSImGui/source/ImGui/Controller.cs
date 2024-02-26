using ImGuiController_OpenTK;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Vintagestory.API.Config;
using VSImGui.src.ImGui;

namespace VSImGui;

public sealed class VSImGuiController : ImGuiController
{
    public VSImGuiController(VSGameWindowWrapper window) : base(window)
    {
        SetConfigFolder();

        mWindowsManager.OnWindowDestroyed += OnWindowDestroyed;

        ImGui.NewFrame();
    }

    public event Action? OnWindowMergedIntoMain;

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

    public void RenderMainWindow(float deltaSeconds)
    {
        mMainWindow.ContextMakeCurrent();
        mMainWindow.OnRender(deltaSeconds);
        mMainWindow.ImGuiRenderer.Render();
        mMainWindow.SwapBuffers();
    }

    public bool KeyboardCaptured()
    {
        return ImGui.GetIO().WantCaptureKeyboard;
    }
    public bool MouseCaptured()
    {
        return ImGui.GetIO().WantCaptureMouse;
    }
    public bool MouseMovesCaptured()
    {
        return ImGui.IsAnyItemHovered();
    }

    private void OnWindowDestroyed(NativeWindow window)
    {
        if (mMainWindow.Native.IsFocused) OnWindowMergedIntoMain?.Invoke();
    }

    protected override void LoadFonts()
    {
        FontManager.Load();
        //ImGui.PushFont(FontManager.Default);
    }

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

public static class FontManager
{
    static public ImFontPtr Default { get; private set; }
    static public ImFontPtr Native { get; private set; }
    static public HashSet<int> Sizes { get; } = new HashSet<int>
    {
        6,
        8,
        10,
        14,
        18,
        24,
        30,
        36,
        48,
        60,
        72
    };
    static public HashSet<string> Fonts { get; } = new HashSet<string>
    {
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Almendra-Bold.otf"),
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Almendra-BoldItalic.otf"),
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Almendra-Italic.otf"),
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Almendra-Regular.otf"),
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Lora-Bold.ttf"),
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Lora-BoldItalic.ttf"),
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Lora-Italic.ttf"),
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Lora-Regular.ttf"),
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Montserrat-Bold.ttf"),
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Montserrat-Italic.ttf"),
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Montserrat-Regular.ttf")
    };
    static public Dictionary<(string, int), ImFontPtr> Loaded { get; } = new();
    internal static void Load()
    {
        Loaded.Clear();
        LoadDefault();

        ImGuiIOPtr io = ImGui.GetIO();
        foreach (string font in Fonts)
        {
            foreach (int size in Sizes)
            {
                ImFontPtr ptr = io.Fonts.AddFontFromFileTTF(font, size);
                Loaded.TryAdd((Path.GetFileNameWithoutExtension(font), size), ptr);
            }
        }
    }
    private static void LoadDefault()
    {
        ImGuiIOPtr io = ImGui.GetIO();
        string defaultFont = Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Montserrat-Regular.ttf");
        int defaultSize = 18;
        Default = io.Fonts.AddFontFromFileTTF(defaultFont, defaultSize);
        Native = io.Fonts.AddFontDefault();
    }
}

public sealed class VSGameWindowWrapper : IWindow
{
    public NativeWindow Native => mWindow;
    internal event Action<float>? Draw;

    public VSGameWindowWrapper(GameWindow window)
    {
        mWindow = window;
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

    private readonly GameWindow mWindow;
    private readonly VSImGuiManager mManager;
}