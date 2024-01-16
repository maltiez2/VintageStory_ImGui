using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace VSImGui;

public static class EnumEditor<TEnum>
    where TEnum : struct, Enum
{
    private static readonly string[] mNames = Enum.GetNames<TEnum>();
    private static readonly TEnum[] mValues = Enum.GetValues<TEnum>();
    private static readonly Dictionary<TEnum, int> mKeys = mValues.Select((value, index) => new { value, index }).ToDictionary(entry => entry.value, entry => entry.index);

    public static TEnum Combo(string title, TEnum value)
    {
        int index = mKeys[value];
        ImGui.Combo(title, ref index, mNames, mNames.Length);
        return mValues[index];
    }
    public static TEnum Combo(string title, TEnum value, ref bool modified)
    {
        TEnum newValue = Combo(title, value);
        modified = mKeys[newValue] == mKeys[value];
        return newValue;
    }
    public static bool Combo(string title, ref TEnum value)
    {
        bool modified = false;
        value = Combo(title, value, ref modified);
        return modified;
    }
    public static TEnum Combo(string title, string filter, TEnum value)
    {
        int index = mKeys[value];
        int selection = index;
        EditorsUtils.FilterElements(filter, mNames, out IEnumerable<string> filtered, out IEnumerable<int> indexes, ref index, ref selection);
        ImGui.Combo(title, ref selection, filtered.ToArray(), filtered.Count());
        return mValues[indexes.ToArray()[selection]];
    }
    public static TEnum Combo(string title, string filter, TEnum value, ref bool modified)
    {
        TEnum newValue = Combo(title, filter, value);
        modified = mKeys[newValue] == mKeys[value];
        return newValue;
    }
    public static bool Combo(string title, string filter, ref TEnum value)
    {
        bool modified = false;
        value = Combo(title, filter, value, ref modified);
        return modified;
    }

    public static TEnum List(string title, TEnum value)
    {
        int index = mKeys[value];
        ImGui.ListBox(title, ref index, mNames, mNames.Length);
        return mValues[index];
    }
    public static TEnum List(string title, TEnum value, ref bool modified)
    {
        TEnum newValue = List(title, value);
        modified = mKeys[newValue] == mKeys[value];
        return newValue;
    }
    public static bool List(string title, ref TEnum value)
    {
        bool modified = false;
        value = List(title, value, ref modified);
        return modified;
    }
    public static TEnum List(string title, string filter, TEnum value)
    {
        int index = mKeys[value];
        int selection = index;
        EditorsUtils.FilterElements(filter, mNames, out IEnumerable<string> filtered, out IEnumerable<int> indexes, ref index, ref selection);
        ImGui.ListBox(title, ref selection, filtered.ToArray(), filtered.Count());
        return mValues[indexes.ToArray()[selection]];
    }
    public static TEnum List(string title, string filter, TEnum value, ref bool modified)
    {
        TEnum newValue = List(title, filter, value);
        modified = mKeys[newValue] == mKeys[value];
        return newValue;
    }
    public static bool List(string title, string filter, ref TEnum value)
    {
        bool modified = false;
        value = List(title, filter, value, ref modified);
        return modified;
    }

    public static TEnum ComboWithFilter(string title, string filterHolder, TEnum value, uint maxFilterLength = 256, string hint = "supports wildcards")
    {
        ImGui.InputTextWithHint($"Filter##{title}", hint, ref filterHolder, maxFilterLength);
        return Combo(title, filterHolder, value);
    }
    public static TEnum ListWithFilter(string title, string filterHolder, TEnum value, uint maxFilterLength = 256, string hint = "supports wildcards")
    {
        ImGui.InputTextWithHint($"Filter##{title}", hint, ref filterHolder, maxFilterLength);
        return List(title, filterHolder, value);
    }
}

public static class TextEditor
{
    public static void ListWithFilter(string title, string filterHolder, in IEnumerable<string> elements, ref int index, out string value, uint maxFilterLength = 256, string hint = "supports wildcards")
    {
        ImGui.InputTextWithHint($"Filter##{title}", hint, ref filterHolder, maxFilterLength);
        int selection = index;
        EditorsUtils.FilterElements(filterHolder, elements, out IEnumerable<string> filtered, out IEnumerable<int> indexes, ref index, ref selection);
        ImGui.ListBox(title, ref selection, filtered.ToArray(), filtered.Count());
        value = elements.ToArray()[indexes.ToArray()[selection]];
    }
}

public static class ColorEditor
{
    public static Value4 HSVAEdit(string title, Value4 value)
    {
        Vector4 vector = value;
        ImGui.ColorEdit4(title, ref vector, ImGuiColorEditFlags.InputHSV | ImGuiColorEditFlags.DisplayHSV);
        return vector;
    }
    public static uint HSVAEdit(string title, uint value)
    {
        Vector4 vector = ImGui.ColorConvertU32ToFloat4(value);
        ImGui.ColorEdit4(title, ref vector, ImGuiColorEditFlags.InputHSV | ImGuiColorEditFlags.DisplayHSV);
        return ImGui.ColorConvertFloat4ToU32(vector);
    }
    public static Value4 RGBAEdit(string title, Value4 value)
    {
        Vector4 vector = value;
        ImGui.ColorEdit4(title, ref vector, ImGuiColorEditFlags.InputRGB | ImGuiColorEditFlags.DisplayRGB);
        return vector;
    }
    public static uint RGBAEdit(string title, uint value)
    {
        Vector4 vector = ImGui.ColorConvertU32ToFloat4(value);
        ImGui.ColorEdit4(title, ref vector, ImGuiColorEditFlags.InputRGB | ImGuiColorEditFlags.DisplayRGB);
        return ImGui.ColorConvertFloat4ToU32(vector);
    }
    public static Value3 HSVEdit(string title, Value3 value)
    {
        Vector3 vector = value;
        ImGui.ColorEdit3(title, ref vector, ImGuiColorEditFlags.InputHSV | ImGuiColorEditFlags.DisplayHSV);
        return vector;
    }
    public static Value3 RGBEdit(string title, Value3 value)
    {
        Vector3 vector = value;
        ImGui.ColorEdit3(title, ref vector, ImGuiColorEditFlags.InputRGB | ImGuiColorEditFlags.DisplayRGB);
        return vector;
    }
    public static Value4 HSVAPicker(string title, Value4 value)
    {
        Vector4 vector = value;
        ImGui.ColorPicker4(title, ref vector, ImGuiColorEditFlags.InputHSV | ImGuiColorEditFlags.DisplayHSV);
        return vector;
    }
    public static uint HSVAPicker(string title, uint value)
    {
        Vector4 vector = ImGui.ColorConvertU32ToFloat4(value);
        ImGui.ColorPicker4(title, ref vector, ImGuiColorEditFlags.InputHSV | ImGuiColorEditFlags.DisplayHSV);
        return ImGui.ColorConvertFloat4ToU32(vector);
    }
    public static Value4 RGBAPicker(string title, Value4 value)
    {
        Vector4 vector = value;
        ImGui.ColorPicker4(title, ref vector, ImGuiColorEditFlags.InputRGB | ImGuiColorEditFlags.DisplayRGB);
        return vector;
    }
    public static uint RGBAPicker(string title, uint value)
    {
        Vector4 vector = ImGui.ColorConvertU32ToFloat4(value);
        ImGui.ColorPicker4(title, ref vector, ImGuiColorEditFlags.InputRGB | ImGuiColorEditFlags.DisplayRGB);
        return ImGui.ColorConvertFloat4ToU32(vector);
    }
    public static Value3 HSVPicker(string title, Value3 value)
    {
        Vector3 vector = value;
        ImGui.ColorPicker3(title, ref vector, ImGuiColorEditFlags.InputHSV | ImGuiColorEditFlags.DisplayHSV);
        return vector;
    }
    public static Value3 RGBPicker(string title, Value3 value)
    {
        Vector3 vector = value;
        ImGui.ColorPicker3(title, ref vector, ImGuiColorEditFlags.InputRGB | ImGuiColorEditFlags.DisplayRGB);
        return vector;
    }
}

public static class EditorsUtils
{
    public static void FilterElements(string filter, in IEnumerable<string> elements, out IEnumerable<string> filtered, out IEnumerable<int> indexes)
    {
        if (filter == "")
        {
            filtered = elements;
            indexes = elements.Select((_, index) => index);
            return;
        }

        string regexFilter = StyleEditor.WildCardToRegular(filter);

        var filteredMapping = elements.Select((value, index) => new { value, index }).Where(entry => StyleEditor.Match(regexFilter, entry.value));

        filtered = filteredMapping.Select(entry => entry.value).ToArray();
        indexes = filteredMapping.Select(entry => entry.index).ToArray();
    }
    public static bool FilterElements(string filter, in IEnumerable<string> elements, out IEnumerable<string> filtered, out IEnumerable<int> indexes, ref int index, ref int selection)
    {
        FilterElements(filter, in elements, out filtered, out indexes);

        int selectedIndex = index;
        selectedIndex = indexes.Where(value => value == selectedIndex).FirstOrDefault(-1);

        if (selectedIndex == -1)
        {
            index = 0;
            selection = 0;
            return true;
        }

        if (selection != selectedIndex)
        {
            selection = selectedIndex;
            return true;
        }

        return false;
    }
}