using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ReceptApp
{
    public class ListClass: INotifyPropertyChanged
    {
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion
        #region Properties

        private ObservableCollection<Ingrediens>? _ingredienslista;
        private Ingrediens _valdingrediens;
        private Ingrediens _valdLäggTillIRecptIngrediens;
        private Recept _nyttrecept;
        private ObservableCollection<Recept>? _receptlista;
        private Recept _valtrecept;
        private ObservableCollection<ReceptIngrediens>? _shoppingingredienser;
        private int _antalportioner = 4;
        private string _addKnapp = "Lägg till";
        private string _ingredientfiltertext = string.Empty;
        private string _addrecipefiltertext = string.Empty;
        private string _recipefiltertext = string.Empty;

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
        #endregion

        public bool HasAddedImage { get; set; }
        public bool HasExtension { get; set; } 
        public BitmapImage TempBild { get; set; } = new BitmapImage();
        public ListClass()
        {
           
            Ingredienslista = SaveLoad.LoadIngrediens("Ingrediens");
            //ReceptLista = SaveLoad.LoadReceptFromDB();
            ReceptLista = SaveLoad.LoadRecept("Recept");
            ShoppingIngredienser = new ObservableCollection<ReceptIngrediens>(); 
            Antalportioner = 4;
            if (Ingredienslista.Count != 0) ValdIngrediens = Ingredienslista[0]; else { ValdIngrediens = new Ingrediens(); AddImageSource(); }
            Nyttrecept = new Recept(Antalportioner);
            if (ReceptLista.Count > 0) ValtRecept = ReceptLista[0]; else ValtRecept = new Recept(4);
        }

        private void AddImageSource()
        {
            ValdIngrediens.Bild = "pack://application:,,,/ReceptApp;component/Bilder/dummybild.png";
        }
    }
}
