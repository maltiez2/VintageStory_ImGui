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