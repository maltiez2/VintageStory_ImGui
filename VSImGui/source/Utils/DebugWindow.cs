using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using VSImGui.API;

namespace VSImGui;

/// <summary>
/// Collection of methods for building small, fast to make, one-line debug tools that can be called from anywhere in code any number of times.<br/><br/>
/// Tools are drawn in ImGui windows with specified domain used as id and title, in tabs with specified category used as title and id.<br/><br/>
/// Widgets that require integer id use it as widget id to apply changes to existing widget instead of registering new one on subsequent calls.<br/>
/// Other widgets use label as their widget id and its hash as integer id that <see cref="DebugWindow.Remove(string, int)"/> method accepts.
/// </summary>
public static class DebugWindow
{
    public static void Draw(string domain, string category, int id, Action drawDelegate)
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, drawDelegate);
    }

    public static void Remove(string domain, int id) => DebugWindowsManager.DrawEntries[domain].Remove(id);

    public static int Button(string domain, string category, string label, Action<bool> setter)
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        int id = label.GetHashCode();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () =>
        {
            setter?.Invoke(ImGui.Button(label));
        });
        return id;
    }

    public static int CheckBox(string domain, string category, string label, System.Func<bool> getter, Action<bool> setter)
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        int id = label.GetHashCode();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () =>
        {
            bool value = getter?.Invoke() ?? false;
            ImGui.Checkbox(label, ref value);
            setter?.Invoke(value);
        });
        return id;
    }

    public static int IntSlider(string domain, string category, string label, int min, int max, System.Func<int> getter, Action<int> setter)
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        int id = label.GetHashCode();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () =>
        {
            int value = getter?.Invoke() ?? 0;
            ImGui.SliderInt(label, ref value, min, max);
            setter?.Invoke(value);
        });
        return id;
    }

    public static int FloatSlider(string domain, string category, string label, float min, float max, System.Func<float> getter, Action<float> setter)
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        int id = label.GetHashCode();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () =>
        {
            float value = getter?.Invoke() ?? 0;
            ImGui.SliderFloat(label, ref value, min, max);
            setter?.Invoke(value);
        });
        return id;
    }

    public static int Text(string domain, string category, int id, string text)
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () => ImGui.Text(text));
        return id;
    }

    public static int TextInput(string domain, string category, string label, System.Func<string> getter, Action<string> setter, uint maxLength = 512)
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        int id = label.GetHashCode();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () =>
        {
            string value = getter?.Invoke() ?? "";
            ImGui.InputText(label, ref value, maxLength);
            setter?.Invoke(value);
        });
        return id;
    }

    public static int TextInputMultiline(string domain, string category, string label, System.Func<string> getter, Action<string> setter, uint maxLength = 512, ImGuiInputTextFlags flags = ImGuiInputTextFlags.None, float width = 0, float height = 0)
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        int id = label.GetHashCode();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () =>
        {
            string value = getter?.Invoke() ?? "";
            ImGui.InputTextMultiline(label, ref value, maxLength, new(width, height), flags);
            setter?.Invoke(value);
        });
        return id;
    }

    public static int IntDrag(string domain, string category, string label, System.Func<int> getter, Action<int> setter)
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        int id = label.GetHashCode();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () =>
        {
            int value = getter?.Invoke() ?? 0;
            ImGui.DragInt(label, ref value);
            setter?.Invoke(value);
        });
        return id;
    }

    public static int FloatDrag(string domain, string category, string label, System.Func<float> getter, Action<float> setter)
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        int id = label.GetHashCode();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () =>
        {
            float value = getter?.Invoke() ?? 0;
            ImGui.DragFloat(label, ref value);
            setter?.Invoke(value);
        });
        return id;
    }

    public static int Float3Drag(string domain, string category, string label, System.Func<Value3> getter, Action<Value3> setter)
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        int id = label.GetHashCode();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () =>
        {
            Vector3 value = getter?.Invoke() ?? new(0,0,0);
            ImGui.DragFloat3(label, ref value);
            setter?.Invoke(value);
        });
        return id;
    }

    public static int ColorPicker(string domain, string category, string label, System.Func<Value4> getter, Action<Value4> setter, ImGuiColorEditFlags flags = ImGuiColorEditFlags.None)
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        int id = label.GetHashCode();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () =>
        {
            Vector4 value = getter?.Invoke() ?? new(0, 0, 0, 0);
            ImGui.ColorEdit4(label, ref value, flags);
            setter?.Invoke(value);
        });
        return id;
    }

    public static int EnumCombo<TEnum>(string domain, string category, string label, System.Func<TEnum> getter, Action<TEnum> setter)
        where TEnum : struct, Enum
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        int id = label.GetHashCode();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () =>
        {
            TEnum value = getter.Invoke();
            EnumEditor<TEnum>.Combo(label, ref value);
            setter?.Invoke(value);
        });
        return id;
    }

    public static int EnumList<TEnum>(string domain, string category, string label, System.Func<TEnum> getter, Action<TEnum> setter)
        where TEnum : struct, Enum
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        int id = label.GetHashCode();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () =>
        {
            TEnum value = getter.Invoke();
            value = EnumEditor<TEnum>.List(label, value);
            setter?.Invoke(value);
        });
        return id;
    }

    public static int EnumList<TEnum>(string domain, string category, string label, string filterHolder, System.Func<TEnum> getter, Action<TEnum> setter)
        where TEnum : struct, Enum
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        int id = label.GetHashCode();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () =>
        {
            TEnum value = getter.Invoke();
            value = EnumEditor<TEnum>.ListWithFilter(label, filterHolder, value);
            setter?.Invoke(value);
        });
        return id;
    }

    public static int StringList(string domain, string category, string label, string filterHolder, string[] list, System.Func<int> getter, Action<int> indexSetter, Action<string> valueSetter = null)
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        int id = label.GetHashCode();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () =>
        {
            int index = getter.Invoke();
            TextEditor.ListWithFilter(label, filterHolder, list, ref index, out string value);
            indexSetter?.Invoke(index);
            valueSetter?.Invoke(value);
        });
        return id;
    }
    public static int StringList(string domain, string category, string label, string filterHolder, string[] list, System.Func<int> getter, Action<string> valueSetter, Action<int> indexSetter = null)
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        int id = label.GetHashCode();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () =>
        {
            int index = getter.Invoke();
            TextEditor.ListWithFilter(label, filterHolder, list, ref index, out string value);
            indexSetter?.Invoke(index);
            valueSetter?.Invoke(value);
        });
        return id;
    }
}

/// <summary>
/// Manages <see cref="DebugWindow"/> draw calls.
/// </summary>
public static class DebugWindowsManager
{
    /// <summary>
    /// Registered ImGui widgets to draw
    /// </summary>
    public static Dictionary<string, Dictionary<int, DrawEntry>> DrawEntries { get; } = new();

    /// <summary>
    /// Draws all domains' windows
    /// </summary>
    /// <param name="deltaSeconds"></param>
    /// <returns></returns>
    internal static CallbackGUIStatus Draw(float deltaSeconds)
    {
        bool anyDrawn = false;
        
        foreach ((string domain, HashSet<string> categories) in DrawEntry.Categories)
        {
            ImGui.Begin(domain);
            ImGui.BeginTabBar(domain);

            DrawCategories(domain, categories, ref anyDrawn);

            ImGui.EndTabBar();
            ImGui.End();
        }

        return anyDrawn ? CallbackGUIStatus.DontGrabMouse : CallbackGUIStatus.Closed;
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
    internal static void Clear()
    {
        DrawEntries.Clear();
        DrawEntry.Categories.Clear();
    }

    /// <summary>
    /// Holds single draw callback with one widget. Manages categories.
    /// </summary>
    public class DrawEntry
    {
        public string Domain { get; }
        public string Category { get; }
        public Action Delegate { get; }

        public static readonly Dictionary<string, HashSet<string>> Categories = new();

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

            if (!Categories.ContainsKey(domain)) Categories[domain] = new();
            if (!Categories[domain].Contains(category)) Categories[domain].Add(category);
        }

        /// <summary>
        /// Draw associated ImGui widget
        /// </summary>
        public void InvokeDrawCallback()
        {
            Delegate?.Invoke();
        }
    }
}
