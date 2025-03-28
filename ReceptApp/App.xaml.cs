﻿using ReceptApp.Model;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;
//using static System.Net.Mime.MediaTypeNames;

namespace ReceptApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

    public partial class App : Application
    {


        public App()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Set culture to Swedish (uses comma for decimals)
            var culture = new CultureInfo("sv-SE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        private bool FilterPredicate(object obj)
        {
            if (obj is Ingrediens ingrediens)
            {
                //return !ingrediens.ÄrTillagdIRecept;
            }
            return false;
        }


        public void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Focus();
                textBox.SelectAll();
                e.Handled = true;
            }
        }


        public void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox textBox && !textBox.IsFocused)
            {
                textBox.Focus();
                textBox.SelectAll();
                e.Handled = true;
            }
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AppData.Instance.SaveAll();
        }
    }

    #region Converters
    public class RoundedNumber : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double Number)
            {
                return  Math.Round(Number).ToString("0", culture);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class Round2Decimal : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double Number)
            {
                culture = new CultureInfo("sv-SE");
                return Math.Floor(Number) != Number ? Math.Round(Number, 2).ToString("0.00", culture) : Math.Round(Number, 2).ToString("0", culture);
            }
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MåttOchMängd : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double mängd = values[0] is double ? (double)values[0] : 0;
            string mått = values[1] is string ? (string)values[1] : "";
            string namn = values[2] is string ? (string)values[2] : "";
            string typ = values[3] is string ? (string)values[3] : "";
            string info = values[4] is string ? (string)values[4] : "";
            string mängditext = mängd % 1 != 0 && mått != "g" ? (Math.Round(mängd * 2, MidpointRounding.AwayFromZero) / 2).ToString("0.0", culture) : mängd.ToString("0", culture);
            return $"{mängditext} {mått} {namn} {typ} {info}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class KonverteraMått : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var mått = values[0] is string ? (string)values[0] : "";
            var mängd = values[1] is double ? (double)values[1] : 0;

            switch (mått)
            {
                case "Gram": return "g";
                case "Deciliter": return "dl";
                case "Matsked": return "msk";
                case "Tesked": return "tsk";
                case "Kryddmått": return "krm";
                case "Stycken": return "st";
                case "Antal stor": if (mängd > 1) return "stora"; else return "stor";
                case "Antal medel": if (mängd > 1) return "medelstora"; else return "medelstor";
                case "Antal liten": if (mängd > 1) return "små"; else return "liten";
                default: return "";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { value, Binding.DoNothing };
        }
    }

    public class KonverteraMåttTillText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                switch (text)
                {
                    case "g": return "Gram";
                    case "dl": return "Deciliter";
                    case "msk": return "Matsked";
                    case "tsk": return "Tesked";
                    case "krm": return "Kryddmått";
                    case "st": return "Stycken";
                    //case "stor": return "Antal stor";
                    //case "stora": return "Antal stor";
                    //case "medelstor": return "Antal medel";
                    //case "medelstora": return "Antal medel";
                    //case "liten": return "Antal liten";
                    //case "små": return "Antal liten";
                    default: return text;
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                switch (text)
                {
                    case "Gram": return "g";
                    case "Deciliter": return "dl";
                    case "Matsked": return "msk";
                    case "Tesked": return "tsk";
                    case "Kryddmått": return "krm";
                    case "Stycken": return "st";
                    //case "Antal stor": return "stor";
                    //case "Antal medel": return "medelstor";
                    //case "Antal liten": return "liten";
                    default: return text;
                }
            }
            return value;
        }
    }

    public class KonverteraTillSvenskDecimal : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text) return text;
            if (value is double doubleValue)
            {
                if (doubleValue.ToString("R").Contains("."))
                {
                    if (doubleValue.ToString().Split('.')[1].Length > 1) return doubleValue.ToString("F2", new CultureInfo("sv-SE"));
                    else return doubleValue.ToString("F1", new CultureInfo("sv-SE"));
                } 
                else
                {
                    if (doubleValue % 1 == 0) return doubleValue.ToString("F0", new CultureInfo("sv-SE"));
                    else
                    {
                        string numStr = doubleValue.ToString(CultureInfo.InvariantCulture);
                        int decimalIndex = numStr.IndexOf('.');
                        if (decimalIndex == -1) return doubleValue.ToString("F0", new CultureInfo("sv-SE"));
                        if (numStr.Length - decimalIndex > 2) return doubleValue.ToString("F2", new CultureInfo("sv-SE"));
                        else return doubleValue.ToString("F1", new CultureInfo("sv-SE"));
                    }
                        
                        return doubleValue.ToString("F2", new CultureInfo("sv-SE"));
                }
                return doubleValue.ToString(doubleValue % 1 == 0 ? "F0" : "F2", new CultureInfo("sv-SE"));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                if (stringValue.EndsWith(",") || stringValue.EndsWith(".")) return DependencyProperty.UnsetValue;
                if (double.TryParse(stringValue, NumberStyles.Any, new CultureInfo("sv-SE"), out var result))
                {
                    return result;
                }

            }
            return DependencyProperty.UnsetValue;
        }
    }

    public class NullableIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue && intValue == 0) return string.Empty;

            return value?.ToString() ?? string.Empty; // Convert null to empty string
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
                return null; // Convert empty string back to null

            if (int.TryParse(value.ToString(), out int result))
                return result; // Convert valid numbers

            return DependencyProperty.UnsetValue; // Fallback for invalid input
        }
    }

    public class KonverteraTypTillPlural : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string typ = values[0] is string ? (string)values[0] : "";
            int antal = values[1] is int ? (int)values[1] : 0;
            if (typ != "")
            {
                if (antal > 1)
                {
                    switch (typ)
                    {
                        case "påse": return antal > 1 ? $"påsar" : "påse";
                        case "burk": return antal > 1 ? $"burkar" : "burk";
                        case "förp": return antal > 1 ? $"förpackningar" : "förpackning";
                        case "tub": return antal > 1 ? $"tuber" : "tub";
                        case "flaska": return antal > 1 ? $"flaskor" : "flaska";
                        default: return typ;
                    }
                }
                else return typ;
            }
            return typ;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class KombineraMängdOchMått : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double mängd = values[0] is double ? (double)values[0] : 0;
            string mått = values[1] is string ? (string)values[1] : "";

            return $"{mängd.ToString()}{mått}";
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        
    }

    public class LäggTillKr : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double doubleValue)
            {
                return $"{doubleValue.ToString("0.00", culture)}kr";
            }
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                if (stringValue.EndsWith(" kr"))
                {
                    stringValue = stringValue.Replace("kr", "");
                }
                if (double.TryParse(stringValue, NumberStyles.Any, new CultureInfo("sv-SE"), out var result))
                {
                    return result;
                }
            }
            return DependencyProperty.UnsetValue;
        }
    }

    public class JmfrprisSomText : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double pris = values[0] is double ? (double)values[0] : 0;
            string mått = values[1] is string ? (string)values[1] : "";
            bool ovrigvara = values[2] is bool ? (bool)values[2] : false;

            if (ovrigvara) return null;

            switch (mått)
            {
                case "g": return $"{pris.ToString("0.00", culture)}kr/kg";
                case "kg": return $"{pris.ToString("0.00", culture)}kr/kg";
                case "dl": return $"{pris.ToString("0.00", culture)}kr/L";
                case "msk": return $"{pris.ToString("0.00", culture)}kr/L";
                case "tsk": return $"{pris.ToString("0.00", culture)}kr/L";
                case "krm": return $"{pris.ToString("0.00", culture)}kr/L";
                case "st": return $"{pris.ToString("0.00", culture)}kr/st";
                default: return $"{pris.ToString("0.00", culture)} kr";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class LäggTillKomma : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string namn = values[0] is string ? (string)values[0] : "";
            string info = values[1] is string ? (string)values[1] : "";

            if (!string.IsNullOrEmpty(namn) && !string.IsNullOrEmpty(info))return $"{namn}, {info}"; 
            
            return namn ?? info ?? string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class OmvändBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = value is bool ? (bool)value : false;
            return !boolValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = value is bool ? (bool)value : false;
            return !boolValue;
        }
    }

    public class MängdSomNull : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double mängd = values[0] is double ? (double)values[0] : 0;
            bool ovrig = values[1] is bool ? (bool)values[1] : false;

            return ovrig ? null : mängd.ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ReturneraSomArray : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ( values.Length ==3 )
            {
                var item1 = values[0];
                var item2 = values[1];
                var item3 = values[2];
                return new Tuple<object, object, object>(item1, item2, item3);
            }
            return values;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
