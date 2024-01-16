using ImGuiNET;
using Newtonsoft.Json;
using System;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Datastructures;

namespace VSImGui;

[JsonObject(MemberSerialization.OptOut)]
public class WindowStyle : Style
{
    public WindowStyle() : base()
    {

    }

    public WindowStyle(ICoreClientAPI api, Style style) : base(style)
    {
        mApi = api;
    }

    public WindowStyle(ICoreClientAPI api) : base()
    {
        mApi = api;
    }

    public WindowStyle(ICoreClientAPI api, string fontName, int fontSize = 14) : base(fontName, fontSize)
    {
        mApi = api;
    }

    public WindowStyle(ICoreClientAPI api, ImGuiStyle style, string fontName, int fontSize = 14) : base(style, fontName, fontSize)
    {
        mApi = api;
    }

    // TEXTURE BACKGROUND
    public string BackgroundTexture
    {
        get => mBackgroundTexturePath;

        set
        {
            mBackgroundTexturePath = value;
            try
            {
                mBackgroundTexture = mApi?.Render.GetOrLoadTexture(new Vintagestory.API.Common.AssetLocation(mBackgroundTexturePath)) ?? -1;
            }
            catch
            {
                mBackgroundTexture = -1;
            }
        }
    }
    public Value2 BackgroundTextureSize { get; set; } = (-1, -1);
    public bool BackgroundTextureApplied
    {
        get => mBackgroundTexture > 0 && BackgroundTextureSize.X > 0 && BackgroundTextureSize.Y > 0 && mBackgroundTextureApplied;
        set
        {
            if (value)
            {
                BackgroundTexture = mBackgroundTexturePath;
                mBackgroundTextureApplied = true;
            }
            else
            {
                mBackgroundTexture = -1;
                mBackgroundTextureApplied = false;
            }
        }
    }
    [JsonIgnore]
    public ICoreClientAPI Api => mApi;

    [JsonProperty("WindowFlags")]
    public WindowFlags Flags = new WindowFlags() { WindowFlagsValue = ImGuiWindowFlags.None };

    private ICoreClientAPI mApi;
    private int mBackgroundTexture = -1;
    private string mBackgroundTexturePath = "";
    private bool mBackgroundTextureApplied = false;

    public bool Begin(string name, ref bool open)
    {
        Push();
        bool start = ImGui.Begin(name, ref open, Flags.WindowFlagsValue);
        if (BackgroundTextureApplied) TilingUtils.TileWindowWithTexture(mBackgroundTexture, BackgroundTextureSize);
        return start;
    }
    public bool Begin(string name)
    {
        Push();
        bool start = ImGui.Begin(name, Flags.WindowFlagsValue);
        if (BackgroundTextureApplied) TilingUtils.TileWindowWithTexture(mBackgroundTexture, BackgroundTextureSize);
        return start;
    }
    public void End()
    {
        Pop();
        ImGui.End();
    }
    public void OnDeserilization(ICoreClientAPI api)
    {
        mApi = api;
        if (mBackgroundTextureApplied) BackgroundTexture = mBackgroundTexturePath;
    }


    public override void SetFrom(Style style)
    {
        base.SetFrom(style);
        if (style is not WindowStyle windowStyle) return;

        Flags = windowStyle.Flags;
        mBackgroundTexture = windowStyle.mBackgroundTexture;
        BackgroundTextureSize = windowStyle.BackgroundTextureSize;
        mBackgroundTexturePath = windowStyle.mBackgroundTexturePath;
        mApi = windowStyle.mApi;
    }
    public override string ToCode()
    {
        StringBuilder result = new();

        result.AppendLine("VSImGui.WindowStyle style = new(api);");

        TexturePropertiesToCode(result);
        WindowPropertiesToCode(result);
        OtherPropertiesToCode(result);
        FloatPropertiesToCode(result);
        Value2PropertiesToCode(result);
        Value4PropertiesToCode(result);

        return result.ToString();
    }

    protected void TexturePropertiesToCode(StringBuilder result)
    {
        if (mBackgroundTextureApplied) result.AppendLine($"style.BackgroundTexture = \"{BackgroundTexture}\";");
        if (mBackgroundTextureApplied) result.AppendLine($"style.BackgroundTextureSize = ({BackgroundTextureSize.X}, {BackgroundTextureSize.Y});");
    }
    protected void WindowPropertiesToCode(StringBuilder result)
    {
        if (Flags.NoTitleBar) result.AppendLine($"style.Flags.NoTitleBar = true;");
        if (Flags.NoResize) result.AppendLine($"style.Flags.NoResize = true;");
        if (Flags.NoMove) result.AppendLine($"style.Flags.NoMove = true;");
        if (Flags.NoScrollbar) result.AppendLine($"style.Flags.NoScrollbar = true;");
        if (Flags.NoScrollWithMouse) result.AppendLine($"style.Flags.NoScrollWithMouse = true;");
        if (Flags.NoCollapse) result.AppendLine($"style.Flags.NoCollapse = true;");
        if (Flags.AlwaysAutoResize) result.AppendLine($"style.Flags.AlwaysAutoResize = true;");
        if (Flags.NoBackground) result.AppendLine($"style.Flags.NoBackground = true;");
        if (Flags.NoSavedSettings) result.AppendLine($"style.Flags.NoSavedSettings = true;");
        if (Flags.NoMouseInputs) result.AppendLine($"style.Flags.NoMouseInputs = true;");
        if (Flags.MenuBar) result.AppendLine($"style.Flags.MenuBar = true;");
        if (Flags.HorizontalScrollbar) result.AppendLine($"style.Flags.HorizontalScrollbar = true;");
        if (Flags.NoFocusOnAppearing) result.AppendLine($"style.Flags.NoFocusOnAppearing = true;");
        if (Flags.NoBringToFrontOnFocus) result.AppendLine($"style.Flags.NoBringToFrontOnFocus = true;");
        if (Flags.AlwaysVerticalScrollbar) result.AppendLine($"style.Flags.AlwaysVerticalScrollbar = true;");
        if (Flags.AlwaysHorizontalScrollbar) result.AppendLine($"style.Flags.AlwaysHorizontalScrollbar = true;");
        if (Flags.AlwaysUseWindowPadding) result.AppendLine($"style.Flags.AlwaysUseWindowPadding = true;");
        if (Flags.NoNavInputs) result.AppendLine($"style.Flags.NoNavInputs = true;");
        if (Flags.NoNavFocus) result.AppendLine($"style.Flags.NoNavFocus = true;");
        if (Flags.UnsavedDocument) result.AppendLine($"style.Flags.UnsavedDocument = true;");
        if (Flags.NoDocking) result.AppendLine($"style.Flags.NoDocking = true;");
        if (Flags.NoNav) result.AppendLine($"style.Flags.NoNav = true;");
        if (Flags.NoDecoration) result.AppendLine($"style.Flags.NoDecoration = true;");
        if (Flags.NoInputs) result.AppendLine($"style.Flags.NoInputs = true;");
        if (Flags.NavFlattened) result.AppendLine($"style.Flags.NavFlattened = true;");
        if (Flags.ChildWindow) result.AppendLine($"style.Flags.ChildWindow = true;");
        if (Flags.Tooltip) result.AppendLine($"style.Flags.Tooltip = true;");
        if (Flags.Popup) result.AppendLine($"style.Flags.Popup = true;");
        if (Flags.Modal) result.AppendLine($"style.Flags.Modal = true;");
        if (Flags.ChildMenu) result.AppendLine($"style.Flags.ChildMenu = true;");
    }
}

public class StyleApplier : IDisposable
{
    private readonly Style mStyle;
    private bool disposedValue;

    public StyleApplier(Style style)
    {
        mStyle = style;
        mStyle.Push();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                mStyle.Pop();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

[JsonObject(MemberSerialization.OptOut)]
public struct WindowFlags
{
    public WindowFlags() { }
    
    [JsonIgnore]
    public ImGuiWindowFlags WindowFlagsValue { get; set; } = ImGuiWindowFlags.None;
    [JsonIgnore]
    public bool None { get => WindowFlagsValue == 0; set => WindowFlagsValue = value ? 0 : WindowFlagsValue; }
    public bool NoTitleBar { get => (WindowFlagsValue & ImGuiWindowFlags.NoTitleBar) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.NoTitleBar : WindowFlagsValue & ~ImGuiWindowFlags.NoTitleBar; }
    public bool NoResize { get => (WindowFlagsValue & ImGuiWindowFlags.NoResize) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.NoResize : WindowFlagsValue & ~ImGuiWindowFlags.NoResize; }
    public bool NoMove { get => (WindowFlagsValue & ImGuiWindowFlags.NoMove) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.NoMove : WindowFlagsValue & ~ImGuiWindowFlags.NoMove; }
    public bool NoScrollbar { get => (WindowFlagsValue & ImGuiWindowFlags.NoScrollbar) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.NoScrollbar : WindowFlagsValue & ~ImGuiWindowFlags.NoScrollbar; }
    public bool NoScrollWithMouse { get => (WindowFlagsValue & ImGuiWindowFlags.NoScrollWithMouse) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.NoScrollWithMouse : WindowFlagsValue & ~ImGuiWindowFlags.NoScrollWithMouse; }
    public bool NoCollapse { get => (WindowFlagsValue & ImGuiWindowFlags.NoCollapse) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.NoCollapse : WindowFlagsValue & ~ImGuiWindowFlags.NoCollapse; }
    public bool AlwaysAutoResize { get => (WindowFlagsValue & ImGuiWindowFlags.AlwaysAutoResize) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.AlwaysAutoResize : WindowFlagsValue & ~ImGuiWindowFlags.AlwaysAutoResize; }
    public bool NoBackground { get => (WindowFlagsValue & ImGuiWindowFlags.NoBackground) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.NoBackground : WindowFlagsValue & ~ImGuiWindowFlags.NoBackground; }
    public bool NoSavedSettings { get => (WindowFlagsValue & ImGuiWindowFlags.NoSavedSettings) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.NoSavedSettings : WindowFlagsValue & ~ImGuiWindowFlags.NoSavedSettings; }
    public bool NoMouseInputs { get => (WindowFlagsValue & ImGuiWindowFlags.NoMouseInputs) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.NoMouseInputs : WindowFlagsValue & ~ImGuiWindowFlags.NoMouseInputs; }
    public bool MenuBar { get => (WindowFlagsValue & ImGuiWindowFlags.MenuBar) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.MenuBar : WindowFlagsValue & ~ImGuiWindowFlags.MenuBar; }
    public bool HorizontalScrollbar { get => (WindowFlagsValue & ImGuiWindowFlags.HorizontalScrollbar) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.HorizontalScrollbar : WindowFlagsValue & ~ImGuiWindowFlags.HorizontalScrollbar; }
    public bool NoFocusOnAppearing { get => (WindowFlagsValue & ImGuiWindowFlags.NoFocusOnAppearing) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.NoFocusOnAppearing : WindowFlagsValue & ~ImGuiWindowFlags.NoFocusOnAppearing; }
    public bool NoBringToFrontOnFocus { get => (WindowFlagsValue & ImGuiWindowFlags.NoBringToFrontOnFocus) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.NoBringToFrontOnFocus : WindowFlagsValue & ~ImGuiWindowFlags.NoBringToFrontOnFocus; }
    public bool AlwaysVerticalScrollbar { get => (WindowFlagsValue & ImGuiWindowFlags.AlwaysVerticalScrollbar) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.AlwaysVerticalScrollbar : WindowFlagsValue & ~ImGuiWindowFlags.AlwaysVerticalScrollbar; }
    public bool AlwaysHorizontalScrollbar { get => (WindowFlagsValue & ImGuiWindowFlags.AlwaysHorizontalScrollbar) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.AlwaysHorizontalScrollbar : WindowFlagsValue & ~ImGuiWindowFlags.AlwaysHorizontalScrollbar; }
    public bool AlwaysUseWindowPadding { get => (WindowFlagsValue & ImGuiWindowFlags.AlwaysUseWindowPadding) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.AlwaysUseWindowPadding : WindowFlagsValue & ~ImGuiWindowFlags.AlwaysUseWindowPadding; }
    public bool NoNavInputs { get => (WindowFlagsValue & ImGuiWindowFlags.NoNavInputs) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.NoNavInputs : WindowFlagsValue & ~ImGuiWindowFlags.NoNavInputs; }
    public bool NoNavFocus { get => (WindowFlagsValue & ImGuiWindowFlags.NoNavFocus) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.NoNavFocus : WindowFlagsValue & ~ImGuiWindowFlags.NoNavFocus; }
    public bool UnsavedDocument { get => (WindowFlagsValue & ImGuiWindowFlags.UnsavedDocument) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.UnsavedDocument : WindowFlagsValue & ~ImGuiWindowFlags.UnsavedDocument; }
    public bool NoDocking { get => (WindowFlagsValue & ImGuiWindowFlags.NoDocking) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.NoDocking : WindowFlagsValue & ~ImGuiWindowFlags.NoDocking; }
    public bool NoNav { get => (WindowFlagsValue & ImGuiWindowFlags.NoNav) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.NoNav : WindowFlagsValue & ~ImGuiWindowFlags.NoNav; }
    public bool NoDecoration { get => (WindowFlagsValue & ImGuiWindowFlags.NoDecoration) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.NoDecoration : WindowFlagsValue & ~ImGuiWindowFlags.NoDecoration; }
    public bool NoInputs { get => (WindowFlagsValue & ImGuiWindowFlags.NoInputs) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.NoInputs : WindowFlagsValue & ~ImGuiWindowFlags.NoInputs; }
    public bool NavFlattened { get => (WindowFlagsValue & ImGuiWindowFlags.NavFlattened) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.NavFlattened : WindowFlagsValue & ~ImGuiWindowFlags.NavFlattened; }
    public bool ChildWindow { get => (WindowFlagsValue & ImGuiWindowFlags.ChildWindow) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.ChildWindow : WindowFlagsValue & ~ImGuiWindowFlags.ChildWindow; }
    public bool Tooltip { get => (WindowFlagsValue & ImGuiWindowFlags.Tooltip) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.Tooltip : WindowFlagsValue & ~ImGuiWindowFlags.Tooltip; }
    public bool Popup { get => (WindowFlagsValue & ImGuiWindowFlags.Popup) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.Popup : WindowFlagsValue & ~ImGuiWindowFlags.Popup; }
    public bool Modal { get => (WindowFlagsValue & ImGuiWindowFlags.Modal) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.Modal : WindowFlagsValue & ~ImGuiWindowFlags.Modal; }
    public bool ChildMenu { get => (WindowFlagsValue & ImGuiWindowFlags.ChildMenu) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.ChildMenu : WindowFlagsValue & ~ImGuiWindowFlags.ChildMenu; }
    public bool DockNodeHost { get => (WindowFlagsValue & ImGuiWindowFlags.DockNodeHost) != 0; set => WindowFlagsValue = value ? WindowFlagsValue | ImGuiWindowFlags.DockNodeHost : WindowFlagsValue & ~ImGuiWindowFlags.DockNodeHost; }

    static public readonly string[] WindowFlagsEnum = new string[]
    {
        //"None",
        "NoTitleBar",
        "NoResize",
        "NoMove",
        "NoScrollbar",
        "NoScrollWithMouse",
        "NoCollapse",
        "AlwaysAutoResize",
        "NoBackground",
        "NoSavedSettings",
        "NoMouseInputs",
        "MenuBar",
        "HorizontalScrollbar",
        "NoFocusOnAppearing",
        "NoBringToFrontOnFocus",
        "AlwaysVerticalScrollbar",
        "AlwaysHorizontalScrollbar",
        "AlwaysUseWindowPadding",
        "NoNavInputs",
        "NoNavFocus",
        "UnsavedDocument",
        "NoDocking",
        "NoNav",
        "NoDecoration",
        "NoInputs",
        //"NavFlattened",
        //"ChildWindow",
        "Tooltip",
        //"Popup",
        "Modal",
        //"ChildMenu",
        "DockNodeHost"
    };
    static public readonly int[] WindowFlagsEnumValues = new int[]
    {
        //0x0,
        0x1,
        0x2,
        0x4,
        0x8,
        0x10,
        0x20,
        0x40,
        0x80,
        0x100,
        0x200,
        0x400,
        0x800,
        0x1000,
        0x2000,
        0x4000,
        0x8000,
        0x10000,
        0x40000,
        0x80000,
        0x100000,
        0x200000,
        0xC0000,
        0x2B,
        0xC0200,
        //0x800000,
        //0x1000000,
        0x2000000,
        //0x4000000,
        0x8000000,
        //0x10000000,
        0x20000000
    };
    public void DrawEditor(string name)
    {
        int dir = (int)WindowFlagsValue;

        if (ImGui.CollapsingHeader(name, ImGuiTreeNodeFlags.DefaultOpen))
        {
            ImGui.Indent();
            for (int index = 0; index < WindowFlagsEnum.Length; index++)
            {
                ImGui.CheckboxFlags($"{WindowFlagsEnum[index]}##{name}", ref dir, WindowFlagsEnumValues[index]);
            }
            ImGui.Unindent();
        }

        WindowFlagsValue = (ImGuiWindowFlags)dir;
    }
}
