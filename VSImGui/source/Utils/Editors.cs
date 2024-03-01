using ImGuiNET;
using System.Numerics;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace VSImGui;

/// <summary>
/// Collection of wrappers around ImGui widgets for editing regular enum variables.
/// </summary>
/// <typeparam name="TEnum"></typeparam>
public static partial class EnumEditor<TEnum>
    where TEnum : struct, Enum
{
    /// <summary>
    /// Combo-box with all possible values of <see cref="TEnum"/>.
    /// </summary>
    /// <param name="title">Widget's title and id</param>
    /// <param name="value">Current value</param>
    /// <returns>New value</returns>
    public static TEnum Combo(string title, TEnum value)
    {
        int index = _keys[value];
        ImGui.Combo(title, ref index, _names, _names.Length);
        return _values[index];
    }
    /// <summary>
    /// Combo-box with all possible values of <see cref="TEnum"/>.
    /// </summary>
    /// <param name="title">Widget's title and id</param>
    /// <param name="value">Current value</param>
    /// <param name="modified">Sets to <see cref="true"/> if value has been modified</param>
    /// <returns>New value</returns>
    public static TEnum Combo(string title, TEnum value, ref bool modified)
    {
        TEnum newValue = Combo(title, value);
        if (_keys[newValue] != _keys[value]) modified = true;
        return newValue;
    }
    /// <summary>
    /// Combo-box with all possible values of <see cref="TEnum"/>.
    /// </summary>
    /// <param name="title">Widget's title and id</param>
    /// <param name="value">Reference to variable that will be edited</param>
    /// <returns>Whether value has been modified</returns>
    public static bool Combo(string title, ref TEnum value)
    {
        bool modified = false;
        value = Combo(title, value, ref modified);
        return modified;
    }
    /// <summary>
    /// Combo-box with values of <see cref="TEnum"/> filtered by <paramref name="filter"/> using wildcards.
    /// </summary>
    /// <param name="title">Widget's title and id</param>
    /// <param name="filter">Filters possible values using wildcards</param>
    /// <param name="value">Current value</param>
    /// <returns>New value</returns>
    public static TEnum Combo(string title, string filter, TEnum value)
    {
        int index = _keys[value];
        EditorsUtils.FilterElements(filter, _names, out IEnumerable<string> filtered, out IEnumerable<int> indexes, ref index, out int selection);
        ImGui.Combo(title, ref selection, filtered.ToArray(), filtered.Count());
        return _values[indexes.ToArray()[selection]];
    }
    /// <summary>
    /// Combo-box with values of <see cref="TEnum"/> filtered by <paramref name="filter"/> using wildcards.
    /// </summary>
    /// <param name="title">Widget's title and id</param>
    /// <param name="filter">Filters possible values using wildcards</param>
    /// <param name="value">Current value</param>
    /// <param name="modified">Sets to <see cref="true"/> if value has been modified</param>
    /// <returns>New value</returns>
    public static TEnum Combo(string title, string filter, TEnum value, ref bool modified)
    {
        TEnum newValue = Combo(title, filter, value);
        if (_keys[newValue] != _keys[value]) modified = true;
        return newValue;
    }
    /// <summary>
    /// Combo-box with values of <see cref="TEnum"/> filtered by <paramref name="filter"/> using wildcards.
    /// </summary>
    /// <param name="title">Widget's title and id</param>
    /// <param name="filter">Filters possible values using wildcards</param>
    /// <param name="value"Reference to variable that will be edited></param>
    /// <returns>Whether value has been modified</returns>
    public static bool Combo(string title, string filter, ref TEnum value)
    {
        bool modified = false;
        value = Combo(title, filter, value, ref modified);
        return modified;
    }

    /// <summary>
    /// List-box with all possible values of <see cref="TEnum"/>.
    /// </summary>
    /// <param name="title">Widget's title and id</param>
    /// <param name="value">Current value</param>
    /// <returns>New value</returns>
    public static TEnum List(string title, TEnum value)
    {
        int index = _keys[value];
        ImGui.ListBox(title, ref index, _names, _names.Length);
        return _values[index];
    }
    /// <summary>
    /// List-box with all possible values of <see cref="TEnum"/>.
    /// </summary>
    /// <param name="title">Widget's title and id</param>
    /// <param name="value">Current value</param>
    /// <param name="modified">Sets to <see cref="true"/> if value has been modified</param>
    /// <returns>New value</returns>
    public static TEnum List(string title, TEnum value, ref bool modified)
    {
        TEnum newValue = List(title, value);
        if (_keys[newValue] != _keys[value]) modified = true;
        return newValue;
    }
    /// <summary>
    /// List-box with all possible values of <see cref="TEnum"/>.
    /// </summary>
    /// <param name="title">Widget's title and id</param>
    /// <param name="value">Reference to variable that will be edited</param>
    /// <returns>Whether value has been modified</returns>
    public static bool List(string title, ref TEnum value)
    {
        bool modified = false;
        value = List(title, value, ref modified);
        return modified;
    }
    /// <summary>
    /// List-box with values of <see cref="TEnum"/> filtered by <paramref name="filter"/> using wildcards.
    /// </summary>
    /// <param name="title">Widget's title and id</param>
    /// <param name="filter">Filters possible values using wildcards</param>
    /// <param name="value">Current value</param>
    /// <returns>New value</returns>
    public static TEnum List(string title, string filter, TEnum value)
    {
        int index = _keys[value];
        EditorsUtils.FilterElements(filter, _names, out IEnumerable<string> filtered, out IEnumerable<int> indexes, ref index, out int selection);
        ImGui.ListBox(title, ref selection, filtered.ToArray(), filtered.Count());
        return _values[indexes.ToArray()[selection]];
    }
    /// <summary>
    /// List-box with values of <see cref="TEnum"/> filtered by <paramref name="filter"/> using wildcards.
    /// </summary>
    /// <param name="title">Widget's title and id</param>
    /// <param name="filter">Filters possible values using wildcards</param>
    /// <param name="value">Current value</param>
    /// <param name="modified">Sets to <see cref="true"/> if value has been modified</param>
    /// <returns>New value</returns>
    public static TEnum List(string title, string filter, TEnum value, ref bool modified)
    {
        TEnum newValue = List(title, filter, value);
        if (_keys[newValue] != _keys[value]) modified = true;
        return newValue;
    }
    /// <summary>
    /// List-box with values of <see cref="TEnum"/> filtered by <paramref name="filter"/> using wildcards.
    /// </summary>
    /// <param name="title">Widget's title and id</param>
    /// <param name="filter">Filters possible values using wildcards</param>
    /// <param name="value"Reference to variable that will be edited></param>
    /// <returns>Whether value has been modified</returns>
    public static bool List(string title, string filter, ref TEnum value)
    {
        bool modified = false;
        value = List(title, filter, value, ref modified);
        return modified;
    }

    /// <summary>
    /// Combo-box with values of <see cref="TEnum"/> filtered by <paramref name="filterHolder"/> using wildcards.<br/>
    /// Also have text-box for entering filter value
    /// </summary>
    /// <param name="title">Widget's title and id</param>
    /// <param name="filterHolder">String that will hold filter value.<br/>Needed to make filter value persistent between frames.</param>
    /// <param name="value">Current value</param>
    /// <param name="maxFilterLength">Max filter length in characters</param>
    /// <param name="hint">Hint that would be displayed in filter widget</param>
    /// <returns>New value</returns>
    public static TEnum ComboWithFilter(string title, string filterHolder, TEnum value, uint maxFilterLength = 256, string hint = "supports wildcards")
    {
        ImGui.InputTextWithHint($"Filter##{title}", hint, ref filterHolder, maxFilterLength);
        return Combo(title, filterHolder, value);
    }
    /// <summary>
    /// List-box with values of <see cref="TEnum"/> filtered by <paramref name="filterHolder"/> using wildcards.<br/>
    /// Also have text-box for entering filter value
    /// </summary>
    /// <param name="title">Widget's title and id</param>
    /// <param name="filterHolder">String that will hold filter value.<br/>Needed to make filter value persistent between frames.</param>
    /// <param name="value">Current value</param>
    /// <param name="maxFilterLength">Max filter length in characters</param>
    /// <param name="hint">Hint that would be displayed in filter widget</param>
    /// <returns>New value</returns>
    public static TEnum ListWithFilter(string title, string filterHolder, TEnum value, uint maxFilterLength = 256, string hint = "supports wildcards")
    {
        ImGui.InputTextWithHint($"Filter##{title}", hint, ref filterHolder, maxFilterLength);
        return List(title, filterHolder, value);
    }

    /// <summary>
    /// Enum values names to display in combo-boxes and list-boxes
    /// </summary>
    private static readonly string[] _names = Enum.GetNames<TEnum>();
    /// <summary>
    /// Available enum values
    /// </summary>
    private static readonly TEnum[] _values = Enum.GetValues<TEnum>();
    /// <summary>
    /// Maps enum values to their corresponding indexes in <see cref="_names"/> and <see cref="_values"/>
    /// </summary>
    private static readonly Dictionary<TEnum, int> _keys = _values.Select((value, index) => new { value, index }).ToDictionary(entry => entry.value, entry => entry.index);
}

/// <summary>
/// Collection of wrappers around ImGui widgets for editing string values.
/// </summary>
public static partial class TextEditor
{
    /// <summary>
    /// Filtered list of strings with its own widget for editing filter value
    /// </summary>
    /// <param name="title">Widget's title and id</param>
    /// <param name="filterHolder">String that will hold filter value.<br/>Needed to make filter value persistent between frames.</param>
    /// <param name="elements">Collection of string values</param>
    /// <param name="index">Index of selected item in <paramref name="elements"/> using 'ElementAt' method</param>
    /// <param name="value">Value of selected value</param>
    /// <param name="maxFilterLength">Max filter length in characters</param>
    /// <param name="hint">Hint that would be displayed in filter widget</param>
    public static void ListWithFilter(string title, string filterHolder, in IEnumerable<string> elements, ref int index, out string value, uint maxFilterLength = 256, string hint = "supports wildcards")
    {
        ImGui.InputTextWithHint($"Filter##{title}", hint, ref filterHolder, maxFilterLength);
        EditorsUtils.FilterElements(filterHolder, elements, out IEnumerable<string> filtered, out IEnumerable<int> indexes, ref index, out int selection);
        ImGui.ListBox(title, ref selection, filtered.ToArray(), filtered.Count());
        index = indexes.ElementAt(selection);
        value = elements.ElementAt(index);
    }
}

/// <summary>
/// Collection of wrappers around ImGui widgets for editing color values.<br/>
/// It uses <see cref="Value4"/> amd <see cref="Value3"/> types, that are convinience structs that support implicit conversion from different vector types.
/// </summary>
public static partial class ColorEditor
{
    /// <summary>
    /// Small color editor that can open color picker and displays selected color.
    /// </summary>
    /// <param name="title">Widget's title and id</param>
    /// <param name="value">Current value.<br/>Format depends of <paramref name="flags"/>.</param>
    /// <param name="flags"><see cref="ImGui.ColorEdit4"/> flags.</param>
    /// <returns>New value</returns>
    public static Value4 ColorEdit(string title, Value4 value, ImGuiColorEditFlags flags)
    {
        Vector4 vector = value;
        ImGui.ColorEdit4(title, ref vector, flags);
        return vector;
    }
    /// <summary>
    /// Small color editor that can open color picker and displays selected color.
    /// </summary>
    /// <param name="title">Widget's title and id.</param>
    /// <param name="value">Current value in HSVA format.</param>
    /// <returns>New value in HSVA format</returns>
    public static Value4 HSVAEdit(string title, Value4 value)
    {
        Vector4 vector = value;
        ImGui.ColorEdit4(title, ref vector, ImGuiColorEditFlags.InputHSV | ImGuiColorEditFlags.DisplayHSV);
        return vector;
    }
    /// <summary>
    /// Small color editor that can open color picker and displays selected color.
    /// </summary>
    /// <param name="title">Widget's title and id.</param>
    /// <param name="value">Current value in U32 RGBA format.</param>
    /// <returns>New value in U32 RGBA format</returns>
    public static uint HSVAEdit(string title, uint value)
    {
        Vector4 vector = ImGui.ColorConvertU32ToFloat4(value);
        ImGui.ColorEdit4(title, ref vector, ImGuiColorEditFlags.InputRGB | ImGuiColorEditFlags.DisplayHSV);
        return ImGui.ColorConvertFloat4ToU32(vector);
    }
    /// <summary>
    /// Small color editor that can open color picker and displays selected color.
    /// </summary>
    /// <param name="title">Widget's title and id.</param>
    /// <param name="value">Current value in RGBA format.</param>
    /// <returns>New value in RGBA format</returns>
    public static Value4 RGBAEdit(string title, Value4 value)
    {
        Vector4 vector = value;
        ImGui.ColorEdit4(title, ref vector, ImGuiColorEditFlags.InputRGB | ImGuiColorEditFlags.DisplayRGB);
        return vector;
    }
    /// <summary>
    /// Small color editor that can open color picker and displays selected color.
    /// </summary>
    /// <param name="title">Widget's title and id.</param>
    /// <param name="value">Current value in U32 RGBA format.</param>
    /// <returns>New value in U32 RGBA format</returns>
    public static uint RGBAEdit(string title, uint value)
    {
        Vector4 vector = ImGui.ColorConvertU32ToFloat4(value);
        ImGui.ColorEdit4(title, ref vector, ImGuiColorEditFlags.InputRGB | ImGuiColorEditFlags.DisplayRGB);
        return ImGui.ColorConvertFloat4ToU32(vector);
    }
    /// <summary>
    /// Small color editor that can open color picker and displays selected color.
    /// </summary>
    /// <param name="title">Widget's title and id.</param>
    /// <param name="value">Current value in HSV format.</param>
    /// <returns>New value in HSV format</returns>
    public static Value3 HSVEdit(string title, Value3 value)
    {
        Vector3 vector = value;
        ImGui.ColorEdit3(title, ref vector, ImGuiColorEditFlags.InputHSV | ImGuiColorEditFlags.DisplayHSV);
        return vector;
    }
    /// <summary>
    /// Small color editor that can open color picker and displays selected color.
    /// </summary>
    /// <param name="title">Widget's title and id.</param>
    /// <param name="value">Current value in RGB format.</param>
    /// <returns>New value in RGB format</returns>
    public static Value3 RGBEdit(string title, Value3 value)
    {
        Vector3 vector = value;
        ImGui.ColorEdit3(title, ref vector, ImGuiColorEditFlags.InputRGB | ImGuiColorEditFlags.DisplayRGB);
        return vector;
    }

    /// <summary>
    /// Relatively big (can be resized by <see cref="ImGui.PushItemWidth(float)"/>) color picker.
    /// </summary>
    /// <param name="title">Widget's title and id</param>
    /// <param name="value">Current value.<br/>Format depends of <paramref name="flags"/>.</param>
    /// <param name="flags"><see cref="ImGui.ColorEdit4"/> flags.</param>
    /// <returns>New value</returns>
    public static Value4 ColorPicker(string title, Value4 value, ImGuiColorEditFlags flags)
    {
        Vector4 vector = value;
        ImGui.ColorPicker4(title, ref vector, flags);
        return vector;
    }
    /// <summary>
    /// Relatively big (can be resized by <see cref="ImGui.PushItemWidth(float)"/>) color picker.
    /// </summary>
    /// <param name="title">Widget's title and id.</param>
    /// <param name="value">Current value in HSVA format.</param>
    /// <returns>New value in HSVA format</returns>
    public static Value4 HSVAPicker(string title, Value4 value)
    {
        Vector4 vector = value;
        ImGui.ColorPicker4(title, ref vector, ImGuiColorEditFlags.InputHSV | ImGuiColorEditFlags.DisplayHSV);
        return vector;
    }
    /// <summary>
    /// Relatively big (can be resized by <see cref="ImGui.PushItemWidth(float)"/>) color picker.
    /// </summary>
    /// <param name="title">Widget's title and id.</param>
    /// <param name="value">Current value in U32 RGBA format.</param>
    /// <returns>New value in U32 RGBA format</returns>
    public static uint HSVAPicker(string title, uint value)
    {
        Vector4 vector = ImGui.ColorConvertU32ToFloat4(value);
        ImGui.ColorPicker4(title, ref vector, ImGuiColorEditFlags.InputRGB | ImGuiColorEditFlags.DisplayHSV);
        return ImGui.ColorConvertFloat4ToU32(vector);
    }
    /// <summary>
    /// Relatively big (can be resized by <see cref="ImGui.PushItemWidth(float)"/>) color picker.
    /// </summary>
    /// <param name="title">Widget's title and id.</param>
    /// <param name="value">Current value in RGBA format.</param>
    /// <returns>New value in RGBA format</returns>
    public static Value4 RGBAPicker(string title, Value4 value)
    {
        Vector4 vector = value;
        ImGui.ColorPicker4(title, ref vector, ImGuiColorEditFlags.InputRGB | ImGuiColorEditFlags.DisplayRGB);
        return vector;
    }
    /// <summary>
    /// Relatively big (can be resized by <see cref="ImGui.PushItemWidth(float)"/>) color picker.
    /// </summary>
    /// <param name="title">Widget's title and id.</param>
    /// <param name="value">Current value in U32 RGBA format.</param>
    /// <returns>New value in U32 RGBA format</returns>
    public static uint RGBAPicker(string title, uint value)
    {
        Vector4 vector = ImGui.ColorConvertU32ToFloat4(value);
        ImGui.ColorPicker4(title, ref vector, ImGuiColorEditFlags.InputRGB | ImGuiColorEditFlags.DisplayRGB);
        return ImGui.ColorConvertFloat4ToU32(vector);
    }
    /// <summary>
    /// Relatively big (can be resized by <see cref="ImGui.PushItemWidth(float)"/>) color picker.
    /// </summary>
    /// <param name="title">Widget's title and id.</param>
    /// <param name="value">Current value in HSV format.</param>
    /// <returns>New value in HSV format</returns>
    public static Value3 HSVPicker(string title, Value3 value)
    {
        Vector3 vector = value;
        ImGui.ColorPicker3(title, ref vector, ImGuiColorEditFlags.InputHSV | ImGuiColorEditFlags.DisplayHSV);
        return vector;
    }
    /// <summary>
    /// Relatively big (can be resized by <see cref="ImGui.PushItemWidth(float)"/>) color picker.
    /// </summary>
    /// <param name="title">Widget's title and id.</param>
    /// <param name="value">Current value in RGB format.</param>
    /// <returns>New value in RGB format</returns>
    public static Value3 RGBPicker(string title, Value3 value)
    {
        Vector3 vector = value;
        ImGui.ColorPicker3(title, ref vector, ImGuiColorEditFlags.InputRGB | ImGuiColorEditFlags.DisplayRGB);
        return vector;
    }
}

/// <summary>
/// Collection of editors for <see cref="AssetLocation"/>
/// </summary>
public static partial class AssetLocationEditor
{
    /// <summary>
    /// Allows to edit domain, path and category of asset location. Replaces with default value if <paramref name="value"/> is not Valid.
    /// </summary>
    /// <param name="title">Used as widgets' id</param>
    /// <param name="value"></param>
    /// <param name="defaultValue">Is used to substitute given value in case of it being not valid. Is cloned on substitution.</param>
    public static void Edit(string title, ref AssetLocation value, AssetLocation? defaultValue)
    {
        try
        {
            if (!value.Valid && defaultValue != null)
            {
                value = defaultValue.Clone();
            }
        }
        catch
        {
            if (defaultValue == null)
            {
                ImGui.Text($"{title}: invalid");
                return;
            }    
            value = defaultValue.Clone();
        }

        string domain = value.Domain;
        string path = value.Path.Split('/').Where((_, index) => index > 0).Aggregate((first, second) => $"{first}/{second}");
        AssetCategory category = value.Category;

        ImGui.InputText($"Domain##{title}", ref domain, 256);
        ImGui.InputText($"Path##{title}", ref path, 2048);
        AssetCategoryEditor.Combo($"Category##{title}", ref category);

        value.Domain = domain;
        value.Path = $"{category.Code}/{path}";
    }
    /// <summary>
    /// Allows to edit domain, path and category of asset location
    /// </summary>
    /// <param name="title">Used as widgets' id</param>
    /// <param name="value"></param>
    public static void Edit(string title, AssetLocation value) => Edit(title, ref value, null);
}

/// <summary>
/// Collection of editors for <see cref="AssetCategoryEditor"/>
/// </summary>
public static partial class AssetCategoryEditor
{
    /// <summary>
    /// Combo-box with list of available <see cref="AssetCategory.categories"/>.
    /// </summary>
    /// <param name="title">Widget's title and id</param>
    /// <param name="value">Replaces value with selected one from <see cref="AssetCategory.categories"/></param>
    public static void Combo(string title, ref AssetCategory value)
    {
        string code = value.Code;
        Dictionary<string, AssetCategory> categories = AssetCategory.categories;
        string[] codes = categories.Select((entry, _) => entry.Key).ToArray();
        int index = codes.IndexOf(code);

        ImGui.Combo(title, ref index, codes, codes.Length);

        value = categories[codes[index]];
    }
    /// <summary>
    /// Combo-box with list of available <see cref="AssetCategory.categories"/>.
    /// </summary>
    /// <param name="title">Widget's title and id</param>
    /// <param name="value">A category from <see cref="AssetCategory.categories"/></param>
    /// <returns>New selected value</returns>
    public static AssetCategory Combo(string title, AssetCategory value)
    {
        Combo(title, ref value);
        return value;
    }
}

/// <summary>
/// Collection of editors for lists
/// </summary>
public static partial class ListEditor
{
    /// <summary>
    /// List-box for editing a list of values with 'Add' and 'Remove' buttons. Manages adding and removing items.
    /// </summary>
    /// <param name="title">Widget title (and id for buttons)</param>
    /// <param name="list">Displayed list</param>
    /// <param name="selected">Index of selected value in <paramref name="list"/></param>
    /// <param name="onRemove">Callback on remove button press. Selected index and value are passed as arguments.</param>
    /// <param name="onAdd">Callback on add button press. Selected index is passed as argument. New value is expected as return value.</param>
    /// <param name="appendToEnd">If <see cref="false"/> value appended to selected position, else to the end of list</param>
    public static void Edit(string title, ref string[] list, ref int selected, System.Func<int, string>? onAdd = null, Action<string, int>? onRemove = null, bool appendToEnd = false)
    {
        if (selected >= list.Length) selected = Math.Max(0, list.Length - 1);

        if (ImGui.Button($"Add##{title}"))
        {
            if (appendToEnd)
            {
                list = list.Append(onAdd?.Invoke(selected) ?? list[selected]).ToArray();
            }
            else
            {
                int selected_not_ref = selected;
                IEnumerable<string> begin = list.Where((_, index) => index <= selected_not_ref).Append(onAdd?.Invoke(selected) ?? list[selected]);
                IEnumerable<string> end = list.Where((_, index) => index > selected_not_ref);
                list = begin.Concat(end).ToArray();
            }
        }
        ImGui.SameLine();

        bool disable = list.Length == 0;
        if (disable) ImGui.BeginDisabled();
        if (ImGui.Button($"Remove##{title}"))
        {
            onRemove?.Invoke(list[selected], selected);
            list = list.RemoveEntry(selected);
        }
        if (disable) ImGui.EndDisabled();

        if (selected >= list.Length) selected = Math.Max(0, list.Length - 1);

        ImGui.ListBox($"{title}", ref selected, list, list.Length);
    }
    /// <summary>
    /// List-box for editing a list of values with 'Add' and 'Remove' buttons.
    /// </summary>
    /// <param name="title">Widget title (and id for buttons)</param>
    /// <param name="list">Displayed list</param>
    /// <param name="selected">Index of selected value in <paramref name="list"/></param>
    /// <param name="onRemove">Callback on remove button press. Should handle removing element from list. Selected index and value are passed as arguments.</param>
    /// <param name="onAdd">Callback on add button press. Should handle adding element from list. Selected index is passed as argument. New value is expected as return value.</param>
    public static void Edit(string title, string[] list, ref int selected, Action<string, int> onRemove, System.Func<int, string> onAdd)
    {
        if (selected >= list.Length) selected = Math.Max(0, list.Length - 1);

        if (ImGui.Button($"Add##{title}"))
        {
            list = list.Append(onAdd.Invoke(selected)).ToArray();
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
    [Obsolete("appendToEnd argument do nothing, use 'public static void Edit(string title, string[] list, ref int selected, Action<string, int> onRemove, System.Func<int, string> onAdd)' instead")]
    public static void Edit(string title, string[] list, ref int selected, Action<string, int> onRemove, System.Func<int, string> onAdd, bool appendToEnd = false) => Edit(title, list, ref selected, onRemove, onAdd);
}

/// <summary>
/// Collection of editors for <see cref="TimeSpan"/>
/// </summary>
public static partial class TimeEditor
{
    /// <summary>
    /// Available time scales for float based editors
    /// </summary>
    public static readonly string[] TimeTypes = new string[]
    {
        "microseconds",
        "milliseconds",
        "seconds",
        "minutes",
        "hours",
        "days"
    };

    /// <summary>
    /// Draggable float editor with selection of timescale
    /// </summary>
    /// <param name="title">Title and id of widget</param>
    /// <param name="value"></param>
    /// <param name="timeScale">Index from <see cref="TimeTypes"/>. Is used to hold selected scale value between frames.</param>
    public static void WithScaleSelection(string title, ref TimeSpan value, ref int timeScale)
    {
        float value_f = Get(title, value, ref timeScale);

        ImGui.DragFloat(title, ref value_f);

        value = Set(value_f, timeScale);
    }
    /// <summary>
    /// Draggable float editor with selection of timescale
    /// </summary>
    /// <param name="title">Title and id of widget</param>
    /// <param name="value">Current value</param>
    /// <param name="timeScale">Index from <see cref="TimeTypes"/>. Is used to hold selected scale value between frames.</param>
    /// <returns>New value</returns>
    public static TimeSpan WithScaleSelection(string title, TimeSpan value, ref int timeScale)
    {
        WithScaleSelection(title, ref value, ref timeScale);
        return value;
    }
    /// <summary>
    /// Slider float editor with selection of timescale
    /// </summary>
    /// <param name="title">Title and id of widget</param>
    /// <param name="value"></param>
    /// <param name="timeScale">Index from <see cref="TimeTypes"/>. Is used to hold selected scale value between frames</param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public static void WithScaleSelection(string title, ref TimeSpan value, ref int timeScale, TimeSpan min, TimeSpan max)
    {
        float value_f = Get(title, value, ref timeScale);

        ImGui.SliderFloat(title, ref value_f, Get(min, timeScale), Get(max, timeScale));

        value = Set(value_f, timeScale);
    }
    /// <summary>
    /// Slider float editor with selection of timescale
    /// </summary>
    /// <param name="title">Title and id of widget</param>
    /// <param name="value">Current value</param>
    /// <param name="timeScale">Index from <see cref="TimeTypes"/>. Is used to hold selected scale value between frames</param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns>New value</returns>
    public static TimeSpan WithScaleSelection(string title, TimeSpan value, ref int timeScale, TimeSpan min, TimeSpan max)
    {
        WithScaleSelection(title, ref value, ref timeScale, min, max);
        return value;
    }

    /// <summary>
    /// Selector for time scale, returns scaled value
    /// </summary>
    /// <param name="title"></param>
    /// <param name="value"></param>
    /// <param name="timeScale"></param>
    /// <returns></returns>
    private static float Get(string title, TimeSpan value, ref int timeScale)
    {
        ImGui.Combo($"##selector{title}", ref timeScale, TimeTypes, TimeTypes.Length);
        ImGui.SameLine();
        return Get(value, timeScale);
    }
    /// <summary>
    /// Scales value
    /// </summary>
    /// <param name="value"></param>
    /// <param name="timeScale"></param>
    /// <returns></returns>
    private static float Get(TimeSpan value, int timeScale)
    {
        double value_d = TimeTypes[timeScale] switch
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
    /// <summary>
    /// Unscales value
    /// </summary>
    /// <param name="value"></param>
    /// <param name="timeScale"></param>
    /// <returns></returns>
    private static TimeSpan Set(float value, int timeScale)
    {
        return TimeTypes[timeScale] switch
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

/// <summary>
/// General utls for editors
/// </summary>
public static partial class EditorsUtils
{
    /// <summary>
    /// Filters elements of a collection.
    /// </summary>
    /// <param name="filter">Filter with wildcards support</param>
    /// <param name="elements">Collection to filter</param>
    /// <param name="filtered">Result of applying given filter</param>
    /// <param name="indexes">Map from old to new elemets indexes</param>
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
    /// <summary>
    /// Filters elements of a collection and manages selected items
    /// </summary>
    /// <param name="filter">Filter with wildcards support</param>
    /// <param name="elements">Collection to filter</param>
    /// <param name="filtered">Result of applying given filter</param>
    /// <param name="indexes">Map from old to new elements indexes</param>
    /// <param name="index">Selected index in given collection</param>
    /// <param name="selection">Selected index in filtered collection</param>
    /// <returns>True if selection has changed</returns>
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
}