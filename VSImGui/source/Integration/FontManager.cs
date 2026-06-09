using ImGuiNET;
using Vintagestory.API.Config;


namespace VSImGui.API;

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
    static internal List<int> Sizes { get; } = new List<int>
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
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Almendra-Bold.ttf"),
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Almendra-BoldItalic.ttf"),
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Almendra-Italic.ttf"),
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Almendra-Regular.ttf"),
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Lora-Bold.ttf"),
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Lora-BoldItalic.ttf"),
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Lora-Italic.ttf"),
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Lora-Regular.ttf"),
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Montserrat-Bold.ttf"),
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Montserrat-Italic.ttf"),
        Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Montserrat-Regular.ttf")
    };
    static private Dictionary<string, nint> GlyphRanges = new()
    {
        { "default", ImGui.GetIO().Fonts.GetGlyphRangesDefault() },
        { "en", ImGui.GetIO().Fonts.GetGlyphRangesDefault() },
        { "ar", ImGui.GetIO().Fonts.GetGlyphRangesDefault() },
        { "be", ImGui.GetIO().Fonts.GetGlyphRangesCyrillic() },
        { "nl", ImGui.GetIO().Fonts.GetGlyphRangesDefault() },
        { "cs", ImGui.GetIO().Fonts.GetGlyphRangesDefault() },
        { "fr", ImGui.GetIO().Fonts.GetGlyphRangesDefault() },
        { "de", ImGui.GetIO().Fonts.GetGlyphRangesDefault() },
        { "eo", ImGui.GetIO().Fonts.GetGlyphRangesDefault() },
        { "it", ImGui.GetIO().Fonts.GetGlyphRangesDefault() },
        { "ja", ImGui.GetIO().Fonts.GetGlyphRangesJapanese() },
        { "ko", ImGui.GetIO().Fonts.GetGlyphRangesKorean() },
        { "pl", ImGui.GetIO().Fonts.GetGlyphRangesDefault() },
        { "pt-pt", ImGui.GetIO().Fonts.GetGlyphRangesDefault() },
        { "pt-br", ImGui.GetIO().Fonts.GetGlyphRangesDefault() },
        { "ru", ImGui.GetIO().Fonts.GetGlyphRangesCyrillic() },
        { "sr", ImGui.GetIO().Fonts.GetGlyphRangesDefault() },
        { "es-es", ImGui.GetIO().Fonts.GetGlyphRangesDefault() },
        { "es-419", ImGui.GetIO().Fonts.GetGlyphRangesDefault() },
        { "sk", ImGui.GetIO().Fonts.GetGlyphRangesDefault() },
        { "sv-se", ImGui.GetIO().Fonts.GetGlyphRangesDefault() },
        { "th", ImGui.GetIO().Fonts.GetGlyphRangesDefault() },
        { "uk", ImGui.GetIO().Fonts.GetGlyphRangesCyrillic() },
        { "zh-cn", ImGui.GetIO().Fonts.GetGlyphRangesChineseFull() },
        { "tr", ImGui.GetIO().Fonts.GetGlyphRangesDefault() }
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
        HashSet<int> sizes = [.. Sizes];
        HashSet<string> fonts = [.. Fonts];

        BeforeFontsLoaded?.Invoke(fonts, sizes);

        Loaded.Clear();
        LoadDefault();

        ImGuiIOPtr io = ImGui.GetIO();
        foreach (string font in fonts)
        {
            foreach (int size in sizes)
            {
                nint glyphRange = GlyphRanges.ContainsKey(Lang.CurrentLocale) ? GlyphRanges[Lang.CurrentLocale] : GlyphRanges["default"];
                ImFontPtr ptr = io.Fonts.AddFontFromFileTTF(font, size, new ImFontConfigPtr(), glyphRange);
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
        nint glyphRange = GlyphRanges.ContainsKey(Lang.CurrentLocale) ? GlyphRanges[Lang.CurrentLocale] : GlyphRanges["default"];
        _ = io.Fonts.AddFontFromFileTTF(_defaultFont, _defaultSize, new ImFontConfigPtr(), glyphRange);
        _ = io.Fonts.AddFontDefault();
    }
}