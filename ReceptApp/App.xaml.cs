using Newtonsoft.Json;
using ReceptApp.Model;
using ReceptApp.Pages;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        #endregion

        public App()
        {
            appdata = AppData.Load();
            Ingredienslista = appdata.Ingredienslista;
            //Ingredienslista.CollectionChanged += Ingredienslista_CollectionChanged;
            ReceptLista = appdata.ReceptLista;
            //ReceptLista.CollectionChanged += ReceptLista_CollectionChanged;

            FilteredIngredienslista = new ObservableCollection<Ingrediens>(Ingredienslista);

            ShoppingIngredienser = new ObservableCollection<Recept>();
            ValdReceptIngrediens = new ReceptIngrediens();
            PriserIShoppingList = new ObservableCollection<Priser>();
            ValtPris = new Priser("");
            if (Ingredienslista != null && Ingredienslista.Count != 0) ValdIngrediens = Ingredienslista[0]; else { ValdIngrediens = new Ingrediens(); AddImageSource(); }
            Nyttrecept = new Recept(Antalportioner);
            if (ReceptLista != null && ReceptLista.Count > 0) ValtRecept = ReceptLista[0]; else ValtRecept = new Recept(Antalportioner);
            ValdIngrediensIRecept = new Ingrediens();
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

        public event NotifyCollectionChangedEventHandler? CollectionChanged
        {
            add
            {
                ((INotifyCollectionChanged)Ingredienslista).CollectionChanged += value;
                ((INotifyCollectionChanged)ReceptLista).CollectionChanged += value;
            }

            remove
            {
                ((INotifyCollectionChanged)Ingredienslista).CollectionChanged -= value;
                ((INotifyCollectionChanged)ReceptLista).CollectionChanged -= value;
            }
        }

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

        private ReceptIngrediens _valdreceptingrediens; //Selected receptingrediens i Lägg till recept
        public ReceptIngrediens ValdReceptIngrediens
        {
            get => _valdreceptingrediens;
            set
            {
                if (_valdreceptingrediens != value)
                {
                    _valdreceptingrediens = value;
                    OnPropertyChanged(nameof(ValdReceptIngrediens));
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

        private Ingrediens _valdIngrediensIRecept;
        public Ingrediens ValdIngrediensIRecept
        {
            get => _valdIngrediensIRecept;
            set
            {
                if (_valdIngrediensIRecept != value)
                {
                    _valdIngrediensIRecept = value;
                    OnPropertyChanged(nameof(ValdIngrediensIRecept));
                }
            }
        }

        private Recept _nyttrecept;
        public Recept Nyttrecept //För lägg till recept
        {
            get { return _nyttrecept; }
            set
            {
                if (_nyttrecept != value)
                {
                    _nyttrecept = value;
                    OnPropertyChanged(nameof(Nyttrecept));
                }
            }
        }


        private Recept _valtrecept;
        public Recept ValtRecept //För receptlistan
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

        private int _antalportioner = 4;
        public int Antalportioner
        {
            get { return _antalportioner; }
            set
            {
                if (_antalportioner != value)
                {
                    _antalportioner = value;
                    OnPropertyChanged(nameof(Antalportioner));
                }
            }
        }

        private string _addNyttPrisKnapp = "Lägg till pris";
        public string AddNyttPrisKnapp
        {
            get => _addNyttPrisKnapp;
            set
            {
                if (_addNyttPrisKnapp != value)
                {
                    _addNyttPrisKnapp = value;
                    OnPropertyChanged(nameof(AddNyttPrisKnapp));
                }
            }
        }

        private string _addKnapp = "Ny ingrediens";
        public string AddKnapp
        {
            get => _addKnapp;
            set
            {
                if (_addKnapp != value)
                {
                    _addKnapp = value;
                    OnPropertyChanged(nameof(AddKnapp));
                }
            }
        }

        private string _ingredientfiltertext = string.Empty;
        public string IngredientFilterText
        {
            get => _ingredientfiltertext;
            set
            {
                if (_ingredientfiltertext != value)
                {
                    _ingredientfiltertext = value;
                    OnPropertyChanged(nameof(IngredientFilterText));
                }
            }
        }

        private string _addrecipefiltertext = string.Empty;
        public string AddRecipeFilterText
        {
            get => _addrecipefiltertext;
            set
            {
                if (_addrecipefiltertext != value)
                {
                    _addrecipefiltertext = value;
                    OnPropertyChanged(nameof(AddRecipeFilterText));
                }
            }
        }

        private string _recipefiltertext = string.Empty;
        public string RecipeFilterText
        {
            get => _recipefiltertext;
            set
            {
                if (_recipefiltertext != value)
                {
                    _recipefiltertext = value;
                    OnPropertyChanged(nameof(RecipeFilterText));
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

        public bool SkaKopieraBild { get; set; }

        #endregion

        public bool HasAddedImage { get; set; }
        public bool HasExtension { get; set; }
        public BitmapImage TempBild { get; set; } = new BitmapImage();
        public bool HasChangedData { get; set; }

        private bool FilterPredicate(object obj)
        {
            if (obj is Ingrediens ingrediens)
            {
                return !ingrediens.ÄrTillagdIRecept;
            }
            return false;
        }

        public void KopieraBild(BitmapImage img, string filnamn, string fileextension, bool hasExtension)
        {
            if (!SkaKopieraBild)
            {
                SkaKopieraBild = true; return;
            }
            if (!hasExtension) fileextension = ".png";
            filnamn += fileextension;
            string _folderpath = AppDomain.CurrentDomain.BaseDirectory;
            string bildfolder = _folderpath + @"\Bilder\";
            if (!Directory.Exists(bildfolder)) Directory.CreateDirectory(bildfolder);

            string filePath = Path.Combine(bildfolder, filnamn);

            var file = Directory.GetFiles(bildfolder, filnamn);
            if (file.Any()) return;
            if (img != null && !string.IsNullOrEmpty(filnamn))
            {
                // Create a new BitmapEncoder
                BitmapEncoder encoder = new PngBitmapEncoder(); // Choose the appropriate encoder based on your requirements

                // Create a new MemoryStream to hold the encoded image data
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Encode the BitmapImage and write the encoded data to the MemoryStream
                    encoder.Frames.Add(BitmapFrame.Create(img));
                    encoder.Save(memoryStream);


                    // Write the encoded data from the MemoryStream to the file
                    File.WriteAllBytes(filePath, memoryStream.ToArray());
                }
            }

        }


        public void AddImageSource()
        {
            ValdIngrediens.Bild = "pack://application:,,,/ReceptApp;component/Bilder/dummybild.png";
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

        public void TextBox_FilterText_Changed(object sender, TextChangedEventArgs e)
        {
            string filtertext = "";
            if (sender is TextBox box)
            {
                DependencyObject parent = box as DependencyObject;

                while (parent != null)
                {
                    parent = LogicalTreeHelper.GetParent(parent);
                    if (parent is IngredientPage)
                    {
                        filtertext = IngredientFilterText;
                        break;
                    }
                    else if (parent is AddRecipePage)
                    {
                        filtertext = AddRecipeFilterText;
                        break;
                    }
                    else if (parent is RecipePage)
                    {
                        filtertext = RecipeFilterText;
                        break;
                    }
                }

                if (parent is IngredientPage || parent is AddRecipePage)
                {
                    ICollectionView view = CollectionViewSource.GetDefaultView(Ingredienslista);
                    view.Filter = obj =>
                    {
                        if (obj is Ingrediens ingrediens)
                        {
                            return ingrediens.Namn.Contains(filtertext, StringComparison.OrdinalIgnoreCase);
                        }
                        return false;
                    };
                }
                else if (parent is RecipePage)
                {
                    ICollectionView view = CollectionViewSource.GetDefaultView(ReceptLista);
                    view.Filter = obj =>
                    {
                        if (obj is Recept recept)
                        {
                            return recept.Namn.Contains(filtertext, StringComparison.OrdinalIgnoreCase);
                        }
                        return false;
                    };
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (appdata != null) appdata.SaveAll();
        }
    }


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
                    case "stor": return "Antal stor";
                    case "stora": return "Antal stor";
                    case "medelstor": return "Antal medel";
                    case "medelstora": return "Antal medel";
                    case "liten": return "Antal liten";
                    case "små": return "Antal liten";
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
                    case "Antal stor": return "stor";
                    case "Antal medel": return "medelstor";
                    case "Antal liten": return "liten";
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
            if (value is double doubleValue)
            {
                if (doubleValue.ToString("R").Contains("."))
                {
                    if (doubleValue.ToString().Split('.')[1].Length > 1) return doubleValue.ToString("F2", new CultureInfo("sv-SE"));
                    else return doubleValue.ToString("F1", new CultureInfo("sv-SE"));
                }
                return doubleValue.ToString(new CultureInfo("sv-SE"));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue &&
                double.TryParse(stringValue, NumberStyles.Any, new CultureInfo("sv-SE"), out var result))
            {
                return result;
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


}
