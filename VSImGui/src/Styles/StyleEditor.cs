using ImGuiNET;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace VSImGui;

public class StyleEditor
{
    private Style mStyle;
    private Style mBackup;
    private string mColorsFilter = "";
    private string mLayoutFilter = "";
    private Guid mId = Guid.NewGuid();

    public StyleEditor(Style style)
    {
        mStyle = style;
        mBackup = new(style);
    }

    static private uint sIdCounter = 0;
    public bool Draw()
    {
        sIdCounter = 0;
        bool opened = true;
        if (!ImGui.Begin($"Style editor##vsimgui{mId}", ref opened, ImGuiWindowFlags.MenuBar)) return false;

        if (ImGui.BeginMenuBar())
        {
            ImGui.BeginDisabled();
            if (ImGui.MenuItem("Save##StyleEditor.vsimgui")) Save();
            if (ImGui.MenuItem("Load##StyleEditor.vsimgui")) Load();
            ImGui.EndDisabled();
            if (ImGui.MenuItem("Restore##StyleEditor.vsimgui")) mStyle.SetFrom(mBackup);
            Code(ImGui.MenuItem("Code##StyleEditor.vsimgui"));
            ImGui.EndMenuBar();
        }

        if (ImGui.BeginTabBar("##StyleEditor.vsimgui.tabs", ImGuiTabBarFlags.None))
        {
            if (ImGui.BeginTabItem("Font & texture"))
            {
                FontAndTextureEditor(mStyle);
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Colors"))
            {
                ColorsEditor(mStyle, ref mColorsFilter);
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Layout"))
            {
                LayoutSettingsEditor(mStyle, ref mLayoutFilter);
                ImGui.EndTabItem();
            }

            if (mStyle is WindowStyle windowStyle && ImGui.BeginTabItem("Window"))
            {
                windowStyle.Flags.DrawEditor("Window flags");
                ImGui.EndTabItem();
            }

            ImGui.EndTabBar();
        }

        ImGui.End();
        return opened;
    }

    public void Save()
    {

    }
    public void Load()
    {

    }

    private string mSerialized = "";
    private string mCsharpCode = "";
    private bool mOutputOpened = false;
    private bool mException = false;
    private string mMessage = "";
    private int mOutputOption = 0;
    public void Code(bool open)
    {
        if (open)
        {
            ImGui.OpenPopup("Output");
            mSerialized = JsonConvert.SerializeObject(mStyle, Formatting.Indented);
            mCsharpCode = mStyle.ToCode();
            mOutputOpened = true;
        }

        if (mOutputOpened && ImGui.BeginPopupModal("Output", ref mOutputOpened))
        {
            ImGui.RadioButton("JSON##codeoutputvsimgui", ref mOutputOption, 0); ImGui.SameLine();
            ImGui.RadioButton("C# ##codeoutputvsimgui", ref mOutputOption, 1);

            switch (mOutputOption)
            {
                case 0:
                    ImGui.SeparatorText("JSON");
                    JsonCode();
                    break;
                case 1:
                    ImGui.SeparatorText("C#");
                    CsharpCode();
                    break;
            }

            ImGui.EndPopup();
        }
    }

    private void CsharpCode()
    {
        Vector2 size = ImGui.GetWindowSize();
        size.X -= 10;
        size.Y -= 94;

        ImGui.InputTextMultiline("##C#resultjsonoutputvsimgui", ref mCsharpCode, (uint)mCsharpCode.Length * 2, size, ImGuiInputTextFlags.ReadOnly);
    }
    private void JsonCode()
    {
        Vector2 size = ImGui.GetWindowSize();
        size.X -= 10;
        size.Y -= 124;

        if (ImGui.Button("Apply##jsonoutputvsimgui")) Deserialize(mSerialized);
        if (mException) ShowException(ref size);
        ImGui.InputTextMultiline("##JSONresultjsonoutputvsimgui", ref mSerialized, (uint)mSerialized.Length * 2, size);
    }

    private void Deserialize(string json)
    {
        try
        {
            mException = false;
            if (mStyle is WindowStyle windowStyle)
            {
                WindowStyle style = JsonConvert.DeserializeObject<WindowStyle>(json);
                style.OnDeserilization(windowStyle.Api);
                mStyle.SetFrom(style);
            }
            else
            {
                Style style = JsonConvert.DeserializeObject<Style>(json);
                mStyle.SetFrom(style);
            }
        }
        catch (Exception exception)
        {
            mException = true;
            mMessage = $"Error: {exception.Message}\n\nStack trace: {exception.StackTrace}";
        }
    }
    private void ShowException(ref Vector2 size)
    {
        Vector2 errorSize = ImGui.GetWindowSize();
        errorSize.X -= 10;
        errorSize.Y = 100;
        size.Y -= 104;

        ImGui.BeginChild("ApplyExceptionjsonoutputvsimgui", errorSize, true);
        ImGui.PushTextWrapPos(ImGui.GetCursorPosX() + errorSize.X);
        ImGui.Text(mMessage);
        ImGui.PopTextWrapPos();
        ImGui.EndChild();
    }

    static private void FontAndTextureEditor(Style style)
    {
        string[] fonts = FontManager.Fonts.ToArray().Prepend("").ToArray();
        string[] fontsNames = FontManager.Fonts.Select(font => Path.GetFileNameWithoutExtension(font)).ToArray().Prepend("").ToArray();
        int[] sizes = FontManager.Sizes.ToArray();
        string[] sizesNames = FontManager.Sizes.Select(size => $"{size}px").ToArray();
        int currentSizeIndex, currentFontIndex;
        for (currentFontIndex = 0; currentFontIndex < fonts.Length; currentFontIndex++)
        {
            if (fontsNames[currentFontIndex] == style.FontName) break;
        }
        for (currentSizeIndex = 0; currentSizeIndex < fonts.Length; currentSizeIndex++)
        {
            if (sizes[currentSizeIndex] == style.FontSize) break;
        }

        ImGui.Combo("Font", ref currentFontIndex, fontsNames, fonts.Length);
        ImGui.Combo("Size", ref currentSizeIndex, sizesNames, sizes.Length);

        style.Font = (fontsNames[currentFontIndex], sizes[currentSizeIndex]);

        if (style is WindowStyle windowStyle)
        {
            if (!windowStyle.BackgroundTextureApplied && ImGui.Button("Reenable texture"))
                windowStyle.BackgroundTextureApplied = true;
            else if (windowStyle.BackgroundTextureApplied && ImGui.Button("Disable texture"))
                windowStyle.BackgroundTextureApplied = false;

            string texture = (string)windowStyle.BackgroundTexture.Clone();
            if (ImGui.InputText("Texture path with domain", ref texture, 1000, ImGuiInputTextFlags.EnterReturnsTrue))
            {
                windowStyle.BackgroundTexture = texture;
            }

            Vector2 size = windowStyle.BackgroundTextureSize;
            ImGui.InputFloat2("Texture size", ref size);
            windowStyle.BackgroundTextureSize = size;
        }
    }
    static private void ColorsEditor(Style style, ref string filter)
    {
        ImGui.InputTextWithHint("Filter colors", "filter, supports wildcards", ref filter, 100);
        string regular = WildCardToRegular(filter);
        ImGui.BeginChild("Filtered colors", new Vector2(), true);

        BackgroundColorsEditor(regular, style);
        TextEditor(regular, style);
        BordersEditor(regular, style);
        ScrollEditor(regular, style);
        ButtonsEditor(regular, style);
        HeadersEditor(regular, style);
        SeparatorEditor(regular, style);
        ResizeEditor(regular, style);
        TabsEditor(regular, style);
        PlotsEditor(regular, style);
        OtherColorsEditor(regular, style);

        ImGui.EndChild();
    }
    static private void LayoutSettingsEditor(Style style, ref string filter)
    {
        ImGui.InputTextWithHint("Filter settings", "filter, supports wildcards", ref filter, 100);
        string regular = WildCardToRegular(filter);
        ImGui.BeginChild("Filtered settings", new Vector2(), true);

        SizesEditor(regular, style);
        SpacingEditor(regular, style);
        BordersLayoutEditor(regular, style);
        PaddingsEditor(regular, style);
        RoundingEditor(regular, style);
        AlignEditor(regular, style);
        HoverEditor(regular, style);
        RenderingEditor(regular, style);
        OtherEditor(regular, style);

        ImGui.EndChild();
    }

    // LAYOUT
    static private void PaddingsEditor(string filter, Style style)
    {
        if (!Match(filter,
            "Window",
            "Frame",
            "Cell",
            "TouchExtra",
            "SeparatorText",
            "DisplayWindow",
            "DisplaySafeArea"
            ))
        {
            return;
        }

        ImGui.SeparatorText("Paddings");
        style.PaddingWindow = Pair("PaddingWindow", "Window", filter, style.PaddingWindow);
        style.PaddingFrame = Pair("PaddingFrame", "Frame", filter, style.PaddingFrame);
        style.PaddingCell = Pair("PaddingCell", "Cell", filter, style.PaddingCell);
        style.PaddingTouchExtra = Pair("PaddingTouchExtra", "TouchExtra", filter, style.PaddingTouchExtra);
        style.PaddingSeparatorText = Pair("PaddingSeparatorText", "SeparatorText", filter, style.PaddingSeparatorText);
        style.PaddingDisplayWindow = Pair("PaddingDisplayWindow", "DisplayWindow", filter, style.PaddingDisplayWindow);
        style.PaddingDisplaySafeArea = Pair("PaddingDisplaySafeArea", "DisplaySafeArea", filter, style.PaddingDisplaySafeArea);
    }
    static private void SpacingEditor(string filter, Style style)
    {
        if (!Match(filter,
            "Item",
            "ItemInner",
            "Indent",
            "ColumnsMin"
            ))
        {
            return;
        }

        ImGui.SeparatorText("Spacing");
        style.SpacingItem = Pair("SpacingItem", "Item", filter, style.SpacingItem);
        style.SpacingItemInner = Pair("SpacingItemInner", "ItemInner", filter, style.SpacingItemInner);
        style.SpacingIndent = Float("SpacingIndent", "Indent", filter, style.SpacingIndent);
        style.SpacingColumnsMin = Float("SpacingColumnsMin", "ColumnsMin", filter, style.SpacingColumnsMin);
    }
    static private void BordersLayoutEditor(string filter, Style style)
    {
        if (!Match(filter,
            "Window",
            "Child",
            "Popup",
            "Frame",
            "Tab",
            "SeparatorText"
            ))
        {
            return;
        }

        ImGui.SeparatorText("Borders");
        style.BorderWindow = Float("BorderWindow", "Window", filter, style.BorderWindow);
        style.BorderChild = Float("BorderChild", "Child", filter, style.BorderChild);
        style.BorderPopup = Float("BorderPopup", "Popup", filter, style.BorderPopup);
        style.BorderFrame = Float("BorderFrame", "Frame", filter, style.BorderFrame);
        style.BorderTab = Float("BorderTab", "Tab", filter, style.BorderTab);
        style.BorderSeparatorText = Float("BorderSeparatorText", "SeparatorText", filter, style.BorderSeparatorText);
    }
    static private void RoundingEditor(string filter, Style style)
    {
        if (!Match(filter,
            "Window",
            "Child",
            "Popup",
            "Frame",
            "Scrollbar",
            "Grab",
            "Tab"
            ))
        {
            return;
        }

        ImGui.SeparatorText("Rounding");
        style.RoundingWindow = Float("RoundingWindow", "Window", filter, style.RoundingWindow);
        style.RoundingChild = Float("RoundingChild", "Child", filter, style.RoundingChild);
        style.RoundingPopup = Float("RoundingPopup", "Popup", filter, style.RoundingPopup);
        style.RoundingFrame = Float("RoundingFrame", "Frame", filter, style.RoundingFrame);
        style.RoundingScrollbar = Float("RoundingScrollbar", "Scrollbar", filter, style.RoundingScrollbar);
        style.RoundingGrab = Float("RoundingGrab", "Grab", filter, style.RoundingGrab);
        style.RoundingTab = Float("RoundingTab", "Tab", filter, style.RoundingTab);
    }
    static private void SizesEditor(string filter, Style style)
    {
        if (!Match(filter,
            "WindowMin",
            "Scrollbar",
            "GrabMin"
            ))
        {
            return;
        }

        ImGui.SeparatorText("Sizes");
        style.SizeWindowMin = Pair("SizeWindowMin", "WindowMin", filter, style.SizeWindowMin);
        style.SizeScrollbar = Float("SizeScrollbar", "Scrollbar", filter, style.SizeScrollbar);
        style.SizeGrabMin = Float("SizeGrabMin", "GrabMin", filter, style.SizeGrabMin);
    }
    static private void AlignEditor(string filter, Style style)
    {
        if (!Match(filter,
            "WindowTitle",
            "ButtonText",
            "SelectableText",
            "SeparatorText"
            ))
        {
            return;
        }

        ImGui.SeparatorText("Align");
        style.AlignWindowTitle = Pair("AlignWindowTitle", "WindowTitle", filter, style.AlignWindowTitle);
        style.AlignButtonText = Pair("AlignButtonText", "ButtonText", filter, style.AlignButtonText);
        style.AlignSelectableText = Pair("AlignSelectableText", "SelectableText", filter, style.AlignSelectableText);
        style.AlignSeparatorText = Pair("AlignSeparatorText", "SeparatorText", filter, style.AlignSeparatorText);
    }
    static private void RenderingEditor(string filter, Style style)
    {
        if (!Match(filter,
            "Alpha",
            "DisabledAlpha",
            "AntiAliasedLines",
            "AntiAliasedLinesUseTex",
            "AntiAliasedFill",
            "CurveTessellationTol",
            "CircleTessellationMaxError"
            ))
        {
            return;
        }

        ImGui.SeparatorText("Rendering");
        style.Alpha = Float("Alpha", "Alpha", filter, style.Alpha);
        style.DisabledAlpha = Float("DisabledAlpha", "DisabledAlpha", filter, style.DisabledAlpha);
        style.AntiAliasedLines = Boolean("AntiAliasedLines", "AntiAliasedLines", filter, style.AntiAliasedLines);
        style.AntiAliasedLinesUseTex = Boolean("AntiAliasedLinesUseTex", "AntiAliasedLinesUseTex", filter, style.AntiAliasedLinesUseTex);
        style.AntiAliasedFill = Boolean("AntiAliasedFill", "AntiAliasedFill", filter, style.AntiAliasedFill);
        style.CurveTessellationTol = Float("CurveTessellationTol", "CurveTessellationTol", filter, style.CurveTessellationTol);
        style.CircleTessellationMaxError = Float("CircleTessellationMaxError", "CircleTessellationMaxError", filter, style.CircleTessellationMaxError);
    }
    static private void HoverEditor(string filter, Style style)
    {
        if (!Match(filter,
            "StationaryDelay",
            "DelayShort",
            "DelayNormal",
            "FlagsForTooltipMouse",
            "FlagsForTooltipNav"
            ))
        {
            return;
        }

        ImGui.SeparatorText("Hover");
        style.HoverStationaryDelay = Float("HoverStationaryDelay", "StationaryDelay", filter, style.HoverStationaryDelay);
        style.HoverDelayShort = Float("HoverDelayShort", "DelayShort", filter, style.HoverDelayShort);
        style.HoverDelayNormal = Float("HoverDelayNormal", "DelayNormal", filter, style.HoverDelayNormal);
        style.HoverFlagsForTooltipMouse = HoveredFlag("HoverFlagsForTooltipMouse", "FlagsForTooltipMouse", filter, style.HoverFlagsForTooltipMouse);
        style.HoverFlagsForTooltipNav = HoveredFlag("HoverFlagsForTooltipNav", "FlagsForTooltipNav", filter, style.HoverFlagsForTooltipNav);
    }
    static private void OtherEditor(string filter, Style style)
    {
        if (!Match(filter,
            "MenuButtonPosition",
            "ButtonPosition",
            "SliderDeadzone",
            "MinWidthForCloseButton",
            "CursorScale"
            ))
        {
            return;
        }

        ImGui.SeparatorText("Other");
        style.WindowMenuButtonPosition = Direction("WindowMenuButtonPosition", "WindowMenuButtonPosition", filter, style.WindowMenuButtonPosition);
        style.ColorButtonPosition = Direction("ColorButtonPosition", "ColorButtonPosition", filter, style.ColorButtonPosition);
        style.LogSliderDeadzone = Float("LogSliderDeadzone", "LogSliderDeadzone", filter, style.LogSliderDeadzone);
        style.TabMinWidthForCloseButton = Float("TabMinWidthForCloseButton", "TabMinWidthForCloseButton", filter, style.TabMinWidthForCloseButton);
        style.MouseCursorScale = Float("MouseCursorScale", "MouseCursorScale", filter, style.MouseCursorScale);
    }

    // COLORS
    static private void BackgroundColorsEditor(string filter, Style style)
    {
        if (!Match(filter,
            "BackgroundWindow",
            "BackgroundChild",
            "BackgroundPopup",
            "BackgroundFrame",
            "BackgroundFrameHovered",
            "BackgroundFrameActive",
            "BackgroundTitle",
            "BackgroundTitleActive",
            "BackgroundTitleCollapsed",
            "BackgroundMenuBar",
            "BackgroundScrollbar",
            "BackgroundDockingEmpty",
            "BackgroundTableHeader",
            "BackgroundTableRow",
            "BackgroundTableRowAlt",
            "BackgroundTextSelected",
            "BackgroundNavWindowingDim",
            "BackgroundModalWindowDim"
            ))
        {
            return;
        }

        ImGui.SeparatorText("Backgrounds");
        style.ColorBackgroundWindow = Color("BackgroundWindow", "Window", filter, style.ColorBackgroundWindow);
        style.ColorBackgroundChild = Color("BackgroundChild", "Child", filter, style.ColorBackgroundChild);
        style.ColorBackgroundPopup = Color("BackgroundPopup", "Popup", filter, style.ColorBackgroundPopup);
        style.ColorBackgroundFrame = Color("BackgroundFrame", "Frame", filter, style.ColorBackgroundFrame);
        style.ColorBackgroundFrameHovered = Color("BackgroundFrameHovered", "FrameHovered", filter, style.ColorBackgroundFrameHovered);
        style.ColorBackgroundFrameActive = Color("BackgroundFrameActive", "FrameActive", filter, style.ColorBackgroundFrameActive);
        style.ColorBackgroundTitle = Color("BackgroundTitle", "Title", filter, style.ColorBackgroundTitle);
        style.ColorBackgroundTitleActive = Color("BackgroundTitleActive", "TitleActive", filter, style.ColorBackgroundTitleActive);
        style.ColorBackgroundTitleCollapsed = Color("BackgroundTitleCollapsed", "TitleCollapsed", filter, style.ColorBackgroundTitleCollapsed);
        style.ColorBackgroundMenuBar = Color("BackgroundMenuBar", "MenuBar", filter, style.ColorBackgroundMenuBar);
        style.ColorBackgroundScrollbar = Color("BackgroundScrollbar", "Scrollbar", filter, style.ColorBackgroundScrollbar);
        style.ColorBackgroundDockingEmpty = Color("BackgroundDockingEmpty", "DockingEmpty", filter, style.ColorBackgroundDockingEmpty);
        style.ColorBackgroundTableHeader = Color("BackgroundTableHeader", "TableHeader", filter, style.ColorBackgroundTableHeader);
        style.ColorBackgroundTableRow = Color("BackgroundTableRow", "TableRow", filter, style.ColorBackgroundTableRow);
        style.ColorBackgroundTableRowAlt = Color("BackgroundTableRowAlt", "TableRowAlt", filter, style.ColorBackgroundTableRowAlt);
        style.ColorBackgroundTextSelected = Color("BackgroundTextSelected", "TextSelected", filter, style.ColorBackgroundTextSelected);
        style.ColorBackgroundNavWindowingDim = Color("BackgroundNavWindowingDim", "NavWindowingDim", filter, style.ColorBackgroundNavWindowingDim);
        style.ColorBackgroundModalWindowDim = Color("BackgroundModalWindowDim", "ModalWindowDim", filter, style.ColorBackgroundModalWindowDim);
    }
    static private void TextEditor(string filter, Style style)
    {
        if (!Match(filter,
            "Text",
            "TextDisabled"
            ))
        {
            return;
        }

        ImGui.SeparatorText("Text");
        style.ColorText = Color("Text", "Text", filter, style.ColorText);
        style.ColorTextDisabled = Color("TextDisabled", "TextDisabled", filter, style.ColorTextDisabled);
        style.ColorBackgroundTextSelected = Color("BackgroundTextSelected", "TextSelected", filter, style.ColorBackgroundTextSelected);
    }
    static private void BordersEditor(string filter, Style style)
    {
        if (!Match(filter,
            "Border",
            "BorderShadow",
            "TableBorderStrong",
            "TableBorderLight"
            ))
        {
            return;
        }

        ImGui.SeparatorText("Borders");
        style.ColorBorder = Color("Border", "Border", filter, style.ColorBorder);
        style.ColorBorderShadow = Color("BorderShadow", "BorderShadow", filter, style.ColorBorderShadow);
        style.ColorTableBorderStrong = Color("TableBorderStrong", "TableBorderStrong", filter, style.ColorTableBorderStrong);
        style.ColorTableBorderLight = Color("TableBorderLight", "TableBorderLight", filter, style.ColorTableBorderLight);
    }
    static private void ScrollEditor(string filter, Style style)
    {
        if (!Match(filter,
            "ScrollbarGrab",
            "ScrollbarGrabHovered",
            "ScrollbarGrabActive",
            "SliderGrab",
            "SliderGrabActive"
            ))
        {
            return;
        }

        ImGui.SeparatorText("Scrollbars & sliders");
        style.ColorBackgroundScrollbar = Color("BackgroundScrollbar", "ScrollbarBackground", filter, style.ColorBackgroundScrollbar);
        style.ColorScrollbarGrab = Color("ScrollbarGrab", "ScrollbarGrab", filter, style.ColorScrollbarGrab);
        style.ColorScrollbarGrabHovered = Color("ScrollbarGrabHovered", "ScrollbarGrabHovered", filter, style.ColorScrollbarGrabHovered);
        style.ColorScrollbarGrabActive = Color("ScrollbarGrabActive", "ScrollbarGrabActive", filter, style.ColorScrollbarGrabActive);
        style.ColorSliderGrab = Color("SliderGrab", "SliderGrab", filter, style.ColorSliderGrab);
        style.ColorSliderGrabActive = Color("SliderGrabActive", "SliderGrabActive", filter, style.ColorSliderGrabActive);
    }
    static private void ButtonsEditor(string filter, Style style)
    {
        if (!Match(filter,
            "Button",
            "ButtonHovered",
            "ButtonActive"
            ))
        {
            return;
        }

        ImGui.SeparatorText("Buttons");
        style.ColorButton = Color("Button", "Button", filter, style.ColorButton);
        style.ColorButtonHovered = Color("ButtonHovered", "ButtonHovered", filter, style.ColorButtonHovered);
        style.ColorButtonActive = Color("ButtonActive", "ButtonActive", filter, style.ColorButtonActive);
    }
    static private void HeadersEditor(string filter, Style style)
    {
        if (!Match(filter,
            "Header",
            "HeaderHovered",
            "HeaderActive"
            ))
        {
            return;
        }

        ImGui.SeparatorText("Headers");
        style.ColorHeader = Color("Header", "Header", filter, style.ColorHeader);
        style.ColorHeaderHovered = Color("HeaderHovered", "HeaderHovered", filter, style.ColorHeaderHovered);
        style.ColorHeaderActive = Color("HeaderActive", "HeaderActive", filter, style.ColorHeaderActive);
    }
    static private void SeparatorEditor(string filter, Style style)
    {
        if (!Match(filter,
            "Separator",
            "SeparatorHovered",
            "SeparatorActive"
            ))
        {
            return;
        }

        ImGui.SeparatorText("Separators");
        style.ColorSeparator = Color("Separator", "Separator", filter, style.ColorSeparator);
        style.ColorSeparatorHovered = Color("SeparatorHovered", "SeparatorHovered", filter, style.ColorSeparatorHovered);
        style.ColorSeparatorActive = Color("SeparatorActive", "SeparatorActive", filter, style.ColorSeparatorActive);
    }
    static private void ResizeEditor(string filter, Style style)
    {
        if (!Match(filter,
            "ResizeGrip",
            "ResizeGripHovered",
            "ResizeGripActive"
            ))
        {
            return;
        }

        ImGui.SeparatorText("Resize");
        style.ColorResizeGrip = Color("ResizeGrip", "ResizeGrip", filter, style.ColorResizeGrip);
        style.ColorResizeGripHovered = Color("ResizeGripHovered", "ResizeGripHovered", filter, style.ColorResizeGripHovered);
        style.ColorResizeGripActive = Color("ResizeGripActive", "ResizeGripActive", filter, style.ColorResizeGripActive);
    }
    static private void TabsEditor(string filter, Style style)
    {
        if (!Match(filter,
            "Tab",
            "TabHovered",
            "TabActive",
            "TabUnfocused",
            "TabUnfocusedActive"
            ))
        {
            return;
        }

        ImGui.SeparatorText("Tabs");
        style.ColorTab = Color("Tab", "Tab", filter, style.ColorTab);
        style.ColorTabHovered = Color("TabHovered", "TabHovered", filter, style.ColorTabHovered);
        style.ColorTabActive = Color("TabActive", "TabActive", filter, style.ColorTabActive);
        style.ColorTabUnfocused = Color("TabUnfocused", "TabUnfocused", filter, style.ColorTabUnfocused);
        style.ColorTabUnfocusedActive = Color("TabUnfocusedActive", "TabUnfocusedActive", filter, style.ColorTabUnfocusedActive);
    }
    static private void PlotsEditor(string filter, Style style)
    {
        if (!Match(filter,
            "PlotLines",
            "PlotLinesHovered",
            "PlotHistogram",
            "PlotHistogramHovered"
            ))
        {
            return;
        }

        ImGui.SeparatorText("Plots");
        style.ColorPlotLines = Color("PlotLines", "PlotLines", filter, style.ColorPlotLines);
        style.ColorPlotLinesHovered = Color("PlotLinesHovered", "PlotLinesHovered", filter, style.ColorPlotLinesHovered);
        style.ColorPlotHistogram = Color("PlotHistogram", "PlotHistogram", filter, style.ColorPlotHistogram);
        style.ColorPlotHistogramHovered = Color("PlotHistogramHovered", "PlotHistogramHovered", filter, style.ColorPlotHistogramHovered);
    }
    static private void OtherColorsEditor(string filter, Style style)
    {
        if (!Match(filter,
            "CheckMark",
            "DockingPreview",
            "DragDropTarget",
            "NavHighlight",
            "NavWindowingHighlight"
            ))
        {
            return;
        }

        ImGui.SeparatorText("Other");
        style.ColorCheckMark = Color("CheckMark", "CheckMark", filter, style.ColorCheckMark);
        style.ColorDockingPreview = Color("DockingPreview", "DockingPreview", filter, style.ColorDockingPreview);
        style.ColorDragDropTarget = Color("DragDropTarget", "DragDropTarget", filter, style.ColorDragDropTarget);
        style.ColorNavHighlight = Color("NavHighlight", "NavHighlight", filter, style.ColorNavHighlight);
        style.ColorNavWindowingHighlight = Color("NavWindowingHighlight", "NavWindowingHighlight", filter, style.ColorNavWindowingHighlight);
    }

    static private string Title(string title) => $"{title}##{sIdCounter++}";

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

    static public readonly Dictionary<string, string> Hints = new()
    {
        {  "BackgroundWindow", "TEST" }
    };
    static private Value4 Color(string id, string name, string filter, Value4 value)
    {
        if (filter == "" || Match(filter, id)) return Editors.Color(Title(name), value, Hints.ContainsKey(id) ? Hints[id] : null);
        return value;
    }
    static private Value2 Pair(string id, string name, string filter, Value2 value)
    {
        if (filter == "" || Match(filter, id)) return Editors.Pair(Title(name), value, Hints.ContainsKey(id) ? Hints[id] : null);
        return value;
    }
    static private float Float(string id, string name, string filter, float value)
    {
        if (filter == "" || Match(filter, id)) return Editors.Float(Title(name), value, Hints.ContainsKey(id) ? Hints[id] : null);
        return value;
    }
    static private int Integer(string id, string name, string filter, int value)
    {
        if (filter == "" || Match(filter, id)) return Editors.Integer(Title(name), value, Hints.ContainsKey(id) ? Hints[id] : null);
        return value;
    }
    static private bool Boolean(string id, string name, string filter, bool value)
    {
        if (filter == "" || Match(filter, id)) return Editors.Boolean(Title(name), value, Hints.ContainsKey(id) ? Hints[id] : null);
        return value;
    }
    static private ImGuiDir Direction(string id, string name, string filter, ImGuiDir value)
    {
        if (filter == "" || Match(filter, id)) return Editors.Direction(Title(name), value, Hints.ContainsKey(id) ? Hints[id] : null);
        return value;
    }
    static private ImGuiHoveredFlags HoveredFlag(string id, string name, string filter, ImGuiHoveredFlags value)
    {
        if (filter == "" || Match(filter, id)) return Editors.HoveredFlag(Title(name), value);
        return value;
    }
}

static public class Editors
{
    static public Value4 Color(string name, Vector4 value, string hint = null)
    {
        if (hint == null)
        {
            ImGui.ColorEdit4(name, ref value, ImGuiColorEditFlags.AlphaPreviewHalf | ImGuiColorEditFlags.AlphaBar);
        }
        else
        {
            Vector2 spacing = ImGui.GetStyle().ItemSpacing;
            spacing.X = 1;
            ImGui.ColorEdit4($"##{name}", ref value, ImGuiColorEditFlags.AlphaPreviewHalf | ImGuiColorEditFlags.AlphaBar);
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, spacing);
            ImGui.SameLine();
            DrawHint(hint);
            ImGui.SameLine();
            ImGui.Text(name.Split("##", 2)[0]);
            ImGui.PopStyleVar();
        }

        return value;
    }
    static public Value2 Pair(string name, Value2 value, string hint = null)
    {
        Vector2 pair = value;
        if (hint == null)
        {
            ImGui.DragFloat2(name, ref pair);
        }
        else
        {
            Vector2 spacing = ImGui.GetStyle().ItemSpacing;
            spacing.X = 1;
            ImGui.DragFloat2($"##{name}", ref pair);
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, spacing);
            ImGui.SameLine();
            DrawHint(hint);
            ImGui.SameLine();
            ImGui.Text(name.Split("##", 2)[0]);
            ImGui.PopStyleVar();
        }

        return pair;
    }
    static public float Float(string name, float value, string hint = null)
    {
        if (hint == null)
        {
            ImGui.DragFloat(name, ref value);
        }
        else
        {
            Vector2 spacing = ImGui.GetStyle().ItemSpacing;
            spacing.X = 1;
            ImGui.DragFloat($"##{name}", ref value);
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, spacing);
            ImGui.SameLine();
            DrawHint(hint);
            ImGui.SameLine();
            ImGui.Text(name.Split("##", 2)[0]);
            ImGui.PopStyleVar();
        }

        return value;
    }
    static public int Integer(string name, int value, string hint = null)
    {
        if (hint == null)
        {
            ImGui.DragInt(name, ref value);
        }
        else
        {
            Vector2 spacing = ImGui.GetStyle().ItemSpacing;
            spacing.X = 1;
            ImGui.DragInt($"##{name}", ref value);
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, spacing);
            ImGui.SameLine();
            DrawHint(hint);
            ImGui.SameLine();
            ImGui.Text(name.Split("##", 2)[0]);
            ImGui.PopStyleVar();
        }

        return value;
    }
    static public bool Boolean(string name, bool value, string hint = null)
    {
        if (hint == null)
        {
            ImGui.Checkbox(name, ref value);
        }
        else
        {
            Vector2 spacing = ImGui.GetStyle().ItemSpacing;
            spacing.X = 1;
            ImGui.Checkbox($"##{name}", ref value);
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, spacing);
            ImGui.SameLine();
            DrawHint(hint);
            ImGui.SameLine();
            ImGui.Text(name.Split("##", 2)[0]);
            ImGui.PopStyleVar();
        }

        return value;
    }
    static public readonly string[] Directions = new string[]
    {
        "NONE",
        "Left",
        "Right",
        "Up",
        "Down"
    };
    static public ImGuiDir Direction(string name, ImGuiDir value, string hint = null)
    {
        int dir = (int)value;
        if (hint == null)
        {
            ImGui.Combo(name, ref dir, Directions, Directions.Length);
        }
        else
        {
            Vector2 spacing = ImGui.GetStyle().ItemSpacing;
            spacing.X = 1;
            ImGui.Combo($"##{name}", ref dir, Directions, Directions.Length);
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, spacing);
            ImGui.SameLine();
            DrawHint(hint);
            ImGui.SameLine();
            ImGui.Text(name.Split("##", 2)[0]);
            ImGui.PopStyleVar();
        }

        return (ImGuiDir)dir;
    }
    static public readonly string[] HoveredFlags = new string[]
    {
        //"None",
        "ChildWindows",
        "RootWindow",
        "AnyWindow",
        "NoPopupHierarchy",
        "DockHierarchy",
        "AllowWhenBlockedByPopup",
        "AllowWhenBlockedByActiveItem",
        "AllowWhenOverlappedByItem",
        "AllowWhenOverlappedByWindow",
        "AllowWhenDisabled",
        "NoNavOverride",
        "AllowWhenOverlapped",
        "RectOnly",
        "RootAndChildWindows",
        "ForTooltip",
        "Stationary",
        "DelayNone",
        "DelayShort",
        "DelayNormal",
        "NoSharedDelay"
    };
    static public readonly int[] HoveredFlagsValues = new int[]
    {
        //0x0,
        0x1,
        0x2,
        0x4,
        0x8,
        0x10,
        0x20,
        0x80,
        0x100,
        0x200,
        0x400,
        0x800,
        0x300,
        0x3A0,
        0x3,
        0x800,
        0x1000,
        0x2000,
        0x4000,
        0x8000,
        0x10000
    };
    static public ImGuiHoveredFlags HoveredFlag(string name, ImGuiHoveredFlags value)
    {
        int dir = (int)value;

        if (ImGui.TreeNode(name))
        {
            ImGui.Indent();
            for (int index = 0; index < HoveredFlagsValues.Length; index++)
            {
                ImGui.CheckboxFlags($"{HoveredFlags[index]}##{name}", ref dir, HoveredFlagsValues[index]);
            }
            ImGui.Unindent();
            ImGui.TreePop();
        }

        return (ImGuiHoveredFlags)dir;
    }
    static public void DrawHint(string hint)
    {
        ImGui.SameLine();
        ImGui.TextDisabled("(?)");
        if (ImGui.BeginItemTooltip())
        {
            ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35f);
            ImGui.TextUnformatted(hint);
            ImGui.PopTextWrapPos();

            ImGui.EndTooltip();
        }
    }
}
