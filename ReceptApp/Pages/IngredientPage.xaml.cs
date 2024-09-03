using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReceptApp.Pages
{
    /// <summary>
    /// Interaction logic for IngredientPage.xaml
    /// </summary>
    public partial class IngredientPage : Page, INotifyPropertyChanged
    {
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        private string _filtertext;
        private int _listviewselectedindex;
        private int _selectedindex;
        private bool _hasDeletedIngredient;
        private Ingrediens _valdingrediens;
        private Ingrediens _tempvaldingrediens;

        public string FilterText
        {
            get => _filtertext;
            set
            {
                if (_filtertext != value)
                {
                    _filtertext = value;
                    OnPropertyChanged(nameof(FilterText));
                }
            }
        }
        public int ListviewSelectedIndex
        {
            get => _listviewselectedindex;
            set
            {
                if (_listviewselectedindex != value)
                {
                    _listviewselectedindex = value;
                    OnPropertyChanged(nameof(ListviewSelectedIndex));
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
        public Ingrediens TempValdIngrediens
        {
            get { return _tempvaldingrediens; }
            set
            {
                if (_tempvaldingrediens != value)
                {
                    _tempvaldingrediens = value;
                    OnPropertyChanged(nameof(TempValdIngrediens));
                }
            }
        }

        public ListClass AllLists { get; }
        
        public IngredientPage(ListClass allLists)
        {
            InitializeComponent();           
            AllLists = allLists;
            DataContext = this;
            InitializeValues();
        }

        private void InitializeValues()
        {
            _listviewselectedindex = 0;
            _filtertext = string.Empty;
            ValdIngrediens = AllLists.Ingredienslista[0];
        }

        private void TextBox_FilterText_Changed(object sender, TextChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(AllLists.Ingredienslista);
            view.Filter = FilterMethod;

        }

        private bool FilterMethod(object obj)
        {
            if (obj is Ingrediens ingrediens)
            {
                return ingrediens.Namn.Contains(FilterText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (ValdIngrediens != null)
            {
                _selectedindex = ScrollIngrediens.SelectedIndex;
                _hasDeletedIngredient = true;
                AllLists.Ingredienslista.Remove(ValdIngrediens);

                //SaveLoad.SaveIngrediens("Ingrediens", _main.KlassMedListor.Ingredienslista);
            }
        }

        private void IngredientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {          
            ScrollIngrediens.SelectedIndex = _hasDeletedIngredient ? (_selectedindex >0 ? _selectedindex - 1 : _selectedindex) : ScrollIngrediens.SelectedIndex;
            _hasDeletedIngredient = false;
            var listview = sender as ListView;
            if (listview != null)
            {
                ValdIngrediens = (Ingrediens)listview.SelectedItem;
            }
        }
    }
}
