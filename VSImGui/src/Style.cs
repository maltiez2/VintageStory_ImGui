using ImGuiNET;
using System;
using System.Linq;
using System.Numerics;
using Vintagestory.API.MathTools;

namespace VSImGui.ImGuiUtils
{
    public struct Value2
    {
        public Vector2 Value { get; set; }
        public float X => Value.X;
        public float Y => Value.Y;


        public Value2(Vector2 value) => Value = value;
        public Value2((float first, float second) values) => Value = new(values.first, values.second);
        public Value2(Vec2f value) => Value = new(value.X, value.Y);
        public Value2(float first, float second) => Value = new(first, second);

        public static implicit operator Value2(Vector2 value) => new(value);
        public static implicit operator Value2((float first, float second) values) => new(values);
        public static implicit operator Value2(Vec2f value) => new(value);
        public static implicit operator Vector2(Value2 value) => new(value.X, value.Y);
    }
    public struct Value3
    {
        public Vector3 Value { get; set; }
        public float X => Value.X;
        public float Y => Value.Y;
        public float Z => Value.Z;
        public float R => Value.X;
        public float G => Value.Y;
        public float B => Value.Z;

        public Value3(Vector3 value) => Value = value;
        public Value3((float first, float second, float third) values) => Value = new(values.first, values.second, values.third);
        public Value3(Vec3f value) => Value = new(value.X, value.Y, value.Z);
        public Value3(float first, float second, float third) => Value = new(first, second, third);

        public static implicit operator Value3(Vector3 value) => new(value);
        public static implicit operator Value3((float first, float second, float third) values) => new(values);
        public static implicit operator Value3(Vec3f value) => new(value);
        public static implicit operator Vector3(Value3 value) => new(value.X, value.Y, value.Z);
    }
    public struct Value4
    {
        public Vector4 Value { get; set; }
        public float X => Value.X;
        public float Y => Value.Y;
        public float Z => Value.Z;
        public float W => Value.W;
        public float R => Value.X;
        public float G => Value.Y;
        public float B => Value.Z;
        public float A => Value.W;
        public Value3 RGB => new(R, G, B);

        public Value4(Vector4 value) => Value = value;
        public Value4((float first, float second, float third, float fourth) values) => Value = new(values.first, values.second, values.third, values.fourth);
        public Value4(Vec4f value) => Value = new(value.X, value.Y, value.Z, value.W);
        public Value4(float first, float second, float third, float fourth) => Value = new(first, second, third, fourth);
        public Value4((Value3 RGB, float alpha) values) => Value = new(values.RGB.X, values.RGB.Y, values.RGB.Z, values.alpha);

        public static implicit operator Value4(Vector4 value) => new(value);
        public static implicit operator Value4((float first, float second, float third, float fourth) values) => new(values);
        public static implicit operator Value4((Value3 RGB, float alpha) values) => new(values);
        public static implicit operator Value4(Vec4f value) => new(value);
        public static implicit operator Vector4(Value4 value) => new(value.X, value.Y, value.Z, value.W);
    }
    public struct Style
    {
        public readonly ImGuiStyle NativeStyle => mNativeStyle;

        // PADDINGS
        public Value2 PaddingWindow { readonly get => mNativeStyle.WindowPadding; set => mNativeStyle.WindowPadding = value.Value; }
        public Value2 PaddingFrame { readonly get => mNativeStyle.FramePadding; set => mNativeStyle.FramePadding = value.Value; }
        public Value2 PaddingCell { readonly get => mNativeStyle.CellPadding; set => mNativeStyle.CellPadding = value.Value; }
        public Value2 PaddingTouchExtra { readonly get => mNativeStyle.TouchExtraPadding; set => mNativeStyle.TouchExtraPadding = value.Value; }
        public Value2 PaddingSeparatorText { readonly get => mNativeStyle.SeparatorTextPadding; set => mNativeStyle.SeparatorTextPadding = value.Value; }
        public Value2 PaddingDisplayWindow { readonly get => mNativeStyle.DisplayWindowPadding; set => mNativeStyle.DisplayWindowPadding = value.Value; }
        public Value2 PaddingDisplaySafeArea { readonly get => mNativeStyle.DisplaySafeAreaPadding; set => mNativeStyle.DisplaySafeAreaPadding = value.Value; }

        // SPACING
        public Value2 SpacingItem { readonly get => mNativeStyle.ItemSpacing; set => mNativeStyle.ItemSpacing = value.Value; }
        public Value2 SpacingItemInner { readonly get => mNativeStyle.ItemInnerSpacing; set => mNativeStyle.ItemInnerSpacing = value.Value; }
        public float SpacingIndent { readonly get => mNativeStyle.IndentSpacing; set => mNativeStyle.IndentSpacing = value; }
        public float SpacingColumnsMin { readonly get => mNativeStyle.ColumnsMinSpacing; set => mNativeStyle.ColumnsMinSpacing = value; }

        // BORDERS
        public float BorderWindow { readonly get => mNativeStyle.WindowBorderSize; set => mNativeStyle.WindowBorderSize = value; }
        public float BorderChild { readonly get => mNativeStyle.ChildBorderSize; set => mNativeStyle.ChildBorderSize = value; }
        public float BorderPopup { readonly get => mNativeStyle.PopupBorderSize; set => mNativeStyle.PopupBorderSize = value; }
        public float BorderFrame { readonly get => mNativeStyle.FrameBorderSize; set => mNativeStyle.FrameBorderSize = value; }
        public float BorderTab { readonly get => mNativeStyle.TabBorderSize; set => mNativeStyle.TabBorderSize = value; }
        public float BorderSeparatorText { readonly get => mNativeStyle.SeparatorTextBorderSize; set => mNativeStyle.SeparatorTextBorderSize = value; }

        // ROUNDING
        public float RoundingWindow { readonly get => mNativeStyle.WindowRounding; set => mNativeStyle.WindowRounding = value; }
        public float RoundingChild { readonly get => mNativeStyle.ChildRounding; set => mNativeStyle.ChildRounding = value; }
        public float RoundingPopup { readonly get => mNativeStyle.PopupRounding; set => mNativeStyle.PopupRounding = value; }
        public float RoundingFrame { readonly get => mNativeStyle.FrameRounding; set => mNativeStyle.FrameRounding = value; }
        public float RoundingScrollbar { readonly get => mNativeStyle.ScrollbarRounding; set => mNativeStyle.ScrollbarRounding = value; }
        public float RoundingGrab { readonly get => mNativeStyle.GrabRounding; set => mNativeStyle.GrabRounding = value; }
        public float RoundingTab { readonly get => mNativeStyle.TabRounding; set => mNativeStyle.TabRounding = value; }

        // SIZES
        public Value2 SizeWindowMin { readonly get => mNativeStyle.WindowMinSize; set => mNativeStyle.WindowMinSize = value.Value; }
        public float SizeScrollbar { readonly get => mNativeStyle.ScrollbarSize; set => mNativeStyle.ScrollbarSize = value; }
        public float SizeGrabMin { readonly get => mNativeStyle.GrabMinSize; set => mNativeStyle.GrabMinSize = value; }

        // ALIGN
        public Value2 AlignWindowTitle { readonly get => mNativeStyle.WindowTitleAlign; set => mNativeStyle.WindowTitleAlign = value; }
        public Value2 AlignButtonText { readonly get => mNativeStyle.ButtonTextAlign; set => mNativeStyle.ButtonTextAlign = value; }
        public Value2 AlignSelectableText { readonly get => mNativeStyle.SelectableTextAlign; set => mNativeStyle.SelectableTextAlign = value; }
        public Value2 AlignSeparatorText { readonly get => mNativeStyle.SeparatorTextAlign; set => mNativeStyle.SeparatorTextAlign = value; }

        // RENDERING
        public float Alpha { readonly get => mNativeStyle.Alpha; set => mNativeStyle.Alpha = value; }
        public float DisabledAlpha { readonly get => mNativeStyle.DisabledAlpha; set => mNativeStyle.DisabledAlpha = value; }
        public bool AntiAliasedLines { readonly get => BitConverter.ToBoolean(new byte[] { mNativeStyle.AntiAliasedLines }); set => mNativeStyle.AntiAliasedLines = value ? (byte)1 : (byte)0; }
        public bool AntiAliasedLinesUseTex { readonly get => BitConverter.ToBoolean(new byte[] { mNativeStyle.AntiAliasedLinesUseTex }); set => mNativeStyle.AntiAliasedLinesUseTex = value ? (byte)1 : (byte)0; }
        public bool AntiAliasedFill { readonly get => BitConverter.ToBoolean(new byte[] { mNativeStyle.AntiAliasedFill }); set => mNativeStyle.AntiAliasedFill = value ? (byte)1 : (byte)0; }
        public float CurveTessellationTol { readonly get => mNativeStyle.CurveTessellationTol; set => mNativeStyle.CurveTessellationTol = value; }
        public float CircleTessellationMaxError { readonly get => mNativeStyle.CircleTessellationMaxError; set => mNativeStyle.CircleTessellationMaxError = value; }

        // HOVER
        public float HoverStationaryDelay { readonly get => mNativeStyle.HoverStationaryDelay; set => mNativeStyle.HoverStationaryDelay = value; }
        public float HoverDelayShort { readonly get => mNativeStyle.HoverDelayShort; set => mNativeStyle.HoverDelayShort = value; }
        public float HoverDelayNormal { readonly get => mNativeStyle.HoverDelayNormal; set => mNativeStyle.HoverDelayNormal = value; }
        public ImGuiHoveredFlags HoverFlagsForTooltipMouse { readonly get => mNativeStyle.HoverFlagsForTooltipMouse; set => mNativeStyle.HoverFlagsForTooltipMouse = value; }
        public ImGuiHoveredFlags HoverFlagsForTooltipNav { readonly get => mNativeStyle.HoverFlagsForTooltipNav; set => mNativeStyle.HoverFlagsForTooltipNav = value; }

        // OTHER
        public ImGuiDir WindowMenuButtonPosition { readonly get => mNativeStyle.WindowMenuButtonPosition; set => mNativeStyle.WindowMenuButtonPosition = value; }
        public ImGuiDir ColorButtonPosition { readonly get => mNativeStyle.ColorButtonPosition; set => mNativeStyle.ColorButtonPosition = value; }
        public float LogSliderDeadzone { readonly get => mNativeStyle.LogSliderDeadzone; set => mNativeStyle.LogSliderDeadzone = value; }
        public float TabMinWidthForCloseButton { readonly get => mNativeStyle.TabMinWidthForCloseButton; set => mNativeStyle.TabMinWidthForCloseButton = value; }
        public float MouseCursorScale { readonly get => mNativeStyle.MouseCursorScale; set => mNativeStyle.MouseCursorScale = value; }

        // COLORS
        // COLORS: BACKGROUND
        public Value4 ColorBackgroundWindow { readonly get => mNativeStyle.Colors_2; set => mNativeStyle.Colors_2 = value; }
        public Value4 ColorBackgroundChild { readonly get => mNativeStyle.Colors_3; set => mNativeStyle.Colors_3 = value; }
        public Value4 ColorBackgroundPopup { readonly get => mNativeStyle.Colors_4; set => mNativeStyle.Colors_4 = value; }
        public Value4 ColorBackgroundFrame { readonly get => mNativeStyle.Colors_7; set => mNativeStyle.Colors_7 = value; }
        public Value4 ColorBackgroundFrameHovered { readonly get => mNativeStyle.Colors_8; set => mNativeStyle.Colors_8 = value; }
        public Value4 ColorBackgroundFrameActive { readonly get => mNativeStyle.Colors_9; set => mNativeStyle.Colors_9 = value; }
        public Value4 ColorBackgroundTitle { readonly get => mNativeStyle.Colors_10; set => mNativeStyle.Colors_10 = value; }
        public Value4 ColorBackgroundTitleActive { readonly get => mNativeStyle.Colors_11; set => mNativeStyle.Colors_11 = value; }
        public Value4 ColorBackgroundTitleCollapsed { readonly get => mNativeStyle.Colors_12; set => mNativeStyle.Colors_12 = value; }
        public Value4 ColorBackgroundMenuBar { readonly get => mNativeStyle.Colors_13; set => mNativeStyle.Colors_13 = value; }
        public Value4 ColorBackgroundScrollbar { readonly get => mNativeStyle.Colors_14; set => mNativeStyle.Colors_14 = value; }
        public Value4 ColorBackgroundDockingEmpty { readonly get => mNativeStyle.Colors_39; set => mNativeStyle.Colors_39 = value; }
        public Value4 ColorBackgroundTableHeader { readonly get => mNativeStyle.Colors_44; set => mNativeStyle.Colors_44 = value; }
        public Value4 ColorBackgroundTableRow { readonly get => mNativeStyle.Colors_47; set => mNativeStyle.Colors_47 = value; }
        public Value4 ColorBackgroundTableRowAlt { readonly get => mNativeStyle.Colors_48; set => mNativeStyle.Colors_48 = value; }
        public Value4 ColorBackgroundTextSelected { readonly get => mNativeStyle.Colors_49; set => mNativeStyle.Colors_49 = value; }
        public Value4 ColorBackgroundNavWindowingDim { readonly get => mNativeStyle.Colors_53; set => mNativeStyle.Colors_53 = value; }
        public Value4 ColorBackgroundModalWindowDim { readonly get => mNativeStyle.Colors_54; set => mNativeStyle.Colors_54 = value; }


        // COLORS: TEXT
        public Value4 ColorText { readonly get => mNativeStyle.Colors_0; set => mNativeStyle.Colors_0 = value; }
        public Value4 ColorTextDisabled { readonly get => mNativeStyle.Colors_1; set => mNativeStyle.Colors_1 = value; }

        // COLORS: BORDERS
        public Value4 ColorBorder { readonly get => mNativeStyle.Colors_5; set => mNativeStyle.Colors_5 = value; }
        public Value4 ColorBorderShadow { readonly get => mNativeStyle.Colors_6; set => mNativeStyle.Colors_6 = value; }
        public Value4 ColorTableBorderStrong { readonly get => mNativeStyle.Colors_45; set => mNativeStyle.Colors_45 = value; }
        public Value4 ColorTableBorderLight { readonly get => mNativeStyle.Colors_46; set => mNativeStyle.Colors_46 = value; }

        // COLORS: SCROLL
        public Value4 ColorScrollbarGrab { readonly get => mNativeStyle.Colors_15; set => mNativeStyle.Colors_15 = value; }
        public Value4 ColorScrollbarGrabHovered { readonly get => mNativeStyle.Colors_16; set => mNativeStyle.Colors_16 = value; }
        public Value4 ColorScrollbarGrabActive { readonly get => mNativeStyle.Colors_17; set => mNativeStyle.Colors_17 = value; }


        // COLORS: SLIDER
        public Value4 ColorSliderGrab { readonly get => mNativeStyle.Colors_19; set => mNativeStyle.Colors_19 = value; }
        public Value4 ColorSliderGrabActive { readonly get => mNativeStyle.Colors_20; set => mNativeStyle.Colors_20 = value; }

        // COLORS: BUTTON
        public Value4 ColorButton { readonly get => mNativeStyle.Colors_21; set => mNativeStyle.Colors_21 = value; }
        public Value4 ColorButtonHovered { readonly get => mNativeStyle.Colors_22; set => mNativeStyle.Colors_22 = value; }
        public Value4 ColorButtonActive { readonly get => mNativeStyle.Colors_23; set => mNativeStyle.Colors_23 = value; }

        // COLORS: HEADER
        public Value4 ColorHeader { readonly get => mNativeStyle.Colors_24; set => mNativeStyle.Colors_24 = value; }
        public Value4 ColorHeaderHovered { readonly get => mNativeStyle.Colors_25; set => mNativeStyle.Colors_25 = value; }
        public Value4 ColorHeaderActive { readonly get => mNativeStyle.Colors_26; set => mNativeStyle.Colors_26 = value; }

        // COLORS: SEPARATOR
        public Value4 ColorSeparator { readonly get => mNativeStyle.Colors_27; set => mNativeStyle.Colors_27 = value; }
        public Value4 ColorSeparatorHovered { readonly get => mNativeStyle.Colors_28; set => mNativeStyle.Colors_28 = value; }
        public Value4 ColorSeparatorActive { readonly get => mNativeStyle.Colors_29; set => mNativeStyle.Colors_29 = value; }

        // COLORS: RESIZE
        public Value4 ColorResizeGrip { readonly get => mNativeStyle.Colors_30; set => mNativeStyle.Colors_30 = value; }
        public Value4 ColorResizeGripHovered { readonly get => mNativeStyle.Colors_31; set => mNativeStyle.Colors_31 = value; }
        public Value4 ColorResizeGripActive { readonly get => mNativeStyle.Colors_32; set => mNativeStyle.Colors_32 = value; }

        // COLORS: TAB
        public Value4 ColorTab { readonly get => mNativeStyle.Colors_33; set => mNativeStyle.Colors_33 = value; }
        public Value4 ColorTabHovered { readonly get => mNativeStyle.Colors_34; set => mNativeStyle.Colors_34 = value; }
        public Value4 ColorTabActive { readonly get => mNativeStyle.Colors_35; set => mNativeStyle.Colors_35 = value; }
        public Value4 ColorTabUnfocused { readonly get => mNativeStyle.Colors_36; set => mNativeStyle.Colors_36 = value; }
        public Value4 ColorTabUnfocusedActive { readonly get => mNativeStyle.Colors_37; set => mNativeStyle.Colors_37 = value; }

        // COLORS: PLOT
        public Value4 ColorPlotLines { readonly get => mNativeStyle.Colors_40; set => mNativeStyle.Colors_40 = value; }
        public Value4 ColorPlotLinesHovered { readonly get => mNativeStyle.Colors_41; set => mNativeStyle.Colors_41 = value; }
        public Value4 ColorPlotHistogram { readonly get => mNativeStyle.Colors_42; set => mNativeStyle.Colors_42 = value; }
        public Value4 ColorPlotHistogramHovered { readonly get => mNativeStyle.Colors_43; set => mNativeStyle.Colors_43 = value; }

        // COLORS: OTHER
        public Value4 ColorCheckMark { readonly get => mNativeStyle.Colors_18; set => mNativeStyle.Colors_18 = value; }
        public Value4 ColorDockingPreview { readonly get => mNativeStyle.Colors_38; set => mNativeStyle.Colors_38 = value; }
        public Value4 ColorDragDropTarget { readonly get => mNativeStyle.Colors_50; set => mNativeStyle.Colors_50 = value; }
        public Value4 ColorNavHighlight { readonly get => mNativeStyle.Colors_51; set => mNativeStyle.Colors_51 = value; }
        public Value4 ColorNavWindowingHighlight { readonly get => mNativeStyle.Colors_52; set => mNativeStyle.Colors_52 = value; }

        // WINDOW STYLE
        public (string name, int size) Font { get => mWindowStyle.Font; set => mWindowStyle.Font = value; }

        
        
        private ImGuiStyle mNativeStyle;
        private WindowStyle mWindowStyle;

        public Style(ImGuiStyle style) => mNativeStyle = style;

        public void Push() => mWindowStyle.Push();
        public void Pop() => mWindowStyle.Pop();
    }

    public struct WindowStyle
    {
        public (string name, int size) Font
        { 
            get => mFontKey;
            set
            {
                FontLoaded = ImGuiController.LoadedFonts.ContainsKey(value);
                if (FontLoaded)
                {
                    mFontKey = value;
                    mFont = ImGuiController.LoadedFonts[value];
                }
            }
        }
        public bool FontLoaded { get; private set; }

        private ImFontPtr mFont;
        private (string name, int size) mFontKey;

        public void Push()
        {
            ImGui.PushFont(mFont);
        }

        public void Pop()
        {
            ImGui.PopFont();
        }
    }
}
