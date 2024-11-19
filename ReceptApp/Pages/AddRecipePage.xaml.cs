using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for AddRecipePage.xaml
    /// </summary>
    public partial class AddRecipePage : Page, INotifyPropertyChanged
    {
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        private string _filtertext = "test";
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

        public ListClass AllLists { get; }

        //public AddRecipePage()
        //{
        //    InitializeComponent();
        //    DataContext = this;
        //}

        public AddRecipePage(ListClass allLists)
        {
            InitializeComponent();
            AllLists = allLists;
            DataContext = AllLists;
            
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

        private void ScrollIngrediensNyttRecept_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ScrollIngrediensNyttRecept.SelectedIndex = _hasDeletedIngredient ? (_selectedindex > 0 ? _selectedindex - 1 : _selectedindex) : ScrollIngrediens.SelectedIndex;
            var listview = sender as ListView;
            if (listview != null)
            {
                AllLists.ValdIngrediens = (Ingrediens)listview.SelectedItem;
            }
        }
    }
}
