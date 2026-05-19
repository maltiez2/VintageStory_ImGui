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

/// <summary>
/// Defines what fonts and sizes of fonts will be loaded 
/// </summary>
public static class FontManager
{
    /// <summary>
    /// Detects whether the currently loaded ImGui runtime exposes the 1.92 dynamic glyph-range feature so we can skip manual ranges when safe.
    /// </summary>
    static FontManager()
    {
        if (Version.TryParse(ImGui.GetVersion(), out Version? parsedVersion))
        {
            _supportsFullUnicodeGlyphs = parsedVersion >= new Version(1, 92);
        }
    }
    /// <summary>
    /// Whether ImGui 1.92+ is in use, enabling its dynamic glyph-range support; see https://github.com/ocornut/imgui/issues/8465 for context.
    /// </summary>
    private static readonly bool _supportsFullUnicodeGlyphs;
    /// <summary>
    /// Locale-specific factories that can build additional glyph ranges when ImGui defaults lack certain characters.
    /// </summary>
    private static readonly Dictionary<string, Func<ImGuiIOPtr, ImVector>> CustomGlyphBuilders = new()
    {
        { "pl", BuildPolishGlyphRange }
    };
    /// <summary>
    /// Keeps custom glyph range buffers alive so ImGui can safely reference their memory.
    /// </summary>
    private static readonly List<ImVector> CustomGlyphRanges = new();
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
        { "pl", IntPtr.Zero },
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
        { "zh-cn", ImGui.GetIO().Fonts.GetGlyphRangesChineseFull() }
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
        nint glyphRange = ResolveGlyphRange(io);
        foreach (string font in fonts)
        {
            foreach (int size in sizes)
            {
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
        nint glyphRange = ResolveGlyphRange(io);
        _ = io.Fonts.AddFontFromFileTTF(_defaultFont, _defaultSize, new ImFontConfigPtr(), glyphRange);
        _ = io.Fonts.AddFontDefault();
    }

    /// <summary>
    /// Determines the glyph range pointer that should be used for the current locale, falling back to builders or defaults when needed.
    /// </summary>
    private static nint ResolveGlyphRange(ImGuiIOPtr io)
    {
        if (_supportsFullUnicodeGlyphs)
        {
            return IntPtr.Zero; // ImGui 1.92+ automatically loads all glyphs, no manual range required
        }

        string locale = (Lang.CurrentLocale ?? "en").ToLowerInvariant();

        if (GlyphRanges.TryGetValue(locale, out nint range) && range != IntPtr.Zero)
        {
            return range;
        }

        if (CustomGlyphBuilders.TryGetValue(locale, out Func<ImGuiIOPtr, ImVector>? builder))
        {
            ImVector vector = builder(io);
            CustomGlyphRanges.Add(vector); // keep vector alive so ImGui can use the range memory
            nint ptr = vector.Data;
            GlyphRanges[locale] = ptr;
            return ptr;
        }

        int dashIndex = locale.IndexOf('-');
        if (dashIndex > 0)
        {
            string fallback = locale[..dashIndex];
            return ResolveGlyphRangeForFallback(fallback, io);
        }

        return GlyphRanges["en"];
    }

    /// <summary>
    /// Resolves glyph ranges for shortened locale names (e.g. "pl" extracted from "pl-PL").
    /// </summary>
    private static nint ResolveGlyphRangeForFallback(string locale, ImGuiIOPtr io)
    {
        if (_supportsFullUnicodeGlyphs)
        {
            return IntPtr.Zero;
        }

        if (GlyphRanges.TryGetValue(locale, out nint range) && range != IntPtr.Zero)
        {
            return range;
        }

        if (CustomGlyphBuilders.TryGetValue(locale, out Func<ImGuiIOPtr, ImVector>? builder))
        {
            ImVector vector = builder(io);
            CustomGlyphRanges.Add(vector);
            nint ptr = vector.Data;
            GlyphRanges[locale] = ptr;
            return ptr;
        }

        return GlyphRanges["en"];
    }

    /// <summary>
    /// Builds a glyph range that covers Polish diacritics by merging default, Cyrillic and explicit characters.
    /// </summary>
    private static unsafe ImVector BuildPolishGlyphRange(ImGuiIOPtr io)
    {
        if (_supportsFullUnicodeGlyphs)
        {
            return default;
        }

        ImFontGlyphRangesBuilderPtr builder = new(ImGuiNative.ImFontGlyphRangesBuilder_ImFontGlyphRangesBuilder());
        builder.AddRanges(io.Fonts.GetGlyphRangesDefault());
        builder.AddRanges(io.Fonts.GetGlyphRangesCyrillic());
        builder.AddText("ĄąĆćĘęŁłŃńÓóŚśŹźŻż");
        ImVector ranges;
        builder.BuildRanges(out ranges);
        ImGuiNative.ImFontGlyphRangesBuilder_destroy(builder.NativePtr);
        return ranges;
    }
}
