using ImGuiNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// Should contain all the ImGui calls to draw windows and widgets
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
/// Defines what fonts and sizes of fonts will be loaded 
/// </summary>
public static class FontManager
{
    /// <summary>
    /// Provides access to fonts and sizes collections that determine what fonts will be loaded. Font atlas is limited, so to many fonts and sizes might cause problems.
    /// </summary>
    /// <param name="fonts">Collection of paths to fonts' files. Add your own fonts here.</param>
    /// <param name="sizes">Collection of sizes to generate. Add your sizes here.</param>
    public delegate void FontsLoadingDelegate(HashSet<string> fonts, HashSet<int> sizes);

    /// <summary>
    /// Called right before fonts are loaded
    /// </summary>
    public static event FontsLoadingDelegate? BeforeFontsLoaded;

    /// <summary>
    /// Returns all the loaded fonts
    /// </summary>
    /// <returns>List of pairs: font name to size</returns>
    public static IEnumerable<(string font, int size)> GetLoadedFonts() => Loaded.Select(entry => (entry.Key.Item1, entry.Key.Item2));

    /// <summary>
    /// List of sizes that would be generated for each font
    /// </summary>
    static internal HashSet<int> Sizes { get; } = new HashSet<int>
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
        60
    };
    /// <summary>
    /// List of paths to font files that would be loaded
    /// </summary>
    static internal HashSet<string> Fonts { get; } = new HashSet<string>
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
    /// <summary>
    /// Maps all combinations of loaded fonts to pointers that hold them and are used to set current font
    /// </summary>
    static internal Dictionary<(string, int), ImFontPtr> Loaded { get; } = new();
    /// <summary>
    /// Loads all the fonts for all combinations of font paths and sizes from corresponding collections. Also loads default font.
    /// </summary>
    internal static void Load()
    {
        HashSet<int> sizes = Sizes.Select(value => value).ToHashSet();
        HashSet<string> fonts = Fonts.Select(value => (string)value.Clone()).ToHashSet();

        BeforeFontsLoaded?.Invoke(fonts, sizes);

        Loaded.Clear();
        LoadDefault();

        ImGuiIOPtr io = ImGui.GetIO();
        foreach (string font in fonts)
        {
            foreach (int size in sizes)
            {
                ImFontPtr ptr = io.Fonts.AddFontFromFileTTF(font, size);
                Loaded.TryAdd((Path.GetFileNameWithoutExtension(font), size), ptr);
            }
        }
    }
    /// <summary>
    /// Font that will be loaded and used by default
    /// </summary>
    static private readonly string _defaultFont = Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Montserrat-Regular.ttf");
    /// <summary>
    /// Default font will have this size
    /// </summary>
    static private readonly int _defaultSize = 18;
    /// <summary>
    /// Loads and sets defaults font
    /// </summary>
    private static void LoadDefault()
    {
        ImGuiIOPtr io = ImGui.GetIO();
        _ = io.Fonts.AddFontFromFileTTF(_defaultFont, _defaultSize);
        _ = io.Fonts.AddFontDefault();
    }
}