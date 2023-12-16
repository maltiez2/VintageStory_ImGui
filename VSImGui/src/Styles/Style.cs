using ImGuiNET;
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Datastructures;
using VSImGui.ImGuiUtils;

namespace VSImGui
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Style
    {
        [JsonIgnore]
        public ImGuiStyle NativeStyle => mNativeStyle;

        // PADDINGS
        [JsonIgnore]
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
        [JsonIgnore]
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
        [JsonIgnore]
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
        [JsonIgnore]
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
        [JsonIgnore]
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
        [JsonIgnore]
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
        [JsonIgnore]
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
        [JsonIgnore]
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
        [JsonIgnore]
        public Value4 ColorsBackgroundActive
        {
            private get => ColorBackgroundFrameActive;

            set
            {
                ColorBackgroundFrameActive = value;
                ColorBackgroundTitleActive = value;
            }
        }
        [JsonIgnore]
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
        [JsonIgnore]
        public (string name, int size) Font
        {
            get => mFontKey;
            set
            {
                mFontKey = value;
                mFontLoaded = ImGuiController.LoadedFonts.ContainsKey(value);
                if (mFontLoaded)
                {
                    mFont = ImGuiController.LoadedFonts[value];
                }
            }
        }
        public string FontName { get => Font.name; set => Font = (value, FontSize); }
        public int FontSize { get => Font.size; set => Font = (FontName, value); }
        [JsonIgnore]
        public bool FontLoaded => mFontLoaded;


        private ImGuiStyle mPrevStyle;
        private ImGuiStyle mNativeStyle;
        private ImFontPtr mFont;
        private (string name, int size) mFontKey;
        private bool mPushed = false;
        private bool mFontPushed = false;
        private bool mFontLoaded = false;

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
        unsafe static internal void ApplyStyle(in ImGuiStyle style)
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
        
        public virtual void SetFrom(Style style)
        {
            mPrevStyle = style.mPrevStyle;
            mNativeStyle = style.mNativeStyle;
            mFont = style.mFont;
            if (style.mFontKey.name != null) mFontKey = ((string)style.mFontKey.name.Clone(), style.mFontKey.size);
            mPushed = style.mPushed;
            mFontPushed = style.mFontPushed;
            mFontLoaded = style.mFontLoaded;
        }
        public virtual string ToCode()
        {
            StringBuilder result = new();

            result.AppendLine("VSImGui.Style style = new();");

            OtherPropertiesToCode(result);
            FloatPropertiesToCode(result);
            Value2PropertiesToCode(result);
            Value4PropertiesToCode(result);

            return result.ToString();
        }
        

        protected void FloatPropertiesToCode(StringBuilder result)
        {
            if (!Utils.CompareFloats(mPrevStyle.IndentSpacing, mNativeStyle.IndentSpacing)) result.AppendLine($"style.SpacingIndent = {SpacingIndent}f;");
            if (!Utils.CompareFloats(mPrevStyle.ColumnsMinSpacing, mNativeStyle.ColumnsMinSpacing)) result.AppendLine($"style.SpacingColumnsMin = {SpacingColumnsMin}f;");
            if (!Utils.CompareFloats(mPrevStyle.WindowBorderSize, mNativeStyle.WindowBorderSize)) result.AppendLine($"style.BorderWindow = {BorderWindow}f;");
            if (!Utils.CompareFloats(mPrevStyle.ChildBorderSize, mNativeStyle.ChildBorderSize)) result.AppendLine($"style.BorderChild = {BorderChild}f;");
            if (!Utils.CompareFloats(mPrevStyle.PopupBorderSize, mNativeStyle.PopupBorderSize)) result.AppendLine($"style.BorderPopup = {BorderPopup}f;");
            if (!Utils.CompareFloats(mPrevStyle.FrameBorderSize, mNativeStyle.FrameBorderSize)) result.AppendLine($"style.BorderFrame = {BorderFrame}f;");
            if (!Utils.CompareFloats(mPrevStyle.TabBorderSize, mNativeStyle.TabBorderSize)) result.AppendLine($"style.BorderTab = {BorderTab}f;");
            if (!Utils.CompareFloats(mPrevStyle.SeparatorTextBorderSize, mNativeStyle.SeparatorTextBorderSize)) result.AppendLine($"style.BorderSeparatorText = {BorderSeparatorText}f;");
            if (!Utils.CompareFloats(mPrevStyle.WindowRounding, mNativeStyle.WindowRounding)) result.AppendLine($"style.RoundingWindow = {RoundingWindow}f;");
            if (!Utils.CompareFloats(mPrevStyle.ChildRounding, mNativeStyle.ChildRounding)) result.AppendLine($"style.RoundingChild = {RoundingChild}f;");
            if (!Utils.CompareFloats(mPrevStyle.PopupRounding, mNativeStyle.PopupRounding)) result.AppendLine($"style.RoundingPopup = {RoundingPopup}f;");
            if (!Utils.CompareFloats(mPrevStyle.FrameRounding, mNativeStyle.FrameRounding)) result.AppendLine($"style.RoundingFrame = {RoundingFrame}f;");
            if (!Utils.CompareFloats(mPrevStyle.ScrollbarRounding, mNativeStyle.ScrollbarRounding)) result.AppendLine($"style.RoundingScrollbar = {RoundingScrollbar}f;");
            if (!Utils.CompareFloats(mPrevStyle.GrabRounding, mNativeStyle.GrabRounding)) result.AppendLine($"style.RoundingGrab = {RoundingGrab}f;");
            if (!Utils.CompareFloats(mPrevStyle.TabRounding, mNativeStyle.TabRounding)) result.AppendLine($"style.RoundingTab = {RoundingTab}f;");
            if (!Utils.CompareFloats(mPrevStyle.ScrollbarSize, mNativeStyle.ScrollbarSize)) result.AppendLine($"style.SizeScrollbar = {SizeScrollbar}f;");
            if (!Utils.CompareFloats(mPrevStyle.GrabMinSize, mNativeStyle.GrabMinSize)) result.AppendLine($"style.SizeGrabMin = {SizeGrabMin}f;");
            if (!Utils.CompareFloats(mPrevStyle.Alpha, mNativeStyle.Alpha)) result.AppendLine($"style.Alpha = {Alpha}f;");
            if (!Utils.CompareFloats(mPrevStyle.DisabledAlpha, mNativeStyle.DisabledAlpha)) result.AppendLine($"style.DisabledAlpha = {DisabledAlpha}f;");
            if (!Utils.CompareFloats(mPrevStyle.CurveTessellationTol, mNativeStyle.CurveTessellationTol)) result.AppendLine($"style.CurveTessellationTol = {CurveTessellationTol}f;");
            if (!Utils.CompareFloats(mPrevStyle.CircleTessellationMaxError, mNativeStyle.CircleTessellationMaxError)) result.AppendLine($"style.CircleTessellationMaxError = {CircleTessellationMaxError}f;");
            if (!Utils.CompareFloats(mPrevStyle.HoverStationaryDelay, mNativeStyle.HoverStationaryDelay)) result.AppendLine($"style.HoverStationaryDelay = {HoverStationaryDelay}f;");
            if (!Utils.CompareFloats(mPrevStyle.HoverDelayShort, mNativeStyle.HoverDelayShort)) result.AppendLine($"style.HoverDelayShort = {HoverDelayShort}f;");
            if (!Utils.CompareFloats(mPrevStyle.HoverDelayNormal, mNativeStyle.HoverDelayNormal)) result.AppendLine($"style.HoverDelayNormal = {HoverDelayNormal}f;");
            if (!Utils.CompareFloats(mPrevStyle.LogSliderDeadzone, mNativeStyle.LogSliderDeadzone)) result.AppendLine($"style.LogSliderDeadzone = {LogSliderDeadzone}f;");
            if (!Utils.CompareFloats(mPrevStyle.TabMinWidthForCloseButton, mNativeStyle.TabMinWidthForCloseButton)) result.AppendLine($"style.TabMinWidthForCloseButton = {TabMinWidthForCloseButton}f;");
            if (!Utils.CompareFloats(mPrevStyle.MouseCursorScale, mNativeStyle.MouseCursorScale)) result.AppendLine($"style.MouseCursorScale = {MouseCursorScale}f;");
        }
        protected void Value2PropertiesToCode(StringBuilder result)
        {
            if (mPrevStyle.WindowPadding != mNativeStyle.WindowPadding) result.AppendLine($"style.PaddingWindow = ({PaddingWindow.X},{PaddingWindow.Y});");
            if (mPrevStyle.FramePadding != mNativeStyle.FramePadding) result.AppendLine($"style.PaddingFrame = ({PaddingFrame.X},{PaddingFrame.Y});");
            if (mPrevStyle.CellPadding != mNativeStyle.CellPadding) result.AppendLine($"style.PaddingCell = ({PaddingCell.X},{PaddingCell.Y});");
            if (mPrevStyle.TouchExtraPadding != mNativeStyle.TouchExtraPadding) result.AppendLine($"style.PaddingTouchExtra = ({PaddingTouchExtra.X},{PaddingTouchExtra.Y});");
            if (mPrevStyle.SeparatorTextPadding != mNativeStyle.SeparatorTextPadding) result.AppendLine($"style.PaddingSeparatorText = ({PaddingSeparatorText.X},{PaddingSeparatorText.Y});");
            if (mPrevStyle.DisplayWindowPadding != mNativeStyle.DisplayWindowPadding) result.AppendLine($"style.PaddingDisplayWindow = ({PaddingDisplayWindow.X},{PaddingDisplayWindow.Y});");
            if (mPrevStyle.DisplaySafeAreaPadding != mNativeStyle.DisplaySafeAreaPadding) result.AppendLine($"style.PaddingDisplaySafeArea = ({PaddingDisplaySafeArea.X},{PaddingDisplaySafeArea.Y});");
            if (mPrevStyle.ItemSpacing != mNativeStyle.ItemSpacing) result.AppendLine($"style.SpacingItem = ({SpacingItem.X},{SpacingItem.Y});");
            if (mPrevStyle.ItemInnerSpacing != mNativeStyle.ItemInnerSpacing) result.AppendLine($"style.SpacingItemInner = ({SpacingItemInner.X},{SpacingItemInner.Y});");
            if (mPrevStyle.WindowMinSize != mNativeStyle.WindowMinSize) result.AppendLine($"style.SizeWindowMin = ({SizeWindowMin.X},{SizeWindowMin.Y});");
            if (mPrevStyle.WindowTitleAlign != mNativeStyle.WindowTitleAlign) result.AppendLine($"style.AlignWindowTitle = ({AlignWindowTitle.X},{AlignWindowTitle.Y});");
            if (mPrevStyle.ButtonTextAlign != mNativeStyle.ButtonTextAlign) result.AppendLine($"style.AlignButtonText = ({AlignButtonText.X},{AlignButtonText.Y});");
            if (mPrevStyle.SelectableTextAlign != mNativeStyle.SelectableTextAlign) result.AppendLine($"style.AlignSelectableText = ({AlignSelectableText.X},{AlignSelectableText.Y});");
            if (mPrevStyle.SeparatorTextAlign != mNativeStyle.SeparatorTextAlign) result.AppendLine($"style.AlignSeparatorText = ({AlignSeparatorText.X},{AlignSeparatorText.Y});");
        }
        protected void Value4PropertiesToCode(StringBuilder result)
        {
            if (mPrevStyle.Colors_2 != mNativeStyle.Colors_2) result.AppendLine($"style.ColorBackgroundWindow = ({ColorBackgroundWindow.R},{ColorBackgroundWindow.G},{ColorBackgroundWindow.B},{ColorBackgroundWindow.A});");
            if (mPrevStyle.Colors_3 != mNativeStyle.Colors_3) result.AppendLine($"style.ColorBackgroundChild = ({ColorBackgroundChild.R},{ColorBackgroundChild.G},{ColorBackgroundChild.B},{ColorBackgroundChild.A});");
            if (mPrevStyle.Colors_4 != mNativeStyle.Colors_4) result.AppendLine($"style.ColorBackgroundPopup = ({ColorBackgroundPopup.R},{ColorBackgroundPopup.G},{ColorBackgroundPopup.B},{ColorBackgroundPopup.A});");
            if (mPrevStyle.Colors_7 != mNativeStyle.Colors_7) result.AppendLine($"style.ColorBackgroundFrame = ({ColorBackgroundFrame.R},{ColorBackgroundFrame.G},{ColorBackgroundFrame.B},{ColorBackgroundFrame.A});");
            if (mPrevStyle.Colors_8 != mNativeStyle.Colors_8) result.AppendLine($"style.ColorBackgroundFrameHovered = ({ColorBackgroundFrameHovered.R},{ColorBackgroundFrameHovered.G},{ColorBackgroundFrameHovered.B},{ColorBackgroundFrameHovered.A});");
            if (mPrevStyle.Colors_9 != mNativeStyle.Colors_9) result.AppendLine($"style.ColorBackgroundFrameActive = ({ColorBackgroundFrameActive.R},{ColorBackgroundFrameActive.G},{ColorBackgroundFrameActive.B},{ColorBackgroundFrameActive.A});");
            if (mPrevStyle.Colors_10 != mNativeStyle.Colors_10) result.AppendLine($"style.ColorBackgroundTitle = ({ColorBackgroundTitle.R},{ColorBackgroundTitle.G},{ColorBackgroundTitle.B},{ColorBackgroundTitle.A});");
            if (mPrevStyle.Colors_11 != mNativeStyle.Colors_11) result.AppendLine($"style.ColorBackgroundTitleActive = ({ColorBackgroundTitleActive.R},{ColorBackgroundTitleActive.G},{ColorBackgroundTitleActive.B},{ColorBackgroundTitleActive.A});");
            if (mPrevStyle.Colors_12 != mNativeStyle.Colors_12) result.AppendLine($"style.ColorBackgroundTitleCollapsed = ({ColorBackgroundTitleCollapsed.R},{ColorBackgroundTitleCollapsed.G},{ColorBackgroundTitleCollapsed.B},{ColorBackgroundTitleCollapsed.A});");
            if (mPrevStyle.Colors_13 != mNativeStyle.Colors_13) result.AppendLine($"style.ColorBackgroundMenuBar = ({ColorBackgroundMenuBar.R},{ColorBackgroundMenuBar.G},{ColorBackgroundMenuBar.B},{ColorBackgroundMenuBar.A});");
            if (mPrevStyle.Colors_14 != mNativeStyle.Colors_14) result.AppendLine($"style.ColorBackgroundScrollbar = ({ColorBackgroundScrollbar.R},{ColorBackgroundScrollbar.G},{ColorBackgroundScrollbar.B},{ColorBackgroundScrollbar.A});");
            if (mPrevStyle.Colors_39 != mNativeStyle.Colors_39) result.AppendLine($"style.ColorBackgroundDockingEmpty = ({ColorBackgroundDockingEmpty.R},{ColorBackgroundDockingEmpty.G},{ColorBackgroundDockingEmpty.B},{ColorBackgroundDockingEmpty.A});");
            if (mPrevStyle.Colors_44 != mNativeStyle.Colors_44) result.AppendLine($"style.ColorBackgroundTableHeader = ({ColorBackgroundTableHeader.R},{ColorBackgroundTableHeader.G},{ColorBackgroundTableHeader.B},{ColorBackgroundTableHeader.A});");
            if (mPrevStyle.Colors_47 != mNativeStyle.Colors_47) result.AppendLine($"style.ColorBackgroundTableRow = ({ColorBackgroundTableRow.R},{ColorBackgroundTableRow.G},{ColorBackgroundTableRow.B},{ColorBackgroundTableRow.A});");
            if (mPrevStyle.Colors_48 != mNativeStyle.Colors_48) result.AppendLine($"style.ColorBackgroundTableRowAlt = ({ColorBackgroundTableRowAlt.R},{ColorBackgroundTableRowAlt.G},{ColorBackgroundTableRowAlt.B},{ColorBackgroundTableRowAlt.A});");
            if (mPrevStyle.Colors_49 != mNativeStyle.Colors_49) result.AppendLine($"style.ColorBackgroundTextSelected = ({ColorBackgroundTextSelected.R},{ColorBackgroundTextSelected.G},{ColorBackgroundTextSelected.B},{ColorBackgroundTextSelected.A});");
            if (mPrevStyle.Colors_53 != mNativeStyle.Colors_53) result.AppendLine($"style.ColorBackgroundNavWindowingDim = ({ColorBackgroundNavWindowingDim.R},{ColorBackgroundNavWindowingDim.G},{ColorBackgroundNavWindowingDim.B},{ColorBackgroundNavWindowingDim.A});");
            if (mPrevStyle.Colors_54 != mNativeStyle.Colors_54) result.AppendLine($"style.ColorBackgroundModalWindowDim = ({ColorBackgroundModalWindowDim.R},{ColorBackgroundModalWindowDim.G},{ColorBackgroundModalWindowDim.B},{ColorBackgroundModalWindowDim.A});");
            if (mPrevStyle.Colors_0 != mNativeStyle.Colors_0) result.AppendLine($"style.ColorText = ({ColorText.R},{ColorText.G},{ColorText.B},{ColorText.A});");
            if (mPrevStyle.Colors_1 != mNativeStyle.Colors_1) result.AppendLine($"style.ColorTextDisabled = ({ColorTextDisabled.R},{ColorTextDisabled.G},{ColorTextDisabled.B},{ColorTextDisabled.A});");
            if (mPrevStyle.Colors_5 != mNativeStyle.Colors_5) result.AppendLine($"style.ColorBorder = ({ColorBorder.R},{ColorBorder.G},{ColorBorder.B},{ColorBorder.A});");
            if (mPrevStyle.Colors_6 != mNativeStyle.Colors_6) result.AppendLine($"style.ColorBorderShadow = ({ColorBorderShadow.R},{ColorBorderShadow.G},{ColorBorderShadow.B},{ColorBorderShadow.A});");
            if (mPrevStyle.Colors_45 != mNativeStyle.Colors_45) result.AppendLine($"style.ColorTableBorderStrong = ({ColorTableBorderStrong.R},{ColorTableBorderStrong.G},{ColorTableBorderStrong.B},{ColorTableBorderStrong.A});");
            if (mPrevStyle.Colors_46 != mNativeStyle.Colors_46) result.AppendLine($"style.ColorTableBorderLight = ({ColorTableBorderLight.R},{ColorTableBorderLight.G},{ColorTableBorderLight.B},{ColorTableBorderLight.A});");
            if (mPrevStyle.Colors_15 != mNativeStyle.Colors_15) result.AppendLine($"style.ColorScrollbarGrab = ({ColorScrollbarGrab.R},{ColorScrollbarGrab.G},{ColorScrollbarGrab.B},{ColorScrollbarGrab.A});");
            if (mPrevStyle.Colors_16 != mNativeStyle.Colors_16) result.AppendLine($"style.ColorScrollbarGrabHovered = ({ColorScrollbarGrabHovered.R},{ColorScrollbarGrabHovered.G},{ColorScrollbarGrabHovered.B},{ColorScrollbarGrabHovered.A});");
            if (mPrevStyle.Colors_17 != mNativeStyle.Colors_17) result.AppendLine($"style.ColorScrollbarGrabActive = ({ColorScrollbarGrabActive.R},{ColorScrollbarGrabActive.G},{ColorScrollbarGrabActive.B},{ColorScrollbarGrabActive.A});");
            if (mPrevStyle.Colors_19 != mNativeStyle.Colors_19) result.AppendLine($"style.ColorSliderGrab = ({ColorSliderGrab.R},{ColorSliderGrab.G},{ColorSliderGrab.B},{ColorSliderGrab.A});");
            if (mPrevStyle.Colors_20 != mNativeStyle.Colors_20) result.AppendLine($"style.ColorSliderGrabActive = ({ColorSliderGrabActive.R},{ColorSliderGrabActive.G},{ColorSliderGrabActive.B},{ColorSliderGrabActive.A});");
            if (mPrevStyle.Colors_21 != mNativeStyle.Colors_21) result.AppendLine($"style.ColorButton = ({ColorButton.R},{ColorButton.G},{ColorButton.B},{ColorButton.A});");
            if (mPrevStyle.Colors_22 != mNativeStyle.Colors_22) result.AppendLine($"style.ColorButtonHovered = ({ColorButtonHovered.R},{ColorButtonHovered.G},{ColorButtonHovered.B},{ColorButtonHovered.A});");
            if (mPrevStyle.Colors_23 != mNativeStyle.Colors_23) result.AppendLine($"style.ColorButtonActive = ({ColorButtonActive.R},{ColorButtonActive.G},{ColorButtonActive.B},{ColorButtonActive.A});");
            if (mPrevStyle.Colors_24 != mNativeStyle.Colors_24) result.AppendLine($"style.ColorHeader = ({ColorHeader.R},{ColorHeader.G},{ColorHeader.B},{ColorHeader.A});");
            if (mPrevStyle.Colors_25 != mNativeStyle.Colors_25) result.AppendLine($"style.ColorHeaderHovered = ({ColorHeaderHovered.R},{ColorHeaderHovered.G},{ColorHeaderHovered.B},{ColorHeaderHovered.A});");
            if (mPrevStyle.Colors_26 != mNativeStyle.Colors_26) result.AppendLine($"style.ColorHeaderActive = ({ColorHeaderActive.R},{ColorHeaderActive.G},{ColorHeaderActive.B},{ColorHeaderActive.A});");
            if (mPrevStyle.Colors_27 != mNativeStyle.Colors_27) result.AppendLine($"style.ColorSeparator = ({ColorSeparator.R},{ColorSeparator.G},{ColorSeparator.B},{ColorSeparator.A});");
            if (mPrevStyle.Colors_28 != mNativeStyle.Colors_28) result.AppendLine($"style.ColorSeparatorHovered = ({ColorSeparatorHovered.R},{ColorSeparatorHovered.G},{ColorSeparatorHovered.B},{ColorSeparatorHovered.A});");
            if (mPrevStyle.Colors_29 != mNativeStyle.Colors_29) result.AppendLine($"style.ColorSeparatorActive = ({ColorSeparatorActive.R},{ColorSeparatorActive.G},{ColorSeparatorActive.B},{ColorSeparatorActive.A});");
            if (mPrevStyle.Colors_30 != mNativeStyle.Colors_30) result.AppendLine($"style.ColorResizeGrip = ({ColorResizeGrip.R},{ColorResizeGrip.G},{ColorResizeGrip.B},{ColorResizeGrip.A});");
            if (mPrevStyle.Colors_31 != mNativeStyle.Colors_31) result.AppendLine($"style.ColorResizeGripHovered = ({ColorResizeGripHovered.R},{ColorResizeGripHovered.G},{ColorResizeGripHovered.B},{ColorResizeGripHovered.A});");
            if (mPrevStyle.Colors_32 != mNativeStyle.Colors_32) result.AppendLine($"style.ColorResizeGripActive = ({ColorResizeGripActive.R},{ColorResizeGripActive.G},{ColorResizeGripActive.B},{ColorResizeGripActive.A});");
            if (mPrevStyle.Colors_33 != mNativeStyle.Colors_33) result.AppendLine($"style.ColorTab = ({ColorTab.R},{ColorTab.G},{ColorTab.B},{ColorTab.A});");
            if (mPrevStyle.Colors_34 != mNativeStyle.Colors_34) result.AppendLine($"style.ColorTabHovered = ({ColorTabHovered.R},{ColorTabHovered.G},{ColorTabHovered.B},{ColorTabHovered.A});");
            if (mPrevStyle.Colors_35 != mNativeStyle.Colors_35) result.AppendLine($"style.ColorTabActive = ({ColorTabActive.R},{ColorTabActive.G},{ColorTabActive.B},{ColorTabActive.A});");
            if (mPrevStyle.Colors_36 != mNativeStyle.Colors_36) result.AppendLine($"style.ColorTabUnfocused = ({ColorTabUnfocused.R},{ColorTabUnfocused.G},{ColorTabUnfocused.B},{ColorTabUnfocused.A});");
            if (mPrevStyle.Colors_37 != mNativeStyle.Colors_37) result.AppendLine($"style.ColorTabUnfocusedActive = ({ColorTabUnfocusedActive.R},{ColorTabUnfocusedActive.G},{ColorTabUnfocusedActive.B},{ColorTabUnfocusedActive.A});");
            if (mPrevStyle.Colors_40 != mNativeStyle.Colors_40) result.AppendLine($"style.ColorPlotLines = ({ColorPlotLines.R},{ColorPlotLines.G},{ColorPlotLines.B},{ColorPlotLines.A});");
            if (mPrevStyle.Colors_41 != mNativeStyle.Colors_41) result.AppendLine($"style.ColorPlotLinesHovered = ({ColorPlotLinesHovered.R},{ColorPlotLinesHovered.G},{ColorPlotLinesHovered.B},{ColorPlotLinesHovered.A});");
            if (mPrevStyle.Colors_42 != mNativeStyle.Colors_42) result.AppendLine($"style.ColorPlotHistogram = ({ColorPlotHistogram.R},{ColorPlotHistogram.G},{ColorPlotHistogram.B},{ColorPlotHistogram.A});");
            if (mPrevStyle.Colors_43 != mNativeStyle.Colors_43) result.AppendLine($"style.ColorPlotHistogramHovered = ({ColorPlotHistogramHovered.R},{ColorPlotHistogramHovered.G},{ColorPlotHistogramHovered.B},{ColorPlotHistogramHovered.A});");
            if (mPrevStyle.Colors_18 != mNativeStyle.Colors_18) result.AppendLine($"style.ColorCheckMark = ({ColorCheckMark.R},{ColorCheckMark.G},{ColorCheckMark.B},{ColorCheckMark.A});");
            if (mPrevStyle.Colors_38 != mNativeStyle.Colors_38) result.AppendLine($"style.ColorDockingPreview = ({ColorDockingPreview.R},{ColorDockingPreview.G},{ColorDockingPreview.B},{ColorDockingPreview.A});");
            if (mPrevStyle.Colors_50 != mNativeStyle.Colors_50) result.AppendLine($"style.ColorDragDropTarget = ({ColorDragDropTarget.R},{ColorDragDropTarget.G},{ColorDragDropTarget.B},{ColorDragDropTarget.A});");
            if (mPrevStyle.Colors_51 != mNativeStyle.Colors_51) result.AppendLine($"style.ColorNavHighlight = ({ColorNavHighlight.R},{ColorNavHighlight.G},{ColorNavHighlight.B},{ColorNavHighlight.A});");
            if (mPrevStyle.Colors_52 != mNativeStyle.Colors_52) result.AppendLine($"style.ColorNavWindowingHighlight = ({ColorNavWindowingHighlight.R},{ColorNavWindowingHighlight.G},{ColorNavWindowingHighlight.B},{ColorNavWindowingHighlight.A});");
        }
        protected void OtherPropertiesToCode(StringBuilder result)
        {

            if (mPrevStyle.AntiAliasedLines != mNativeStyle.AntiAliasedLines) result.AppendLine($"style.AntiAliasedLines = {AntiAliasedLines};");
            if (mPrevStyle.AntiAliasedLinesUseTex != mNativeStyle.AntiAliasedLinesUseTex) result.AppendLine($"style.AntiAliasedLinesUseTex = {AntiAliasedLinesUseTex};");
            if (mPrevStyle.AntiAliasedFill != mNativeStyle.AntiAliasedFill) result.AppendLine($"style.AntiAliasedLines = {AntiAliasedFill};");
            if (FontName != null && FontName != "") result.AppendLine($"style.FontName = \"{FontName}\";");
            if (FontName != null && FontName != "") result.AppendLine($"style.FontSize = {FontSize};");

            if (mPrevStyle.HoverFlagsForTooltipMouse != mNativeStyle.HoverFlagsForTooltipMouse) result.AppendLine($"style.HoverFlagsForTooltipMouse = (ImGuiHoveredFlags)0x{HoverFlagsForTooltipMouse.ToString("x")};");
            if (mPrevStyle.HoverFlagsForTooltipNav != mNativeStyle.HoverFlagsForTooltipNav) result.AppendLine($"style.HoverFlagsForTooltipNav = (ImGuiHoveredFlags)0x{HoverFlagsForTooltipNav.ToString("x")};");
            if (mPrevStyle.WindowMenuButtonPosition != mNativeStyle.WindowMenuButtonPosition) result.AppendLine($"style.WindowMenuButtonPosition = (ImGuiDir)0x{WindowMenuButtonPosition.ToString("x")};");
            if (mPrevStyle.ColorButtonPosition != mNativeStyle.ColorButtonPosition) result.AppendLine($"style.ColorButtonPosition = (ImGuiDir)0x{ColorButtonPosition.ToString("x")};");
        }
    }
}
