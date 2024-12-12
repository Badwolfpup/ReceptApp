﻿using ReceptApp.Pages;
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
            if (Ingredienslista.Count != 0) ValdIngrediens = Ingredienslista[0]; else { ValdIngrediens = new Ingrediens(); AddImageSource(); }
            Nyttrecept = new Recept(Antalportioner);
            if (ReceptLista.Count > 0) ValtRecept = ReceptLista[0]; else ValtRecept = new Recept(Antalportioner);
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

        private Ingrediens _valdLäggTillIRecptIngrediens;
        public Ingrediens ValdLäggTillIRecptIngrediens
        {
            get => _valdLäggTillIRecptIngrediens;
            set
            {
                if (_valdLäggTillIRecptIngrediens != value)
                {
                    _valdLäggTillIRecptIngrediens = value;
                    OnPropertyChanged(nameof(ValdLäggTillIRecptIngrediens));
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

        public bool FilterMethod(object obj)
        {
            if (obj is Ingrediens ingrediens)
            {
                return ingrediens.Namn.Contains(IngredientFilterText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
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
}
