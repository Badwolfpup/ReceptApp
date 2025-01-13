using ReceptApp.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ReceptApp.Pages
{
    /// <summary>
    /// Interaction logic for ChangePrice.xaml
    /// </summary>
    public partial class ChangePrice : Window, INotifyPropertyChanged
    {
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        public ChangePrice(ObservableCollection<Priser> prislista, Priser valtpris)
        {
            InitializeComponent();
            ValtPris = valtpris;
            DataContext = prislista;
            _initialtpris = valtpris;
        }

        App app = (App)Application.Current;

        private Priser _initialtpris; //håller det ursprungliga priset

        private Priser _valtpris;
        public Priser ValtPris
        {
            get { return _valtpris; }
            set
            {
                _valtpris = value;
                OnPropertyChanged(nameof(ValtPris));
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                if (dataGrid.SelectedItem is Priser pris)
                {
                    ValtPris = pris;
                }
            }
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (ValtPris != null)
            {
                DataGrid dataGrid = sender as DataGrid;
                dataGrid.SelectedItem = ValtPris;
                //dataGrid.ScrollIntoView(ValtPris);
            }
        }

        private void VäljNyttPris_Click(object sender, RoutedEventArgs e)
        {
            if (ValtPris != _initialtpris)
            {
                if (app.PriserIShoppingList.Count >= 0)
                {
                    //var olditem = app.PriserIShoppingList.First(x => x.Namn == _valtpris.Namn);
                    var index = app.PriserIShoppingList.IndexOf(_initialtpris);
                    app.PriserIShoppingList[index] = ValtPris;
                    var hittaRecept = app.ShoppingIngredienser.FirstOrDefault(r => r.ReceptIngredienser.Any(x => x.Ingrediens.PrisLista.Contains(_valtpris)));
                    if (hittaRecept != null)
                    {
                        var prislista = hittaRecept.ReceptIngredienser.FirstOrDefault(x => x.Ingrediens.PrisLista.Contains(_valtpris));
                        if (prislista != null)
                        {
                            var receptmängd = prislista.AntalGram * 100;
                            var prismängd = app.PriserIShoppingList[index].Mängd;
                            prismängd = KonverteraPrismängd(app.PriserIShoppingList[index].Mått, prismängd, index, prislista);

                            app.PriserIShoppingList[index].AntalProdukter = (int)Math.Ceiling((decimal)(receptmängd / prismängd));
                            app.PriserIShoppingList[index].Summa = app.PriserIShoppingList[index].AntalProdukter > 1 ? app.PriserIShoppingList[index].AntalProdukter * app.PriserIShoppingList[index].Pris : app.PriserIShoppingList[index].Pris;
                        }
                    }
                    app.TotalSumma = (double)app.PriserIShoppingList.Sum(x => x.Summa);
                }
            }
            Close();
        }

        private void Avbryt_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private double? KonverteraPrismängd(string mått, double? prismängd, int index, ReceptIngrediens prislista)
        {
            if (app.PriserIShoppingList[index].Mått == "kg") { return prismängd *= 1000; }
            else if (app.PriserIShoppingList[index].Mått == "dl") { return prismängd *= prislista.Ingrediens.GramPerDl / 100; }
            else if (app.PriserIShoppingList[index].Mått == "l") { return prismängd *= prislista.Ingrediens.GramPerDl / 100 * 10; }
            return prismängd;
        }


    }
}
