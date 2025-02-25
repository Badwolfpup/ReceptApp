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

        public ChangePrice(ObservableCollection<Vara> varulista, Vara vara, ReceptIngrediens r)
        {
            InitializeComponent();
            ValdVara = vara;
            ValdIngrediens = r;
            DataContext = varulista;
            _initialvara = ValdVara;
        }

        App app = (App)Application.Current;

        private Vara _initialvara; //håller det ursprungliga priset

        private Vara _valdvara;
        public Vara ValdVara
        {
            get { return _valdvara; }
            set
            {
                _valdvara = value;
                OnPropertyChanged(nameof(ValdVara));
            }
        }

        private ReceptIngrediens _valdingrediens;
        public ReceptIngrediens ValdIngrediens
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

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                if (dataGrid.SelectedItem is Vara vara)
                {
                    ValdVara = vara;
                }
            }
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (ValdVara != null)
            {
                DataGrid dataGrid = sender as DataGrid;
                dataGrid.SelectedItem = ValdVara;
                //dataGrid.ScrollIntoView(ValtPris);
            }
        }

        private void VäljNyttPris_Click(object sender, RoutedEventArgs e)
        {
            if (ValdVara != _initialvara)
            {
                ValdIngrediens.Vara = ValdVara;
            }
            Close();
        }

        private void Avbryt_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private double? KonverteraPrismängd(string mått, double? prismängd, int index, ReceptIngrediens prislista)
        {
            //if (app.PriserIShoppingList[index].Mått == "kg") { return prismängd *= 1000; }
            //else if (app.PriserIShoppingList[index].Mått == "dl") { return prismängd *= prislista.Ingrediens.GramPerDl; }
            //else if (app.PriserIShoppingList[index].Mått == "L") { return prismängd *= prislista.Ingrediens.GramPerDl * 10; }
            return prismängd;
        }


    }
}
