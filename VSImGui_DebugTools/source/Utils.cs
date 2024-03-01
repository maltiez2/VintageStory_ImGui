using ImGuiNET;
using System.Numerics;
using System.Text.RegularExpressions;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace VSImGui.Debug;

internal static class EnumEditor<TEnum>
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
        EditorsUtils.FilterElements(filter, mNames, out IEnumerable<string> filtered, out IEnumerable<int> indexes, ref index, out int selection);
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
        EditorsUtils.FilterElements(filter, mNames, out IEnumerable<string> filtered, out IEnumerable<int> indexes, ref index, out int selection);
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

internal static class TextEditor
{
    public static void ListWithFilter(string title, string filterHolder, in IEnumerable<string> elements, ref int index, out string value, uint maxFilterLength = 256, string hint = "supports wildcards")
    {
        ImGui.InputTextWithHint($"Filter##{title}", hint, ref filterHolder, maxFilterLength);
        EditorsUtils.FilterElements(filterHolder, elements, out IEnumerable<string> filtered, out IEnumerable<int> indexes, ref index, out int selection);
        ImGui.ListBox(title, ref selection, filtered.ToArray(), filtered.Count());
        index = indexes.ElementAt(selection);
        value = elements.ElementAt(index);
    }
}

internal static class ColorEditor
{
    public static Vector4 HSVAEdit(string title, Vector4 value)
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
    public static Vector4 RGBAEdit(string title, Vector4 value)
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
    public static Vector3 HSVEdit(string title, Vector3 value)
    {
        Vector3 vector = value;
        ImGui.ColorEdit3(title, ref vector, ImGuiColorEditFlags.InputHSV | ImGuiColorEditFlags.DisplayHSV);
        return vector;
    }
    public static Vector3 RGBEdit(string title, Vector3 value)
    {
        Vector3 vector = value;
        ImGui.ColorEdit3(title, ref vector, ImGuiColorEditFlags.InputRGB | ImGuiColorEditFlags.DisplayRGB);
        return vector;
    }
    public static Vector4 HSVAPicker(string title, Vector4 value)
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
    public static Vector4 RGBAPicker(string title, Vector4 value)
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
    public static Vector3 HSVPicker(string title, Vector3 value)
    {
        Vector3 vector = value;
        ImGui.ColorPicker3(title, ref vector, ImGuiColorEditFlags.InputHSV | ImGuiColorEditFlags.DisplayHSV);
        return vector;
    }
    public static Vector3 RGBPicker(string title, Vector3 value)
    {
        Vector3 vector = value;
        ImGui.ColorPicker3(title, ref vector, ImGuiColorEditFlags.InputRGB | ImGuiColorEditFlags.DisplayRGB);
        return vector;
    }
}

internal static class AssetLocationEditor
{
    public static void Edit(string title, ref AssetLocation value, AssetLocation defaultValue)
    {
        try
        {
            if (value?.Valid != true)
            {
                value = defaultValue;
            }
        }
        catch (Exception exception)
        {
            value = defaultValue;
        }


        string domain = value.Domain;
        string path = value.Path.Split('/').Where((_, index) => index > 0).Aggregate((first, second) => $"{first}/{second}");
        AssetCategory category = value.Category;

        ImGui.InputText($"Domain##{title}", ref domain, 256);
        ImGui.InputText($"Path##{title}", ref path, 2048);
        AssetCategoryEditor.Combo($"Category##{title}", ref category);

        string result = $"{category.Code}/{path}";

        value = new AssetLocation(domain, result);
    }
    public static AssetLocation Edit(string title, AssetLocation value, AssetLocation defaultValue)
    {
        Edit(title, ref value, defaultValue);
        return value;
    }
}

internal static class AssetCategoryEditor
{
    public static void Combo(string title, ref AssetCategory value)
    {
        string code = value.Code;
        Dictionary<string, AssetCategory> categories = AssetCategory.categories;
        string[] codes = categories.Select((entry, _) => entry.Key).ToArray();
        int index = codes.IndexOf(code);

        ImGui.Combo(title, ref index, codes, codes.Length);

        value = categories[codes[index]];
    }
    public static AssetCategory Combo(string title, AssetCategory value)
    {
        Combo(title, ref value);
        return value;
    }
}

internal static class ListEditor
{
    public static void Edit(string title, string[] list, ref int selected, Action<string, int> onRemove, System.Func<int, string> onAdd, bool appendToEnd = false)
    {
        if (selected >= list.Length) selected = Math.Max(0, list.Length - 1);

        if (ImGui.Button($"Add##{title}"))
        {
            if (appendToEnd)
            {
                list = list.Append(onAdd.Invoke(selected)).ToArray();
            }
            else
            {
                int selected_not_ref = selected;
                IEnumerable<string> begin = list.Where((_, index) => index <= selected_not_ref).Append(onAdd.Invoke(selected));
                IEnumerable<string> end = list.Where((_, index) => index > selected_not_ref);
                list = begin.Concat(end).ToArray();
            }
        }
        ImGui.SameLine();

        bool disable = list.Length == 0;
        if (disable) ImGui.BeginDisabled();
        if (ImGui.Button($"Remove##{title}"))
        {
            onRemove.Invoke(list[selected], selected);
            list = list.RemoveEntry(selected);
        }
        if (disable) ImGui.EndDisabled();

        if (selected >= list.Length) selected = Math.Max(0, list.Length - 1);

        ImGui.ListBox($"{title}", ref selected, list, list.Length);
    }
}

internal static class TimeEditor
{
    public static readonly string[] sTimeTypes = new string[]
    {
        "microseconds",
        "milliseconds",
        "seconds",
        "minutes",
        "hours",
        "days"
    };

    public static void WithScaleSelection(string title, ref TimeSpan value, ref int timeScale)
    {
        float value_f = Get(title, value, ref timeScale);

        ImGui.DragFloat(title, ref value_f);

        value = Set(value_f, timeScale);
    }
    public static void WithScaleSelection(string title, ref TimeSpan value, ref int timeScale, float min, float max)
    {
        float value_f = Get(title, value, ref timeScale);

        ImGui.SliderFloat(title, ref value_f, min, max);

        value = Set(value_f, timeScale);
    }
    public static void WithScaleSelection(string title, ref TimeSpan value, ref int timeScale, TimeSpan min, TimeSpan max)
    {
        float value_f = Get(title, value, ref timeScale);

        ImGui.SliderFloat(title, ref value_f, Get(min, timeScale), Get(max, timeScale));

        value = Set(value_f, timeScale);
    }

    private static float Get(string title, TimeSpan value, ref int timeScale)
    {
        ImGui.Combo($"##selector{title}", ref timeScale, sTimeTypes, sTimeTypes.Length);
        ImGui.SameLine();
        return Get(value, timeScale);
    }
    private static float Get(TimeSpan value, int timeScale)
    {
        double value_d = sTimeTypes[timeScale] switch
        {
            "microseconds" => (float)value.TotalMicroseconds,
            "milliseconds" => (float)value.TotalMilliseconds,
            "seconds" => (float)value.TotalSeconds,
            "minutes" => (float)value.TotalMinutes,
            "hours" => (float)value.TotalHours,
            "days" => (float)value.TotalDays,
            _ => value.TotalMilliseconds
        };

        return (float)value_d;
    }
    private static TimeSpan Set(float value, int timeScale)
    {
        return sTimeTypes[timeScale] switch
        {
            "microseconds" => TimeSpan.FromMicroseconds(value),
            "milliseconds" => TimeSpan.FromMilliseconds(value),
            "seconds" => TimeSpan.FromSeconds(value),
            "minutes" => TimeSpan.FromMinutes(value),
            "hours" => TimeSpan.FromHours(value),
            "days" => TimeSpan.FromDays(value),
            _ => TimeSpan.FromMilliseconds(value)
        };
    }
}

internal static class EditorsUtils
{
    public static void FilterElements(string filter, in IEnumerable<string> elements, out IEnumerable<string> filtered, out IEnumerable<int> indexes)
    {
        if (filter == "")
        {
            filtered = elements;
            indexes = elements.Select((_, index) => index);
            return;
        }

        string regexFilter = WildCardToRegular(filter);

        var filteredMapping = elements.Select((value, index) => new { value, index }).Where(entry => Match(regexFilter, entry.value));

        filtered = filteredMapping.Select(entry => entry.value).ToArray();
        indexes = filteredMapping.Select(entry => entry.index).ToArray();
    }
    public static bool FilterElements(string filter, in IEnumerable<string> elements, out IEnumerable<string> filtered, out IEnumerable<int> indexes, ref int index, out int selection)
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

        selection = selectedIndex;
        int newIndex = indexes.ElementAt(selectedIndex);

        if (index != newIndex)
        {
            index = newIndex;
            return true;
        }

        return false;
    }

    static public string WildCardToRegular(string value) => "^.*" + Regex.Escape(value).Replace("\\?", ".").Replace("\\*", ".*") + ".*$";
    static public bool Match(string filter, string value) => Regex.IsMatch(value, filter, RegexOptions.IgnoreCase);
    static public bool Match(string filter, params string[] values)
    {
        foreach (string value in values)
        {
            if (Match(filter, value)) return true;
        }
        return false;
    }
}

internal static class Controls
{
    public static void CopySupport(string value, string title)
    {
        if (ImGui.Button(title) || CopyCombination())
        {
            ImGui.SetClipboardText(value);
        }
    }
    public static void PasteSupport(ref string value, string title)
    {
        if (ImGui.Button(title) || PasteCombination())
        {
            value = ImGui.GetClipboardText();
        }
    }
    public static void CopyPasteSupport(ref string value, string copy, string paste)
    {
        CopySupport(value, copy); ImGui.SameLine();
        PasteSupport(ref value, paste); ImGui.SameLine();
    }

    private static bool CopyCombination()
    {
        ImGuiIOPtr io = ImGui.GetIO();
        return io.KeyCtrl && io.KeysDown[(int)ImGuiKey.C];
    }
    private static bool PasteCombination()
    {
        ImGuiIOPtr io = ImGui.GetIO();
        return io.KeyCtrl && io.KeysDown[(int)ImGuiKey.V];
    }
}