using ReceptApp.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ReceptApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

    public partial class App : Application, INotifyPropertyChanged
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
            Ingredienslista = SaveLoad.LoadIngrediens("Ingrediens");
            ReceptLista = SaveLoad.LoadRecept("Recept");
            ShoppingIngredienser = new ObservableCollection<ReceptIngrediens>();
            ValdReceptIngrediens = new ReceptIngrediens();
            if (Ingredienslista != null && Ingredienslista.Count != 0) ValdIngrediens = Ingredienslista[0]; else { ValdIngrediens = new Ingrediens(); AddImageSource(); }
            Nyttrecept = new Recept(Antalportioner);
            if (ReceptLista!= null && ReceptLista.Count > 0) ValtRecept = ReceptLista[0]; else ValtRecept = new Recept(Antalportioner);
            ValdIngrediensIRecept = new Ingrediens();
        }


        #region Properties
        
        private ObservableCollection<Ingrediens>? _ingredienslista;
        public ObservableCollection<Ingrediens> Ingredienslista
        {
            get { return _ingredienslista; }
            set
            {
                if (value != _ingredienslista)
                {
                    _ingredienslista = value;
                    OnPropertyChanged(nameof(Ingredienslista));
                }
            }
        }

        private ObservableCollection<Recept>? _receptlista;
        public ObservableCollection<Recept> ReceptLista
        {
            get { return _receptlista; }
            set
            {
                if (value != _receptlista)
                {
                    _receptlista = value;
                    OnPropertyChanged(nameof(ReceptLista));
                }
            }
        }

        private ObservableCollection<ReceptIngrediens>? _shoppingingredienser;
        public ObservableCollection<ReceptIngrediens> ShoppingIngredienser
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

        private string _addKnapp = "Lägg till";
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
        #endregion

        public bool HasAddedImage { get; set; }
        public bool HasExtension { get; set; }
        public BitmapImage TempBild { get; set; } = new BitmapImage();


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
}
