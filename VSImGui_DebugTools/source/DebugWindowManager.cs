using ImGuiNET;

namespace VSImGui.Debug;

/// <summary>
/// Manages <see cref="DebugWidgets"/> draw calls.
/// </summary>
public static class DebugWindowsManager
{
    /// <summary>
    /// Registered ImGui widgets to draw.<br/> Maps domains to their draw entries collections, and integer ids to draw entries inside these collections.
    /// </summary>
    public static Dictionary<string, Dictionary<int, DrawEntry>> DrawEntries { get; } = new();

    /// <summary>
    /// Draws all domains' windows
    /// </summary>
    /// <param name="deltaSeconds"></param>
    /// <returns></returns>
    public static bool Draw(float deltaSeconds)
    {
        bool anyDrawn = false;

        foreach ((string domain, HashSet<string> categories) in DrawEntry._categories)
        {
            ImGui.Begin(domain);
            ImGui.BeginTabBar(domain);

            DrawCategories(domain, categories, ref anyDrawn);

            ImGui.EndTabBar();
            ImGui.End();
        }

        return anyDrawn;
    }

    /// <summary>
    /// Draws active category
    /// </summary>
    /// <param name="domain"></param>
    /// <param name="categories"></param>
    /// <param name="anyDrawn">Sets to true if any widget was drawn</param>
    private static void DrawCategories(string domain, HashSet<string> categories, ref bool anyDrawn)
    {
        foreach (string category in categories.Where(category => ImGui.BeginTabItem(category)))
        {
            DrawCategory(domain, category, ref anyDrawn);

            ImGui.EndTabItem();
        }
    }

    /// <summary>
    /// Draws all widgets in category
    /// </summary>
    /// <param name="domain"></param>
    /// <param name="category"></param>
    /// <param name="anyDrawn">Sets to true if any widget was drawn</param>
    private static void DrawCategory(string domain, string category, ref bool anyDrawn)
    {
        foreach (DrawEntry entry in DrawEntries[domain].Where(entry => entry.Value.Category == category).Select(entry => entry.Value))
        {
            entry.InvokeDrawCallback();
            anyDrawn = true;
        }
    }

    /// <summary>
    /// Clears stored entries
    /// </summary>
    public static void Clear()
    {
        DrawEntries.Clear();
        DrawEntry._categories.Clear();
    }

    /// <summary>
    /// Holds single draw callback with one widget. Manages categories.
    /// </summary>
    public class DrawEntry
    {
        public string Domain { get; }
        public string Category { get; }
        public Action Delegate { get; }

        internal static readonly Dictionary<string, HashSet<string>> _categories = new();

        /// <summary>
        /// Adds category to collection
        /// </summary>
        /// <param name="domain">Window title</param>
        /// <param name="category">Tab title</param>
        /// <param name="drawDelegate">Callback that draws ImGui widget and processes data</param>
        public DrawEntry(string domain, string category, Action drawDelegate)
        {
            Domain = domain;
            Category = category;
            Delegate = drawDelegate;

            if (!_categories.ContainsKey(domain)) _categories[domain] = new();
            if (!_categories[domain].Contains(category)) _categories[domain].Add(category);
        }

        /// <summary>
        /// Draw associated ImGui widget
        /// </summary>
        public virtual void InvokeDrawCallback()
        {
            Delegate?.Invoke();
        }
    }
}
