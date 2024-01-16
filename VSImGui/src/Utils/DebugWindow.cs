using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace VSImGui;

public static class DebugWindow
{
    public static void Draw(string domain, string category, int id, Action drawDelegate)
    {
        if (!sDrawEntries.ContainsKey(domain)) sDrawEntries[domain] = new();
        sDrawEntries[domain][id] = new(domain, category, drawDelegate);
    }

    public static void Remove(string domain, int id) => sDrawEntries[domain].Remove(id);

    public static int Button(string domain, string category, string label, Action<bool> setter)
    {
        if (!sDrawEntries.ContainsKey(domain)) sDrawEntries[domain] = new();
        int id = label.GetHashCode();
        sDrawEntries[domain][id] = new(domain, category, () =>
        {
            setter?.Invoke(ImGui.Button(label));
        });
        return id;
    }

    public static int CheckBox(string domain, string category, string label, System.Func<bool> getter, Action<bool> setter)
    {
        if (!sDrawEntries.ContainsKey(domain)) sDrawEntries[domain] = new();
        int id = label.GetHashCode();
        sDrawEntries[domain][id] = new(domain, category, () =>
        {
            bool value = getter?.Invoke() ?? false;
            ImGui.Checkbox(label, ref value);
            setter?.Invoke(value);
        });
        return id;
    }

    public static int IntSlider(string domain, string category, string label, int min, int max, System.Func<int> getter, Action<int> setter)
    {
        if (!sDrawEntries.ContainsKey(domain)) sDrawEntries[domain] = new();
        int id = label.GetHashCode();
        sDrawEntries[domain][id] = new(domain, category, () =>
        {
            int value = getter?.Invoke() ?? 0;
            ImGui.SliderInt(label, ref value, min, max);
            setter?.Invoke(value);
        });
        return id;
    }

    public static int FloatSlider(string domain, string category, string label, float min, float max, System.Func<float> getter, Action<float> setter)
    {
        if (!sDrawEntries.ContainsKey(domain)) sDrawEntries[domain] = new();
        int id = label.GetHashCode();
        sDrawEntries[domain][id] = new(domain, category, () =>
        {
            float value = getter?.Invoke() ?? 0;
            ImGui.SliderFloat(label, ref value, min, max);
            setter?.Invoke(value);
        });
        return id;
    }

    public static int Text(string domain, string category, int id, string text)
    {
        if (!sDrawEntries.ContainsKey(domain)) sDrawEntries[domain] = new();
        sDrawEntries[domain][id] = new(domain, category, () => ImGui.Text(text));
        return id;
    }

    public static int TextInput(string domain, string category, string label, System.Func<string> getter, Action<string> setter, uint maxLength = 512)
    {
        if (!sDrawEntries.ContainsKey(domain)) sDrawEntries[domain] = new();
        int id = label.GetHashCode();
        sDrawEntries[domain][id] = new(domain, category, () =>
        {
            string value = getter?.Invoke() ?? "";
            ImGui.InputText(label, ref value, maxLength);
            setter?.Invoke(value);
        });
        return id;
    }

    public static int TextInputMultiline(string domain, string category, string label, System.Func<string> getter, Action<string> setter, uint maxLength = 512, ImGuiInputTextFlags flags = ImGuiInputTextFlags.None, float width = 0, float height = 0)
    {
        if (!sDrawEntries.ContainsKey(domain)) sDrawEntries[domain] = new();
        int id = label.GetHashCode();
        sDrawEntries[domain][id] = new(domain, category, () =>
        {
            string value = getter?.Invoke() ?? "";
            ImGui.InputTextMultiline(label, ref value, maxLength, new(width, height), flags);
            setter?.Invoke(value);
        });
        return id;
    }

    public static int IntDrag(string domain, string category, string label, int min, int max, System.Func<int> getter, Action<int> setter)
    {
        if (!sDrawEntries.ContainsKey(domain)) sDrawEntries[domain] = new();
        int id = label.GetHashCode();
        sDrawEntries[domain][id] = new(domain, category, () =>
        {
            int value = getter?.Invoke() ?? 0;
            ImGui.DragInt(label, ref value, min, max);
            setter?.Invoke(value);
        });
        return id;
    }

    public static int FloatDrag(string domain, string category, string label, float min, float max, System.Func<float> getter, Action<float> setter)
    {
        if (!sDrawEntries.ContainsKey(domain)) sDrawEntries[domain] = new();
        int id = label.GetHashCode();
        sDrawEntries[domain][id] = new(domain, category, () =>
        {
            float value = getter?.Invoke() ?? 0;
            ImGui.DragFloat(label, ref value, min, max);
            setter?.Invoke(value);
        });
        return id;
    }

    public static int ColorPicker(string domain, string category, string label, System.Func<Value4> getter, Action<Value4> setter, ImGuiColorEditFlags flags = ImGuiColorEditFlags.None)
    {
        if (!sDrawEntries.ContainsKey(domain)) sDrawEntries[domain] = new();
        int id = label.GetHashCode();
        sDrawEntries[domain][id] = new(domain, category, () =>
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
        if (!sDrawEntries.ContainsKey(domain)) sDrawEntries[domain] = new();
        int id = label.GetHashCode();
        sDrawEntries[domain][id] = new(domain, category, () =>
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
        if (!sDrawEntries.ContainsKey(domain)) sDrawEntries[domain] = new();
        int id = label.GetHashCode();
        sDrawEntries[domain][id] = new(domain, category, () =>
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
        if (!sDrawEntries.ContainsKey(domain)) sDrawEntries[domain] = new();
        int id = label.GetHashCode();
        sDrawEntries[domain][id] = new(domain, category, () =>
        {
            TEnum value = getter.Invoke();
            value = EnumEditor<TEnum>.ListWithFilter(label, filterHolder, value);
            setter?.Invoke(value);
        });
        return id;
    }

    public static int StringList(string domain, string category, string label, string filterHolder, string[] list, System.Func<int> getter, Action<int> indexSetter, Action<string> valueSetter = null)
    {
        if (!sDrawEntries.ContainsKey(domain)) sDrawEntries[domain] = new();
        int id = label.GetHashCode();
        sDrawEntries[domain][id] = new(domain, category, () =>
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
        if (!sDrawEntries.ContainsKey(domain)) sDrawEntries[domain] = new();
        int id = label.GetHashCode();
        sDrawEntries[domain][id] = new(domain, category, () =>
        {
            int index = getter.Invoke();
            TextEditor.ListWithFilter(label, filterHolder, list, ref index, out string value);
            indexSetter?.Invoke(index);
            valueSetter?.Invoke(value);
        });
        return id;
    }

    private static Dictionary<string, Dictionary<int, DrawEntry>> sDrawEntries = new();

    internal static void Draw()
    {
        foreach ((string domain, HashSet<string> categories) in DrawEntry.Categories)
        {
            ImGui.Begin(domain);
            ImGui.BeginTabBar(domain);

            DrawCategories(domain, categories);

            ImGui.EndTabBar();
            ImGui.End();
        }
    }

    private static void DrawCategories(string domain, HashSet<string> categories)
    {
        foreach (string category in categories.Where(category => ImGui.BeginTabItem(category)))
        {
            DrawCategory(domain, category);

            ImGui.EndTabItem();
        }
    }

    private static void DrawCategory(string domain, string category)
    {
        foreach (DrawEntry entry in sDrawEntries[domain].Where(entry => entry.Value.Category == category).Select(entry => entry.Value))
        {
            entry.Draw();
        }
    }

    internal static void Clear()
    {
        sDrawEntries.Clear();
        DrawEntry.Categories.Clear();
    }
}

internal class DrawEntry
{
    public string Domain { get; }
    public string Category { get; }
    public Action Delegate { get; }

    public static readonly Dictionary<string, HashSet<string>> Categories = new();

    public DrawEntry(string domain, string category, Action drawDelegate)
    {
        Domain = domain;
        Category = category;
        Delegate = drawDelegate;

        if (!Categories.ContainsKey(domain)) Categories[domain] = new();
        if (!Categories[domain].Contains(category)) Categories[domain].Add(category);
    }

    public void Draw()
    {
        Delegate?.Invoke();
    }
}
