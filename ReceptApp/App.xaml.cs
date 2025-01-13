using ReceptApp.Model;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
//using static System.Net.Mime.MediaTypeNames;

namespace ReceptApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

    public partial class App : Application, INotifyPropertyChanged, INotifyCollectionChanged
    {
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        #endregion

        public App()
        {
            appdata = AppData.Load();
            Ingredienslista = appdata.Ingredienslista;
            ReceptLista = appdata.ReceptLista;

            FilteredIngredienslista = new ObservableCollection<Ingrediens>(Ingredienslista);
            ShoppingIngredienser = new ObservableCollection<Recept>();
            PriserIShoppingList = new ObservableCollection<Priser>();
            ValtPris = new Priser("");
            if (Ingredienslista != null && Ingredienslista.Count != 0) ValdIngrediens = Ingredienslista[0]; else { ValdIngrediens = new Ingrediens(); }
            if (ReceptLista != null && ReceptLista.Count > 0) ValtRecept = ReceptLista[0]; else ValtRecept = new Recept(4);
            FilteredIngredientList = CollectionViewSource.GetDefaultView(FilteredIngredienslista);
            FilteredIngredientList.Filter = FilterPredicate;


        }


        private void Ingredienslista_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            appdata.SaveAll();
        }

        private void ReceptLista_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            appdata.SaveAll();
        }

        //public event NotifyCollectionChangedEventHandler? CollectionChanged
        //{
        //    add
        //    {
        //        ((INotifyCollectionChanged)Ingredienslista).CollectionChanged += value;
        //        ((INotifyCollectionChanged)ReceptLista).CollectionChanged += value;
        //    }

        //    remove
        //    {
        //        ((INotifyCollectionChanged)Ingredienslista).CollectionChanged -= value;
        //        ((INotifyCollectionChanged)ReceptLista).CollectionChanged -= value;
        //    }
        //}

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Set culture to Swedish (uses comma for decimals)
            var culture = new CultureInfo("sv-SE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }


        #region Properties
        public AppData appdata { get; set; }

        public List<string> PrisMåttLista { get; } = new List<string> { "g", "kg", "dl", "l" };
        public List<string> PrisFörpackningstypLista { get; } = new List<string> { "", "lösvikt", "st", "tub", "påse", "burk", "förp" };

        private ObservableCollection<Ingrediens>? _ingredienslista;
        public ObservableCollection<Ingrediens> Ingredienslista
        {
            get { return _ingredienslista; }
            set
            {
                if (value != _ingredienslista)
                {
                    if (_ingredienslista != null)
                    {
                        // Detach the event from the old collection
                        _ingredienslista.CollectionChanged -= Ingredienslista_CollectionChanged;
                    }

                    _ingredienslista = value;

                    if (_ingredienslista != null)
                    {
                        // Attach the event to the new collection
                        _ingredienslista.CollectionChanged += Ingredienslista_CollectionChanged;
                    }
                    if (!ReferenceEquals(_ingredienslista, appdata.Ingredienslista)) MessageBox.Show("Listorna är desynchade igen");
                    OnPropertyChanged(nameof(Ingredienslista));
                }
            }
        }

        private ObservableCollection<Ingrediens>? _filteredIngredienslista;
        public ObservableCollection<Ingrediens> FilteredIngredienslista
        {
            get { return _filteredIngredienslista; }
            set
            {
                if (value != _filteredIngredienslista)
                {
                    _filteredIngredienslista = value;
                    OnPropertyChanged(nameof(FilteredIngredienslista));
                }
            }
        }

        public ICollectionView FilteredIngredientList { get; set; }

        private ObservableCollection<Recept>? _receptlista;
        public ObservableCollection<Recept> ReceptLista
        {
            get { return _receptlista; }
            set
            {
                if (value != _receptlista)
                {
                    if (_receptlista != null)
                    {
                        // Detach the event from the old collection
                        _receptlista.CollectionChanged -= Ingredienslista_CollectionChanged;
                    }

                    _receptlista = value;

                    if (_receptlista != null)
                    {
                        // Attach the event to the new collection
                        _receptlista.CollectionChanged += Ingredienslista_CollectionChanged;
                    }
                    OnPropertyChanged(nameof(ReceptLista));
                }
            }
        }

        private ObservableCollection<Recept>? _shoppingingredienser;
        public ObservableCollection<Recept> ShoppingIngredienser
        {
            get => _shoppingingredienser;
            set
            {
                if (_shoppingingredienser != value)
                {
                    _shoppingingredienser = value;
                    OnPropertyChanged(nameof(ShoppingIngredienser));
                }
            }
        }

        private ObservableCollection<ReceptIngrediens> _receptingrediensshoppinglist;
        public ObservableCollection<ReceptIngrediens> ReceptIngrediensShoppingList
        {
            get { return _receptingrediensshoppinglist; }
            set
            {
                if (_receptingrediensshoppinglist != value)
                {
                    _receptingrediensshoppinglist = value;
                    OnPropertyChanged(nameof(ReceptIngrediensShoppingList));
                }
            }
        }

        private ObservableCollection<Priser>? _priserishoppinglist;
        public ObservableCollection<Priser> PriserIShoppingList
        {
            get { return _priserishoppinglist; }
            set
            {
                if (_priserishoppinglist != value)
                {
                    _priserishoppinglist = value;
                    OnPropertyChanged(nameof(PriserIShoppingList));
                }
            }
        }

        private Ingrediens _valdingrediens;
        public Ingrediens ValdIngrediens
        {
            get { return _valdingrediens; }
            set
            {
                if (_valdingrediens != value)
                {
                    _valdingrediens = value;
                    OnPropertyChanged(nameof(ValdIngrediens));
                }
            }
        }


        private Recept _valtrecept;
        public Recept ValtRecept
        {
            get { return _valtrecept; }
            set
            {
                if (_valtrecept != value)
                {
                    _valtrecept = value;
                    OnPropertyChanged(nameof(ValtRecept));
                }
            }
        }

        private Priser _valtpris;
        public Priser ValtPris
        {
            get { return _valtpris; }
            set
            {
                if (_valtpris != value)
                {
                    _valtpris = value;
                    OnPropertyChanged(nameof(ValtPris));
                }
            }
        }

        private double _totalsumma;
        public double TotalSumma
        {
            get => _totalsumma;
            set
            {
                if (_totalsumma != value)
                {
                    _totalsumma = value;
                    OnPropertyChanged(nameof(TotalSumma));
                }
            }
        }


        #endregion


        public bool HasChangedData { get; set; }

        private bool FilterPredicate(object obj)
        {
            if (obj is Ingrediens ingrediens)
            {
                return !ingrediens.ÄrTillagdIRecept;
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
            if (appdata != null) appdata.SaveAll();
        }
    }

    #region Converters
    public class RoundedNumber : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double Number)
            {
                return Math.Round(Number).ToString("0", culture);
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
            string mängditext = mängd % 1 != 0 && mått != "g" ? (Math.Round(mängd * 2, MidpointRounding.AwayFromZero) / 2).ToString("0.0", culture) : mängd.ToString("0", culture);
            return $"{mängditext} {mått} {namn}";
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
            int antalipack = values[2] is int ? (int)values[2] : 0;
            if (typ != "")
            {
                if (antal > 1)
                {
                    switch (typ)
                    {
                        case "påse": return antalipack > 1 ? $"{antalipack}-pack påsar" : "påsar";
                        case "burk": return antalipack > 1 ? $"{antalipack}-pack burkar" : "burkar";
                        case "förp": return antalipack > 1 ? $"{antalipack}-pack förpackningar" : "förpackningar";
                        case "tub": return antalipack > 1 ? $"{antalipack}-pack tuber" : "tuber";
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
    #endregion
}
