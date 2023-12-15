using ImGuiNET;
using System;
using System.Numerics;
using Vintagestory.API.MathTools;

namespace VSImGui
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
        public Value2 XY => new(X, Y);

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
        public Value3 XYZ => new(X, Y, Z);

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
    public class Style
    {
        public ImGuiStyle NativeStyle => mNativeStyle;

        // PADDINGS
        public Value2 Paddings
        {
            private get => PaddingWindow;
            set
            {
                PaddingWindow = value;
                PaddingFrame = value;
                PaddingCell = value;
                PaddingTouchExtra = value;
                PaddingSeparatorText = value;
                PaddingDisplayWindow = value;
                PaddingDisplaySafeArea = value;
            }
        }
        public Value2 PaddingWindow { get => mNativeStyle.WindowPadding; set => mNativeStyle.WindowPadding = value.Value; }
        public Value2 PaddingFrame { get => mNativeStyle.FramePadding; set => mNativeStyle.FramePadding = value.Value; }
        public Value2 PaddingCell { get => mNativeStyle.CellPadding; set => mNativeStyle.CellPadding = value.Value; }
        public Value2 PaddingTouchExtra { get => mNativeStyle.TouchExtraPadding; set => mNativeStyle.TouchExtraPadding = value.Value; }
        public Value2 PaddingSeparatorText { get => mNativeStyle.SeparatorTextPadding; set => mNativeStyle.SeparatorTextPadding = value.Value; }
        public Value2 PaddingDisplayWindow { get => mNativeStyle.DisplayWindowPadding; set => mNativeStyle.DisplayWindowPadding = value.Value; }
        public Value2 PaddingDisplaySafeArea { get => mNativeStyle.DisplaySafeAreaPadding; set => mNativeStyle.DisplaySafeAreaPadding = value.Value; }

        // SPACING
        public Value2 SpacingItem { get => mNativeStyle.ItemSpacing; set => mNativeStyle.ItemSpacing = value.Value; }
        public Value2 SpacingItemInner { get => mNativeStyle.ItemInnerSpacing; set => mNativeStyle.ItemInnerSpacing = value.Value; }
        public float SpacingIndent { get => mNativeStyle.IndentSpacing; set => mNativeStyle.IndentSpacing = value; }
        public float SpacingColumnsMin { get => mNativeStyle.ColumnsMinSpacing; set => mNativeStyle.ColumnsMinSpacing = value; }

        // BORDERS
        public float Borders
        {
            private get => BorderWindow;

            set
            {
                BorderWindow = value;
                BorderChild = value;
                BorderPopup = value;
                BorderFrame = value;
                BorderTab = value;
                BorderSeparatorText = value;
            }
        }
        public float BorderWindow { get => mNativeStyle.WindowBorderSize; set => mNativeStyle.WindowBorderSize = value; }
        public float BorderChild { get => mNativeStyle.ChildBorderSize; set => mNativeStyle.ChildBorderSize = value; }
        public float BorderPopup { get => mNativeStyle.PopupBorderSize; set => mNativeStyle.PopupBorderSize = value; }
        public float BorderFrame { get => mNativeStyle.FrameBorderSize; set => mNativeStyle.FrameBorderSize = value; }
        public float BorderTab { get => mNativeStyle.TabBorderSize; set => mNativeStyle.TabBorderSize = value; }
        public float BorderSeparatorText { get => mNativeStyle.SeparatorTextBorderSize; set => mNativeStyle.SeparatorTextBorderSize = value; }

        // ROUNDING
        public float Rounding
        {
            private get => RoundingWindow;
            
            set
            {
                RoundingWindow = value;
                RoundingChild = value;
                RoundingPopup = value;
                RoundingFrame = value;
                RoundingScrollbar = value;
                RoundingGrab = value;
                RoundingTab = value;
            }
        }
        public float RoundingWindow { get => mNativeStyle.WindowRounding; set => mNativeStyle.WindowRounding = value; }
        public float RoundingChild { get => mNativeStyle.ChildRounding; set => mNativeStyle.ChildRounding = value; }
        public float RoundingPopup { get => mNativeStyle.PopupRounding; set => mNativeStyle.PopupRounding = value; }
        public float RoundingFrame { get => mNativeStyle.FrameRounding; set => mNativeStyle.FrameRounding = value; }
        public float RoundingScrollbar { get => mNativeStyle.ScrollbarRounding; set => mNativeStyle.ScrollbarRounding = value; }
        public float RoundingGrab { get => mNativeStyle.GrabRounding; set => mNativeStyle.GrabRounding = value; }
        public float RoundingTab { get => mNativeStyle.TabRounding; set => mNativeStyle.TabRounding = value; }

        // SIZES
        public Value2 SizeWindowMin { get => mNativeStyle.WindowMinSize; set => mNativeStyle.WindowMinSize = value.Value; }
        public float SizeScrollbar { get => mNativeStyle.ScrollbarSize; set => mNativeStyle.ScrollbarSize = value; }
        public float SizeGrabMin { get => mNativeStyle.GrabMinSize; set => mNativeStyle.GrabMinSize = value; }

        // ALIGN
        public Value2 Aligns
        {
            private get => AlignWindowTitle;

            set
            {
                AlignWindowTitle = value;
                AlignButtonText = value;
                AlignSelectableText = value;
                AlignSeparatorText = value;
            }
        }
        public Value2 AlignWindowTitle { get => mNativeStyle.WindowTitleAlign; set => mNativeStyle.WindowTitleAlign = value; }
        public Value2 AlignButtonText { get => mNativeStyle.ButtonTextAlign; set => mNativeStyle.ButtonTextAlign = value; }
        public Value2 AlignSelectableText { get => mNativeStyle.SelectableTextAlign; set => mNativeStyle.SelectableTextAlign = value; }
        public Value2 AlignSeparatorText { get => mNativeStyle.SeparatorTextAlign; set => mNativeStyle.SeparatorTextAlign = value; }

        // RENDERING
        public float Alpha { get => mNativeStyle.Alpha; set => mNativeStyle.Alpha = value; }
        public float DisabledAlpha { get => mNativeStyle.DisabledAlpha; set => mNativeStyle.DisabledAlpha = value; }
        public bool AntiAliasedLines { get => BitConverter.ToBoolean(new byte[] { mNativeStyle.AntiAliasedLines }); set => mNativeStyle.AntiAliasedLines = value ? (byte)1 : (byte)0; }
        public bool AntiAliasedLinesUseTex { get => BitConverter.ToBoolean(new byte[] { mNativeStyle.AntiAliasedLinesUseTex }); set => mNativeStyle.AntiAliasedLinesUseTex = value ? (byte)1 : (byte)0; }
        public bool AntiAliasedFill { get => BitConverter.ToBoolean(new byte[] { mNativeStyle.AntiAliasedFill }); set => mNativeStyle.AntiAliasedFill = value ? (byte)1 : (byte)0; }
        public float CurveTessellationTol { get => mNativeStyle.CurveTessellationTol; set => mNativeStyle.CurveTessellationTol = value; }
        public float CircleTessellationMaxError { get => mNativeStyle.CircleTessellationMaxError; set => mNativeStyle.CircleTessellationMaxError = value; }

        // HOVER
        public float HoverStationaryDelay { get => mNativeStyle.HoverStationaryDelay; set => mNativeStyle.HoverStationaryDelay = value; }
        public float HoverDelayShort { get => mNativeStyle.HoverDelayShort; set => mNativeStyle.HoverDelayShort = value; }
        public float HoverDelayNormal { get => mNativeStyle.HoverDelayNormal; set => mNativeStyle.HoverDelayNormal = value; }
        public ImGuiHoveredFlags HoverFlagsForTooltipMouse { get => mNativeStyle.HoverFlagsForTooltipMouse; set => mNativeStyle.HoverFlagsForTooltipMouse = value; }
        public ImGuiHoveredFlags HoverFlagsForTooltipNav { get => mNativeStyle.HoverFlagsForTooltipNav; set => mNativeStyle.HoverFlagsForTooltipNav = value; }

        // OTHER
        public ImGuiDir WindowMenuButtonPosition { get => mNativeStyle.WindowMenuButtonPosition; set => mNativeStyle.WindowMenuButtonPosition = value; }
        public ImGuiDir ColorButtonPosition { get => mNativeStyle.ColorButtonPosition; set => mNativeStyle.ColorButtonPosition = value; }
        public float LogSliderDeadzone { get => mNativeStyle.LogSliderDeadzone; set => mNativeStyle.LogSliderDeadzone = value; }
        public float TabMinWidthForCloseButton { get => mNativeStyle.TabMinWidthForCloseButton; set => mNativeStyle.TabMinWidthForCloseButton = value; }
        public float MouseCursorScale { get => mNativeStyle.MouseCursorScale; set => mNativeStyle.MouseCursorScale = value; }

        // COLORS
        public Value4 Colors
        {
            private get => ColorBackgroundWindow;

            set
            {
                ColorBackgroundWindow = value;
                ColorBackgroundChild = value;
                ColorBackgroundPopup = value;
                ColorBackgroundFrame = value;
                ColorBackgroundFrameHovered = value;
                ColorBackgroundFrameActive = value;
                ColorBackgroundTitle = value;
                ColorBackgroundTitleActive = value;
                ColorBackgroundTitleCollapsed = value;
                ColorBackgroundMenuBar = value;
                ColorBackgroundScrollbar = value;
                ColorBackgroundDockingEmpty = value;
                ColorBackgroundTableHeader = value;
                ColorBackgroundTableRow = value;
                ColorBackgroundTableRowAlt = value;
                ColorBackgroundTextSelected = value;
                ColorBackgroundNavWindowingDim = value;
                ColorBackgroundModalWindowDim = value;

                // COLORS: TEXT
                ColorText = value;
                ColorTextDisabled = value;

                // COLORS: BORDERS
                ColorBorder = value;
                ColorBorderShadow = value;
                ColorTableBorderStrong = value;
                ColorTableBorderLight = value;

                // COLORS: SCROLL
                ColorScrollbarGrab = value;
                ColorScrollbarGrabHovered = value;
                ColorScrollbarGrabActive = value;


                // COLORS: SLIDER
                ColorSliderGrab = value;
                ColorSliderGrabActive = value;

                // COLORS: BUTTON
                ColorButton = value;
                ColorButtonHovered = value;
                ColorButtonActive = value;

                // COLORS: HEADER
                ColorHeader = value;
                ColorHeaderHovered = value;
                ColorHeaderActive = value;

                // COLORS: SEPARATOR
                ColorSeparator = value;
                ColorSeparatorHovered = value;
                ColorSeparatorActive = value;

                // COLORS: RESIZE
                ColorResizeGrip = value;
                ColorResizeGripHovered = value;
                ColorResizeGripActive = value;

                // COLORS: TAB
                ColorTab = value;
                ColorTabHovered = value;
                ColorTabActive = value;
                ColorTabUnfocused = value;
                ColorTabUnfocusedActive = value;

                // COLORS: PLOT
                ColorPlotLines = value;
                ColorPlotLinesHovered = value;
                ColorPlotHistogram = value;
                ColorPlotHistogramHovered = value;

                // COLORS: OTHER
                ColorCheckMark = value;
                ColorDockingPreview = value;
                ColorDragDropTarget = value;
                ColorNavHighlight = value;
                ColorNavWindowingHighlight = value;
            }
        }
        public Value4 ColorsActive
        {
            private get => ColorBackgroundFrameActive;

            set
            {
                ColorBackgroundFrameActive = value;
                ColorBackgroundTitleActive = value;
                ColorScrollbarGrabActive = value;
                ColorSliderGrabActive = value;
                ColorButtonActive = value;
                ColorHeaderActive = value;
                ColorSeparatorActive = value;
                ColorResizeGripActive = value;
                ColorTabActive = value;
                ColorTabUnfocusedActive = value;
            }
        }
        public Value4 ColorsHovered
        {
            private get => ColorBackgroundFrameHovered;

            set
            {
                ColorBackgroundFrameHovered = value;
                ColorScrollbarGrabHovered = value;
                ColorButtonHovered = value;
                ColorHeaderHovered = value;
                ColorSeparatorHovered = value;
                ColorResizeGripHovered = value;
                ColorTabHovered = value;
                ColorPlotLinesHovered = value;
                ColorPlotHistogramHovered = value;
            }
        }

        // COLORS: BACKGROUND
        public Value4 ColorsBackground
        {
            private get => ColorBackgroundWindow;

            set
            {
                ColorBackgroundWindow = value;
                ColorBackgroundChild = value;
                ColorBackgroundPopup = value;
                ColorBackgroundFrame = value;
                ColorBackgroundFrameHovered = value;
                ColorBackgroundFrameActive = value;
                ColorBackgroundTitle = value;
                ColorBackgroundTitleActive = value;
                ColorBackgroundTitleCollapsed = value;
                ColorBackgroundMenuBar = value;
                ColorBackgroundScrollbar = value;
                ColorBackgroundDockingEmpty = value;
                ColorBackgroundTableHeader = value;
                ColorBackgroundTableRow = value;
                ColorBackgroundTableRowAlt = value;
                ColorBackgroundTextSelected = value;
                ColorBackgroundNavWindowingDim = value;
                ColorBackgroundModalWindowDim = value;
            }
        }
        public Value4 ColorsBackgroundActive
        {
            private get => ColorBackgroundFrameActive;

            set
            {
                ColorBackgroundFrameActive = value;
                ColorBackgroundTitleActive = value;
            }
        }
        public Value4 ColorsBackgroundHovered
        {
            private get => ColorBackgroundFrameHovered;

            set
            {
                ColorBackgroundFrameHovered = value;
            }
        }
        public Value4 ColorBackgroundWindow { get => mNativeStyle.Colors_2; set => mNativeStyle.Colors_2 = value; }
        public Value4 ColorBackgroundChild { get => mNativeStyle.Colors_3; set => mNativeStyle.Colors_3 = value; }
        public Value4 ColorBackgroundPopup { get => mNativeStyle.Colors_4; set => mNativeStyle.Colors_4 = value; }
        public Value4 ColorBackgroundFrame { get => mNativeStyle.Colors_7; set => mNativeStyle.Colors_7 = value; }
        public Value4 ColorBackgroundFrameHovered { get => mNativeStyle.Colors_8; set => mNativeStyle.Colors_8 = value; }
        public Value4 ColorBackgroundFrameActive { get => mNativeStyle.Colors_9; set => mNativeStyle.Colors_9 = value; }
        public Value4 ColorBackgroundTitle { get => mNativeStyle.Colors_10; set => mNativeStyle.Colors_10 = value; }
        public Value4 ColorBackgroundTitleActive { get => mNativeStyle.Colors_11; set => mNativeStyle.Colors_11 = value; }
        public Value4 ColorBackgroundTitleCollapsed { get => mNativeStyle.Colors_12; set => mNativeStyle.Colors_12 = value; }
        public Value4 ColorBackgroundMenuBar { get => mNativeStyle.Colors_13; set => mNativeStyle.Colors_13 = value; }
        public Value4 ColorBackgroundScrollbar { get => mNativeStyle.Colors_14; set => mNativeStyle.Colors_14 = value; }
        public Value4 ColorBackgroundDockingEmpty { get => mNativeStyle.Colors_39; set => mNativeStyle.Colors_39 = value; }
        public Value4 ColorBackgroundTableHeader { get => mNativeStyle.Colors_44; set => mNativeStyle.Colors_44 = value; }
        public Value4 ColorBackgroundTableRow { get => mNativeStyle.Colors_47; set => mNativeStyle.Colors_47 = value; }
        public Value4 ColorBackgroundTableRowAlt { get => mNativeStyle.Colors_48; set => mNativeStyle.Colors_48 = value; }
        public Value4 ColorBackgroundTextSelected { get => mNativeStyle.Colors_49; set => mNativeStyle.Colors_49 = value; }
        public Value4 ColorBackgroundNavWindowingDim { get => mNativeStyle.Colors_53; set => mNativeStyle.Colors_53 = value; }
        public Value4 ColorBackgroundModalWindowDim { get => mNativeStyle.Colors_54; set => mNativeStyle.Colors_54 = value; }

        // COLORS: TEXT
        public Value4 ColorText { get => mNativeStyle.Colors_0; set => mNativeStyle.Colors_0 = value; }
        public Value4 ColorTextDisabled { get => mNativeStyle.Colors_1; set => mNativeStyle.Colors_1 = value; }

        // COLORS: BORDERS
        public Value4 ColorBorder { get => mNativeStyle.Colors_5; set => mNativeStyle.Colors_5 = value; }
        public Value4 ColorBorderShadow { get => mNativeStyle.Colors_6; set => mNativeStyle.Colors_6 = value; }
        public Value4 ColorTableBorderStrong { get => mNativeStyle.Colors_45; set => mNativeStyle.Colors_45 = value; }
        public Value4 ColorTableBorderLight { get => mNativeStyle.Colors_46; set => mNativeStyle.Colors_46 = value; }

        // COLORS: SCROLL
        public Value4 ColorScrollbarGrab { get => mNativeStyle.Colors_15; set => mNativeStyle.Colors_15 = value; }
        public Value4 ColorScrollbarGrabHovered { get => mNativeStyle.Colors_16; set => mNativeStyle.Colors_16 = value; }
        public Value4 ColorScrollbarGrabActive { get => mNativeStyle.Colors_17; set => mNativeStyle.Colors_17 = value; }


        // COLORS: SLIDER
        public Value4 ColorSliderGrab { get => mNativeStyle.Colors_19; set => mNativeStyle.Colors_19 = value; }
        public Value4 ColorSliderGrabActive { get => mNativeStyle.Colors_20; set => mNativeStyle.Colors_20 = value; }

        // COLORS: BUTTON
        public Value4 ColorButton { get => mNativeStyle.Colors_21; set => mNativeStyle.Colors_21 = value; }
        public Value4 ColorButtonHovered { get => mNativeStyle.Colors_22; set => mNativeStyle.Colors_22 = value; }
        public Value4 ColorButtonActive { get => mNativeStyle.Colors_23; set => mNativeStyle.Colors_23 = value; }

        // COLORS: HEADER
        public Value4 ColorHeader { get => mNativeStyle.Colors_24; set => mNativeStyle.Colors_24 = value; }
        public Value4 ColorHeaderHovered { get => mNativeStyle.Colors_25; set => mNativeStyle.Colors_25 = value; }
        public Value4 ColorHeaderActive { get => mNativeStyle.Colors_26; set => mNativeStyle.Colors_26 = value; }

        // COLORS: SEPARATOR
        public Value4 ColorSeparator { get => mNativeStyle.Colors_27; set => mNativeStyle.Colors_27 = value; }
        public Value4 ColorSeparatorHovered { get => mNativeStyle.Colors_28; set => mNativeStyle.Colors_28 = value; }
        public Value4 ColorSeparatorActive { get => mNativeStyle.Colors_29; set => mNativeStyle.Colors_29 = value; }

        // COLORS: RESIZE
        public Value4 ColorResizeGrip { get => mNativeStyle.Colors_30; set => mNativeStyle.Colors_30 = value; }
        public Value4 ColorResizeGripHovered { get => mNativeStyle.Colors_31; set => mNativeStyle.Colors_31 = value; }
        public Value4 ColorResizeGripActive { get => mNativeStyle.Colors_32; set => mNativeStyle.Colors_32 = value; }

        // COLORS: TAB
        public Value4 ColorTab { get => mNativeStyle.Colors_33; set => mNativeStyle.Colors_33 = value; }
        public Value4 ColorTabHovered { get => mNativeStyle.Colors_34; set => mNativeStyle.Colors_34 = value; }
        public Value4 ColorTabActive { get => mNativeStyle.Colors_35; set => mNativeStyle.Colors_35 = value; }
        public Value4 ColorTabUnfocused { get => mNativeStyle.Colors_36; set => mNativeStyle.Colors_36 = value; }
        public Value4 ColorTabUnfocusedActive { get => mNativeStyle.Colors_37; set => mNativeStyle.Colors_37 = value; }

        // COLORS: PLOT
        public Value4 ColorPlotLines { get => mNativeStyle.Colors_40; set => mNativeStyle.Colors_40 = value; }
        public Value4 ColorPlotLinesHovered { get => mNativeStyle.Colors_41; set => mNativeStyle.Colors_41 = value; }
        public Value4 ColorPlotHistogram { get => mNativeStyle.Colors_42; set => mNativeStyle.Colors_42 = value; }
        public Value4 ColorPlotHistogramHovered { get => mNativeStyle.Colors_43; set => mNativeStyle.Colors_43 = value; }

        // COLORS: OTHER
        public Value4 ColorCheckMark { get => mNativeStyle.Colors_18; set => mNativeStyle.Colors_18 = value; }
        public Value4 ColorDockingPreview { get => mNativeStyle.Colors_38; set => mNativeStyle.Colors_38 = value; }
        public Value4 ColorDragDropTarget { get => mNativeStyle.Colors_50; set => mNativeStyle.Colors_50 = value; }
        public Value4 ColorNavHighlight { get => mNativeStyle.Colors_51; set => mNativeStyle.Colors_51 = value; }
        public Value4 ColorNavWindowingHighlight { get => mNativeStyle.Colors_52; set => mNativeStyle.Colors_52 = value; }

        // FONT
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
        public string FontName { get => Font.name; set => Font = (value, FontSize); }
        public int FontSize { get => Font.size; set => Font = (FontName, value); }
        public bool FontLoaded { get; private set; }

        private ImGuiStyle mPrevStyle;
        private ImGuiStyle mNativeStyle;
        private ImFontPtr mFont;
        private (string name, int size) mFontKey;
        private bool mPushed = false;
        private bool mFontPushed = false;

        public Style(Style style) : this(style.NativeStyle, style.FontName, style.FontSize)
        {

        }

        public Style(ImGuiStyle style, string fontName, int fontSize = 14)
        {
            mNativeStyle = style;
            Font = (fontName, fontSize);
        }

        public Style(string fontName, int fontSize = 14)
        {
            unsafe
            {
                mNativeStyle = *ImGui.GetStyle().NativePtr;
            }

            Font = (fontName, fontSize);
        }

        public Style()
        {
            unsafe
            {
                mNativeStyle = *ImGui.GetStyle().NativePtr;
            }

            Font = ("", 14);
        }

        internal void Push()
        {
            if (mPushed) return;
            mPushed = true;

            unsafe
            {
                mPrevStyle = *ImGui.GetStyle().NativePtr;
            }
            
            if (FontLoaded)
            {
                ImGui.PushFont(mFont);
                mFontPushed = true;
            }

            ApplyStyle(in mNativeStyle);
        }
        internal void Pop()
        {
            if (!mPushed) return;
            mPushed = false;

            if (mFontPushed)
            {
                ImGui.PopFont();
                mFontPushed = false;
            }

            ApplyStyle(in mPrevStyle);
        }
        unsafe static private void ApplyStyle(in ImGuiStyle style)
        {
            ref ImGuiStyle currentStyle = ref *ImGui.GetStyle().NativePtr;

            currentStyle.Alpha = style.Alpha;
            currentStyle.DisabledAlpha = style.DisabledAlpha;
            currentStyle.WindowPadding = style.WindowPadding;
            currentStyle.WindowRounding = style.WindowRounding;
            currentStyle.WindowBorderSize = style.WindowBorderSize;
            currentStyle.WindowMinSize = style.WindowMinSize;
            currentStyle.WindowTitleAlign = style.WindowTitleAlign;
            currentStyle.WindowMenuButtonPosition = style.WindowMenuButtonPosition;
            currentStyle.ChildRounding = style.ChildRounding;
            currentStyle.ChildBorderSize = style.ChildBorderSize;
            currentStyle.PopupRounding = style.PopupRounding;
            currentStyle.PopupBorderSize = style.PopupBorderSize;
            currentStyle.FramePadding = style.FramePadding;
            currentStyle.FrameRounding = style.FrameRounding;
            currentStyle.FrameBorderSize = style.FrameBorderSize;
            currentStyle.ItemSpacing = style.ItemSpacing;
            currentStyle.ItemInnerSpacing = style.ItemInnerSpacing;
            currentStyle.CellPadding = style.CellPadding;
            currentStyle.TouchExtraPadding = style.TouchExtraPadding;
            currentStyle.IndentSpacing = style.IndentSpacing;
            currentStyle.ColumnsMinSpacing = style.ColumnsMinSpacing;
            currentStyle.ScrollbarSize = style.ScrollbarSize;
            currentStyle.ScrollbarRounding = style.ScrollbarRounding;
            currentStyle.GrabMinSize = style.GrabMinSize;
            currentStyle.GrabRounding = style.GrabRounding;
            currentStyle.LogSliderDeadzone = style.LogSliderDeadzone;
            currentStyle.TabRounding = style.TabRounding;
            currentStyle.TabBorderSize = style.TabBorderSize;
            currentStyle.TabMinWidthForCloseButton = style.TabMinWidthForCloseButton;
            currentStyle.ColorButtonPosition = style.ColorButtonPosition;
            currentStyle.ButtonTextAlign = style.ButtonTextAlign;
            currentStyle.SelectableTextAlign = style.SelectableTextAlign;
            currentStyle.SeparatorTextBorderSize = style.SeparatorTextBorderSize;
            currentStyle.SeparatorTextAlign = style.SeparatorTextAlign;
            currentStyle.SeparatorTextPadding = style.SeparatorTextPadding;
            currentStyle.DisplayWindowPadding = style.DisplayWindowPadding;
            currentStyle.DisplaySafeAreaPadding = style.DisplaySafeAreaPadding;
            currentStyle.MouseCursorScale = style.MouseCursorScale;
            currentStyle.AntiAliasedLines = style.AntiAliasedLines;
            currentStyle.AntiAliasedLinesUseTex = style.AntiAliasedLinesUseTex;
            currentStyle.AntiAliasedFill = style.AntiAliasedFill;
            currentStyle.CurveTessellationTol = style.CurveTessellationTol;
            currentStyle.CircleTessellationMaxError = style.CircleTessellationMaxError;
            currentStyle.Colors_0 = style.Colors_0;
            currentStyle.Colors_1 = style.Colors_1;
            currentStyle.Colors_2 = style.Colors_2;
            currentStyle.Colors_3 = style.Colors_3;
            currentStyle.Colors_4 = style.Colors_4;
            currentStyle.Colors_5 = style.Colors_5;
            currentStyle.Colors_6 = style.Colors_6;
            currentStyle.Colors_7 = style.Colors_7;
            currentStyle.Colors_8 = style.Colors_8;
            currentStyle.Colors_9 = style.Colors_9;
            currentStyle.Colors_10 = style.Colors_10;
            currentStyle.Colors_11 = style.Colors_11;
            currentStyle.Colors_12 = style.Colors_12;
            currentStyle.Colors_13 = style.Colors_13;
            currentStyle.Colors_14 = style.Colors_14;
            currentStyle.Colors_15 = style.Colors_15;
            currentStyle.Colors_16 = style.Colors_16;
            currentStyle.Colors_17 = style.Colors_17;
            currentStyle.Colors_18 = style.Colors_18;
            currentStyle.Colors_19 = style.Colors_19;
            currentStyle.Colors_20 = style.Colors_20;
            currentStyle.Colors_21 = style.Colors_21;
            currentStyle.Colors_22 = style.Colors_22;
            currentStyle.Colors_23 = style.Colors_23;
            currentStyle.Colors_24 = style.Colors_24;
            currentStyle.Colors_25 = style.Colors_25;
            currentStyle.Colors_26 = style.Colors_26;
            currentStyle.Colors_27 = style.Colors_27;
            currentStyle.Colors_28 = style.Colors_28;
            currentStyle.Colors_29 = style.Colors_29;
            currentStyle.Colors_30 = style.Colors_30;
            currentStyle.Colors_31 = style.Colors_31;
            currentStyle.Colors_32 = style.Colors_32;
            currentStyle.Colors_33 = style.Colors_33;
            currentStyle.Colors_34 = style.Colors_34;
            currentStyle.Colors_35 = style.Colors_35;
            currentStyle.Colors_36 = style.Colors_36;
            currentStyle.Colors_37 = style.Colors_37;
            currentStyle.Colors_38 = style.Colors_38;
            currentStyle.Colors_39 = style.Colors_39;
            currentStyle.Colors_40 = style.Colors_40;
            currentStyle.Colors_41 = style.Colors_41;
            currentStyle.Colors_42 = style.Colors_42;
            currentStyle.Colors_43 = style.Colors_43;
            currentStyle.Colors_44 = style.Colors_44;
            currentStyle.Colors_45 = style.Colors_45;
            currentStyle.Colors_46 = style.Colors_46;
            currentStyle.Colors_47 = style.Colors_47;
            currentStyle.Colors_48 = style.Colors_48;
            currentStyle.Colors_49 = style.Colors_49;
            currentStyle.Colors_50 = style.Colors_50;
            currentStyle.Colors_51 = style.Colors_51;
            currentStyle.Colors_52 = style.Colors_52;
            currentStyle.Colors_53 = style.Colors_53;
            currentStyle.Colors_54 = style.Colors_54;
            currentStyle.HoverStationaryDelay = style.HoverStationaryDelay;
            currentStyle.HoverDelayShort = style.HoverDelayShort;
            currentStyle.HoverDelayNormal = style.HoverDelayNormal;
            currentStyle.HoverFlagsForTooltipMouse = style.HoverFlagsForTooltipMouse;
            currentStyle.HoverFlagsForTooltipNav = style.HoverFlagsForTooltipNav;
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
}
