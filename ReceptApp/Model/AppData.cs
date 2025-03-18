using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;

namespace ReceptApp.Model
{
    public class AppData : INotifyPropertyChanged
    {
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        private static AppData _instance;
        public static AppData Instance => _instance ?? (_instance = Load());
        private AppData() 
        {
            _ingredienslista.CollectionChanged += CollectionChanged;
            _ovrigavarorlista.CollectionChanged += CollectionChanged;
            _receptlista.CollectionChanged += CollectionChanged;
            //_shoppingListaIngredienser.CollectionChanged += CollectionChanged;
            //_shoppingListaRecept.CollectionChanged += CollectionChanged;
        }



        private ObservableCollection<Ingrediens> _ingredienslista = new ObservableCollection<Ingrediens>();
        public ObservableCollection<Ingrediens> IngrediensLista
        {
            get { return _ingredienslista; }
            set
            {
                if (value != _ingredienslista)
                {
                    if (_ingredienslista != null)
                    {
                        // Detach the event from the old collection
                        _ingredienslista.CollectionChanged -= CollectionChanged;
                    }

                    _ingredienslista = value;

                    if (_ingredienslista != null)
                    {
                        // Attach the event to the new collection
                        _ingredienslista.CollectionChanged += CollectionChanged;
                    }
                    //if (!ReferenceEquals(_ingredienslista, appdata.IngrediensLista)) MessageBox.Show("Listorna är desynchade igen");
                    OnPropertyChanged(nameof(IngrediensLista));
                }
            }
        }

        private ObservableCollection<Ingrediens> _ovrigavarorlista = new ObservableCollection<Ingrediens>();
        public ObservableCollection<Ingrediens> Ovrigavarorlista
        {
            get { return _ovrigavarorlista; }
            set
            {
                if (value != _ovrigavarorlista)
                {
                    if (_ovrigavarorlista != null)
                    {
                        // Detach the event from the old collection
                        _ovrigavarorlista.CollectionChanged -= CollectionChanged;
                    }

                    _ovrigavarorlista = value;

                    if (_ovrigavarorlista != null)
                    {
                        // Attach the event to the new collection
                        _ovrigavarorlista.CollectionChanged += CollectionChanged;
                    }
                    //if (!ReferenceEquals(_ovrigavarorlista, appdata.Ovrigavaraorlista)) MessageBox.Show("Listorna är desynchade igen 1");
                    OnPropertyChanged(nameof(Ovrigavarorlista));
                }
            }
        }


        private ObservableCollection<Recept> _receptlista = new ObservableCollection<Recept>();
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
                        _receptlista.CollectionChanged -= CollectionChanged;
                    }

                    _receptlista = value;

                    if (_receptlista != null)
                    {
                        // Attach the event to the new collection
                        _receptlista.CollectionChanged += CollectionChanged;
                    }
                    OnPropertyChanged(nameof(ReceptLista));
                }
            }
        }

        private ObservableCollection<ReceptIngrediens> _shoppingListaIngredienser = new ObservableCollection<ReceptIngrediens>();
        public ObservableCollection<ReceptIngrediens> ShoppingListaIngredienser
        {
            get { return _shoppingListaIngredienser; }
            set
            {
                if (_shoppingListaIngredienser != value)
                {
                    if (_receptlista != null)
                    {
                        // Detach the event from the old collection
                        _shoppingListaIngredienser.CollectionChanged -= CollectionChanged;
                    }

                    _shoppingListaIngredienser = value;

                    if (_shoppingListaIngredienser != null)
                    {
                        // Attach the event to the new collection
                        _shoppingListaIngredienser.CollectionChanged += CollectionChanged;
                    }
                    OnPropertyChanged(nameof(ShoppingListaIngredienser));
                }
            }
        }

        private ObservableCollection<Recept> _shoppingListaRecept = new ObservableCollection<Recept>();
        public ObservableCollection<Recept> ShoppingListaRecept
        {
            get { return _shoppingListaRecept; }
            set
            {
                if (_shoppingListaRecept != value)
                {
                    if (_receptlista != null)
                    {
                        // Detach the event from the old collection
                        _shoppingListaRecept.CollectionChanged -= CollectionChanged;
                    }

                    _shoppingListaRecept = value;

                    if (_shoppingListaIngredienser != null)
                    {
                        // Attach the event to the new collection
                        _shoppingListaRecept.CollectionChanged += CollectionChanged;
                    }
                    OnPropertyChanged(nameof(ShoppingListaRecept));
                }
            }
        }

        private void CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {

            SaveAll();
        }



        public void SaveAll()
        {
            string _folderpath = AppDomain.CurrentDomain.BaseDirectory;
            string _filepath = _folderpath + "Appdata.json";

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                Formatting = Formatting.Indented

            };

            string file = JsonConvert.SerializeObject(this, settings);
            File.WriteAllText(_filepath, file);
        }

        public static AppData Load()
        {

            string _folderpath = AppDomain.CurrentDomain.BaseDirectory;
            string _filepath = _folderpath + "Appdata.json";

            if (!File.Exists(_filepath))
            {
                return new AppData();
            }

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All
            };

            string file = File.ReadAllText(_filepath);
            return !string.IsNullOrWhiteSpace(file) ? JsonConvert.DeserializeObject<AppData>(file, settings) : new AppData();
        }


    }
}
