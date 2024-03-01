using ImGuiNET;
using System.Numerics;
using Vintagestory.API.MathTools;

namespace VSImGui.Debug;

/// <summary>
/// Collection of methods for building small, fast to make, one-line debug tools that can be called from anywhere in code any number of times.<br/><br/>
/// Tools are drawn in ImGui windows with specified domain used as id and title, in tabs with specified category used as title and id.<br/><br/>
/// Widgets that require integer id use it as widget id to apply changes to existing widget instead of registering new one on subsequent calls.<br/>
/// Other widgets use label as their widget id and its hash as integer id that <see cref="DebugWidgets.Remove(string, int)"/> method accepts.
/// </summary>
public static partial class DebugWidgets
{
    /// <summary>
    /// Draws arbitrary widgets using supplied action
    /// </summary>
    /// <param name="domain">Title and id of window that will be used to draw widgets in</param>
    /// <param name="category">Name and id of a tab inside ImGui window to draw widgets in</param>
    /// <param name="id">Unique id inside a domain. Is used as way to identify same widget on subsequent calls.</param>
    /// <param name="drawDelegate">Contains widgets to draw inside debug window.</param>
    public static void Draw(string domain, string category, int id, Action drawDelegate)
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, drawDelegate);
    }

    /// <summary>
    /// Removes widget with given id from draw list.
    /// </summary>
    /// <param name="domain">Domain given to widget on call of other <see cref="DebugWidgets"/> methods.</param>
    /// <param name="id">Id given to widget explicitly, or hash of its label otherwise. Id is also returned by methods used to add widget.</param>
    public static void Remove(string domain, int id) => DebugWindowsManager.DrawEntries[domain].Remove(id);

    /// <summary>
    /// Button that can be clicked.
    /// </summary>
    /// <param name="domain">Title and id of window that will be used to draw widget</param>
    /// <param name="category">Name and id of a tab inside ImGui window to draw widget in</param>
    /// <param name="label">Unique inside domain. Label and id of the widget. Used to identify same widgets to replace them instead of creating new one.</param>
    /// <param name="setter">Is used to set value. In this case called each frame, and has its argument equal to true in frame when button was pressed.</param>
    /// <returns>Widget id that can be used to <see cref="Remove(string, int)"/> it</returns>
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
    /// <summary>
    /// Button that can be clicked.
    /// </summary>
    /// <param name="domain">Title and id of window that will be used to draw widget</param>
    /// <param name="category">Name and id of a tab inside ImGui window to draw widget in</param>
    /// <param name="label">Unique inside domain. Label and id of the widget. Used to identify same widgets to replace them instead of creating new one.</param>
    /// <param name="callback">Invoked when button is pressed.</param>
    /// <returns>Widget id that can be used to <see cref="Remove(string, int)"/> it</returns>
    public static int Button(string domain, string category, string label, Action callback)
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        int id = label.GetHashCode();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () =>
        {
            if (ImGui.Button(label)) callback?.Invoke();
        });
        return id;
    }
    /// <summary>
    /// Check-box that can be set on/off (like a button that can be toggled on/off).
    /// </summary>
    /// <param name="domain">Title and id of window that will be used to draw widget</param>
    /// <param name="category">Name and id of a tab inside ImGui window to draw widget in</param>
    /// <param name="label">Unique inside domain. Label and id of the widget. Used to identify same widgets to replace them instead of creating new one.</param>
    /// <param name="getter">Used by widget to get current value.</param>
    /// <param name="setter">Invoked by widget with argument equal to new value.</param>
    /// <returns>Widget id that can be used to <see cref="Remove(string, int)"/> it</returns>
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

    /// <summary>
    /// Slider for integer value
    /// </summary>
    /// <param name="domain">Title and id of window that will be used to draw widget</param>
    /// <param name="category">Name and id of a tab inside ImGui window to draw widget in</param>
    /// <param name="label">Unique inside domain. Label and id of the widget. Used to identify same widgets to replace them instead of creating new one.</param>
    /// <param name="min">Minimum value</param>
    /// <param name="max">Maximum value</param>
    /// <param name="getter">Used by widget to get current value.</param>
    /// <param name="setter">Invoked by widget with argument equal to new value.</param>
    /// <returns>Widget id that can be used to <see cref="Remove(string, int)"/> it</returns>
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
    /// <summary>
    /// Slider for float value
    /// </summary>
    /// <param name="domain">Title and id of window that will be used to draw widget</param>
    /// <param name="category">Name and id of a tab inside ImGui window to draw widget in</param>
    /// <param name="label">Unique inside domain. Label and id of the widget. Used to identify same widgets to replace them instead of creating new one.</param>
    /// <param name="min">Minimum value</param>
    /// <param name="max">Maximum value</param>
    /// <param name="getter">Used by widget to get current value.</param>
    /// <param name="setter">Invoked by widget with argument equal to new value.</param>
    /// <returns>Widget id that can be used to <see cref="Remove(string, int)"/> it</returns>
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

    /// <summary>
    /// Displays a line of text.<br/>Call with same <paramref name="id"/> to update displayed text instead of adding new one.
    /// </summary>
    /// <param name="domain">Title and id of window that will be used to draw widget</param>
    /// <param name="category">Name and id of a tab inside ImGui window to draw widget in</param>
    /// <param name="id">>Unique id inside a domain. Is used as way to identify same widget on subsequent calls.</param>
    /// <param name="text">Text to display</param>
    /// <returns>Widget id that can be used to <see cref="Remove(string, int)"/> it</returns>
    public static int Text(string domain, string category, int id, string text)
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () => ImGui.Text(text));
        return id;
    }
    /// <summary>
    /// A box for editing a line of text
    /// </summary>
    /// <param name="domain">Title and id of window that will be used to draw widget</param>
    /// <param name="category">Name and id of a tab inside ImGui window to draw widget in</param>
    /// <param name="label">Unique inside domain. Label and id of the widget. Used to identify same widgets to replace them instead of creating new one.</param>
    /// <param name="getter">Used by widget to get current value.</param>
    /// <param name="setter">Invoked by widget with argument equal to new value.</param>
    /// <param name="maxLength">Max length of string in characters</param>
    /// <param name="controlButtons">If true there will be buttons to copy/paste value from/into this editor</param>
    /// <returns>Widget id that can be used to <see cref="Remove(string, int)"/> it</returns>
    public static int TextInput(string domain, string category, string label, System.Func<string> getter, Action<string> setter, uint maxLength = 512, bool controlButtons = false)
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        int id = label.GetHashCode();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () =>
        {
            string value = getter?.Invoke() ?? "";
            if (controlButtons)
            {
                Controls.CopyPasteSupport(ref value, $"Copy##{label}", $"Paste##{label}");
                ImGui.SameLine();
            }
            ImGui.InputText(label, ref value, maxLength);
            setter?.Invoke(value);
        });
        return id;
    }
    /// <summary>
    /// A box for editing multiline text
    /// </summary>
    /// <param name="domain">Title and id of window that will be used to draw widget</param>
    /// <param name="category">Name and id of a tab inside ImGui window to draw widget in</param>
    /// <param name="label">Unique inside domain. Label and id of the widget. Used to identify same widgets to replace them instead of creating new one.</param>
    /// <param name="getter">Used by widget to get current value.</param>
    /// <param name="setter">Invoked by widget with argument equal to new value.</param>
    /// <param name="maxLength">Max length of string in characters</param>
    /// <param name="controlButtons">If true there will be buttons to copy/paste value from/into this editor</param>
    /// <param name="flags">Flags passed to <see cref="ImGui.InputTextMultiline(ReadOnlySpan{char}, ref string, uint, Vector2, ImGuiInputTextFlags)"/></param>
    /// <param name="width">Sets edit area width if greater than zero</param>
    /// <param name="height">Sets edit area height if greater than zero</param>
    /// <returns>Widget id that can be used to <see cref="Remove(string, int)"/> it</returns>
    public static int TextInputMultiline(string domain, string category, string label, System.Func<string> getter, Action<string> setter, uint maxLength = 512, bool controlButtons = false, ImGuiInputTextFlags flags = ImGuiInputTextFlags.None, float width = 0, float height = 0)
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        int id = label.GetHashCode();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () =>
        {
            string value = getter?.Invoke() ?? "";
            if (controlButtons) Controls.CopyPasteSupport(ref value, $"Copy##{label}", $"Paste##{label}");
            ImGui.InputTextMultiline(label, ref value, maxLength, new(width, height), flags);
            setter?.Invoke(value);
        });
        return id;
    }

    /// <summary>
    /// Box for entering <see cref="int"/> value. Can be dragged with mouse to change value.
    /// </summary>
    /// <param name="domain">Title and id of window that will be used to draw widget</param>
    /// <param name="category">Name and id of a tab inside ImGui window to draw widget in</param>
    /// <param name="label">Unique inside domain. Label and id of the widget. Used to identify same widgets to replace them instead of creating new one.</param>
    /// <param name="getter">Used by widget to get current value.</param>
    /// <param name="setter">Invoked by widget with argument equal to new value.</param>
    /// <returns>Widget id that can be used to <see cref="Remove(string, int)"/> it</returns>
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
    /// <summary>
    /// Box for entering <see cref="float"/> value. Can be dragged with mouse to change value.
    /// </summary>
    /// <param name="domain">Title and id of window that will be used to draw widget</param>
    /// <param name="category">Name and id of a tab inside ImGui window to draw widget in</param>
    /// <param name="label">Unique inside domain. Label and id of the widget. Used to identify same widgets to replace them instead of creating new one.</param>
    /// <param name="getter">Used by widget to get current value.</param>
    /// <param name="setter">Invoked by widget with argument equal to new value.</param>
    /// <returns>Widget id that can be used to <see cref="Remove(string, int)"/> it</returns>
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
    /// <summary>
    /// Box for entering <see cref="Vec3f"/> value. Can be dragged with mouse to change value.
    /// </summary>
    /// <param name="domain">Title and id of window that will be used to draw widget</param>
    /// <param name="category">Name and id of a tab inside ImGui window to draw widget in</param>
    /// <param name="label">Unique inside domain. Label and id of the widget. Used to identify same widgets to replace them instead of creating new one.</param>
    /// <param name="getter">Used by widget to get current value.</param>
    /// <param name="setter">Invoked by widget with argument equal to new value.</param>
    /// <returns>Widget id that can be used to <see cref="Remove(string, int)"/> it</returns>
    public static int Float3Drag(string domain, string category, string label, System.Func<Vec3f> getter, Action<Vec3f> setter)
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        int id = label.GetHashCode();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () =>
        {
            Vec3f value = getter?.Invoke() ?? new(0, 0, 0);
            Vector3 vector = new(value.X, value.Y, value.Z);
            ImGui.DragFloat3(label, ref vector);
            value.X = vector.X;
            value.Y = vector.Y;
            value.Z = vector.Z;
            setter?.Invoke(value);
        });
        return id;
    }

    /// <summary>
    /// A line with four boxes for entering values of color channels and a color button that displays color and can open color picker.
    /// </summary>
    /// <param name="domain">Title and id of window that will be used to draw widget</param>
    /// <param name="category">Name and id of a tab inside ImGui window to draw widget in</param>
    /// <param name="label">Unique inside domain. Label and id of the widget. Used to identify same widgets to replace them instead of creating new one.</param>
    /// <param name="getter">Used by widget to get current value.</param>
    /// <param name="setter">Invoked by widget with argument equal to new value.</param>
    /// <param name="flags">Flags passed to <see cref="ImGui.ColorEdit4(ReadOnlySpan{char}, ref Vector4, ImGuiColorEditFlags)"/>.</param>
    /// <returns>Widget id that can be used to <see cref="Remove(string, int)"/> it</returns>
    public static int ColorPicker(string domain, string category, string label, System.Func<Vec4f> getter, Action<Vec4f> setter, ImGuiColorEditFlags flags = ImGuiColorEditFlags.None)
    {
        if (!DebugWindowsManager.DrawEntries.ContainsKey(domain)) DebugWindowsManager.DrawEntries[domain] = new();
        int id = label.GetHashCode();
        DebugWindowsManager.DrawEntries[domain][id] = new(domain, category, () =>
        {
            Vec4f value = getter?.Invoke() ?? new(0, 0, 0, 0);
            Vector4 vector = new(value.X, value.Y, value.Z, value.W);
            ImGui.ColorEdit4(label, ref vector, flags);
            value.X = vector.X;
            value.Y = vector.Y;
            value.Z = vector.Z;
            value.W = vector.W;
            setter?.Invoke(value);
        });
        return id;
    }

    /// <summary>
    /// Combo-box for selecting a value from <typeparamref name="TEnum"/> enum.
    /// </summary>
    /// <typeparam name="TEnum">Enum which values will be in combo-box.</typeparam>
    /// <param name="domain">Title and id of window that will be used to draw widget</param>
    /// <param name="category">Name and id of a tab inside ImGui window to draw widget in</param>
    /// <param name="label">Unique inside domain. Label and id of the widget. Used to identify same widgets to replace them instead of creating new one.</param>
    /// <param name="getter">Used by widget to get current value.</param>
    /// <param name="setter">Invoked by widget with argument equal to new value.</param>
    /// <returns>Widget id that can be used to <see cref="Remove(string, int)"/> it</returns>
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
    /// <summary>
    /// Scrollable list for selecting a value from <typeparamref name="TEnum"/> enum.
    /// </summary>
    /// <typeparam name="TEnum">Enum which values will be in the list.</typeparam>
    /// <param name="domain">Title and id of window that will be used to draw widget</param>
    /// <param name="category">Name and id of a tab inside ImGui window to draw widget in</param>
    /// <param name="label">Unique inside domain. Label and id of the widget. Used to identify same widgets to replace them instead of creating new one.</param>
    /// <param name="getter">Used by widget to get current value.</param>
    /// <param name="setter">Invoked by widget with argument equal to new value.</param>
    /// <returns>Widget id that can be used to <see cref="Remove(string, int)"/> it</returns>
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
    /// <summary>
    /// Filtered scrollable list for selecting a value from <typeparamref name="TEnum"/> enum.<br/>Has field for editing filter.<br/>Uses wildcards.
    /// </summary>
    /// <typeparam name="TEnum">Enum which values will be in the list.</typeparam>
    /// <param name="domain">Title and id of window that will be used to draw widget</param>
    /// <param name="category">Name and id of a tab inside ImGui window to draw widget in</param>
    /// <param name="label">Unique inside domain. Label and id of the widget. Used to identify same widgets to replace them instead of creating new one.</param>
    /// <param name="getter">Used by widget to get current value.</param>
    /// <param name="setter">Invoked by widget with argument equal to new value.</param>
    /// <param name="filterHolder">String that will be used to store filter value.</param>
    /// <returns>Widget id that can be used to <see cref="Remove(string, int)"/> it</returns>
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

    /// <summary>
    /// Filtered scrollable list of strings that can be selected.
    /// </summary>
    /// <param name="domain">Title and id of window that will be used to draw widget</param>
    /// <param name="category">Name and id of a tab inside ImGui window to draw widget in</param>
    /// <param name="label">Unique inside domain. Label and id of the widget. Used to identify same widgets to replace them instead of creating new one.</param>
    /// <param name="filterHolder">String that will be used to store filter value.</param>
    /// <param name="list">List of string to display and filter</param>
    /// <param name="getter">Used to get current selected index.</param>
    /// <param name="indexSetter">Used to set current selected index.</param>
    /// <param name="valueSetter">Used to set current selected value.</param>
    /// <returns>Widget id that can be used to <see cref="Remove(string, int)"/> it</returns>
    public static int StringList(string domain, string category, string label, string filterHolder, string[] list, System.Func<int> getter, Action<int>? indexSetter, Action<string>? valueSetter = null)
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
    /// <summary>
    /// Filtered scrollable list of strings that can be selected.
    /// </summary>
    /// <param name="domain">Title and id of window that will be used to draw widget</param>
    /// <param name="category">Name and id of a tab inside ImGui window to draw widget in</param>
    /// <param name="label">Unique inside domain. Label and id of the widget. Used to identify same widgets to replace them instead of creating new one.</param>
    /// <param name="filterHolder">String that will be used to store filter value.</param>
    /// <param name="list">List of string to display and filter</param>
    /// <param name="getter">Used to get current selected index.</param>
    /// <param name="indexSetter">Used to set current selected index.</param>
    /// <param name="valueSetter">Used to set current selected value.</param>
    /// <returns>Widget id that can be used to <see cref="Remove(string, int)"/> it</returns>
    public static int StringList(string domain, string category, string label, string filterHolder, string[] list, System.Func<int> getter, Action<string>? valueSetter, Action<int>? indexSetter = null)
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