﻿//*********************************************************//
//    Copyright (c) Microsoft. All rights reserved.
//    
//    Apache 2.0 License
//    
//    You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
//    
//    Unless required by applicable law or agreed to in writing, software 
//    distributed under the License is distributed on an "AS IS" BASIS, 
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or 
//    implied. See the License for the specific language governing 
//    permissions and limitations under the License.
//
//*********************************************************//

using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.VisualStudioTools.Wpf {
    public static class Controls {
        public static readonly object BackgroundKey = VsBrushes.WindowKey;
        public static readonly object BackgroundAccentKey = VsBrushes.ButtonFaceKey;
        public static readonly object ForegroundKey = VsBrushes.WindowTextKey;
        public static readonly object GrayTextKey = VsBrushes.GrayTextKey;
        public static readonly object HighlightKey = VsBrushes.HighlightKey;
        public static readonly object HighlightTextKey = VsBrushes.HighlightTextKey;
        public static readonly object HotTrackKey = VsBrushes.CommandBarMouseOverBackgroundGradientKey;

        public static readonly object TooltipBackgroundKey = VsBrushes.InfoBackgroundKey;
        public static readonly object TooltipTextKey = VsBrushes.InfoTextKey;

        public static readonly object HyperlinkKey = VsBrushes.ControlLinkTextKey;
        public static readonly object HyperlinkHoverKey = VsBrushes.ControlLinkTextHoverKey;

        public static readonly object ControlBackgroundKey = VsBrushes.ComboBoxBackgroundKey;
        public static readonly object ControlForegroundKey = VsBrushes.WindowTextKey;
        public static readonly object ControlBorderKey = VsBrushes.ComboBoxBorderKey;
        public static readonly object ControlBackgroundHoverKey = VsBrushes.ComboBoxMouseOverBackgroundGradientKey;
        public static readonly object ControlBorderHoverKey = VsBrushes.ComboBoxMouseOverGlyphKey;
        public static readonly object ControlBackgroundPressedKey = VsBrushes.ComboBoxMouseDownBackgroundKey;
        public static readonly object ControlForegroundPressedKey = VsBrushes.ComboBoxGlyphKey;
        public static readonly object ControlBorderPressedKey = VsBrushes.ComboBoxMouseDownBorderKey;
        public static readonly object ControlBackgroundSelectedKey = VsBrushes.ComboBoxMouseDownBackgroundKey;
        public static readonly object ControlForegroundSelectedKey = VsBrushes.ComboBoxGlyphKey;
        public static readonly object ControlBorderSelectedKey = VsBrushes.ComboBoxMouseOverBorderKey;
        public static readonly object ControlBackgroundDisabledKey = VsBrushes.ComboBoxDisabledBackgroundKey;
        public static readonly object ControlForegroundDisabledKey = VsBrushes.ComboBoxDisabledGlyphKey;
        public static readonly object ControlBorderDisabledKey = VsBrushes.ComboBoxDisabledBorderKey;

        public static readonly object ComboBoxPopupBackgroundKey = VsBrushes.ComboBoxPopupBackgroundGradientKey;
        public static readonly object ComboBoxPopupBorderKey = VsBrushes.ComboBoxPopupBorderKey;
        public static readonly object ComboBoxPopupForegroundKey = VsBrushes.WindowTextKey;

        public static readonly object ButtonForegroundPressedKey = VsBrushes.ToolWindowButtonDownActiveGlyphKey;
        public static readonly object ButtonBackgroundPressedKey = VsBrushes.ComboBoxMouseOverBorderKey;
        public static readonly object ButtonBackgroundHoverKey = VsBrushes.CommandBarHoverOverSelectedKey;
        public static readonly object ButtonBorderHoverKey = VsBrushes.ComboBoxMouseOverGlyphKey;

        public static readonly object ScrollBarBackgroundKey = VsBrushes.ScrollBarBackgroundKey;
        public static readonly object ScrollBarThumbBackgroundKey = VsBrushes.ScrollBarThumbBackgroundKey;
        public static readonly object ScrollBarThumbBackgroundHoverKey = VsBrushes.ScrollBarThumbMouseOverBackgroundKey;
        public static readonly object ScrollBarThumbBackgroundPressedKey = VsBrushes.ScrollBarThumbPressedBackgroundKey;
        public static readonly object ScrollBarArrowKey = VsBrushes.ScrollBarThumbGlyphKey;
        public static readonly object ScrollBarArrowHoverKey = VsBrushes.GrayTextKey;
        public static readonly object ScrollBarArrowPressedKey = VsBrushes.WindowTextKey;
        public static readonly object ScrollBarArrowDisabledKey = VsBrushes.ScrollBarThumbGlyphKey;
        public static readonly object ScrollBarArrowBackgroundKey = VsBrushes.ScrollBarArrowBackgroundKey;
        public static readonly object ScrollBarArrowBackgroundHoverKey = VsBrushes.ScrollBarArrowMouseOverBackgroundKey;
        public static readonly object ScrollBarArrowBackgroundPressedKey = VsBrushes.ScrollBarArrowPressedBackgroundKey;
        public static readonly object ScrollBarArrowBackgroundDisabledKey = VsBrushes.ScrollBarArrowDisabledBackgroundKey;

#if DEV11_OR_LATER
        public static readonly object SearchGlyphBrushKey = SearchControlColors.SearchGlyphBrushKey;
#else
        public static readonly object SearchGlyphBrushKey = VsBrushes.WindowTextKey;
#endif

        public static readonly BitmapSource UacShield = CreateUacShield();

        private static BitmapSource CreateUacShield() {
            if (Environment.OSVersion.Version.Major >= 6) {
                var sii = new NativeMethods.SHSTOCKICONINFO();
                sii.cbSize = (UInt32)Marshal.SizeOf(typeof(NativeMethods.SHSTOCKICONINFO));

                Marshal.ThrowExceptionForHR(NativeMethods.SHGetStockIconInfo(77, 0x0101, ref sii));
                try {
                    return Imaging.CreateBitmapSourceFromHIcon(
                        sii.hIcon,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                } finally {
                    NativeMethods.DestroyIcon(sii.hIcon);
                }
            } else {
                return Imaging.CreateBitmapSourceFromHIcon(
                    SystemIcons.Shield.Handle,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
        }
    }

    [ValueConversion(typeof(bool), typeof(object))]
    public sealed class IfElseConverter : IValueConverter, IMultiValueConverter {
        public object IfTrue {
            get;
            set;
        }

        public object IfFalse {
            get;
            set;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return (value as bool? == true) ? IfTrue : IfFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return (value == IfTrue);
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return values.All(b => b as bool? == true) ? IfTrue : IfFalse;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(object), typeof(object))]
    public sealed class ComparisonConverter : IValueConverter {
        public object IfTrue {
            get;
            set;
        }

        public object IfFalse {
            get;
            set;
        }

        public ComparisonOperator Operator {
            get;
            set;
        }

        public object SecondOperand {
            get;
            set;
        }

        public enum ComparisonOperator {
            LessThan,
            LessThanOrEqualTo,
            EqualTo,
            NotEqualTo,
            GreaterThan,
            GreaterThanOrEqualTo
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return (Compare(value, Operator, SecondOperand)) ? IfTrue : IfFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }

        private bool Compare(object firstOperand, ComparisonOperator comparisonOperator, object secondOperand) {
            // There may be loss of precision if firstOperand and secondOperand are not of the same type.
            // This is unfortunately unavoidable unless we want to use type-specific converters.

            var firstComparable = firstOperand as IComparable;
            if (firstComparable == null) {
                return false;
            }

            var secondComparable = System.Convert.ChangeType(secondOperand, firstComparable.GetType());
            if (secondComparable == null) {
                return false;
            }

            int result = firstComparable.CompareTo(secondComparable);

            switch (comparisonOperator) {
                case ComparisonOperator.LessThan:
                    return result < 0;
                case ComparisonOperator.LessThanOrEqualTo:
                    return result <= 0;
                case ComparisonOperator.EqualTo:
                    return result == 0;
                case ComparisonOperator.NotEqualTo:
                    return result != 0;
                case ComparisonOperator.GreaterThanOrEqualTo:
                    return result >= 0;
                case ComparisonOperator.GreaterThan:
                    return result > 0;
            }

            throw new ArgumentException(String.Format("Comparison operator {0} not handled", comparisonOperator));
        }
    }
}
