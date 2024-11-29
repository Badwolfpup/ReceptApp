using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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


        public ListClass AllLists { get; }


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
                return ingrediens.Namn.Contains(AllLists.AddRecipeFilterText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private void ScrollIngrediensNyttRecept_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ScrollIngrediensNyttRecept.SelectedIndex = _hasDeletedIngredient ? (_selectedindex > 0 ? _selectedindex - 1 : _selectedindex) : ScrollIngrediens.SelectedIndex;
            var listview = sender as ListView;
            if (listview != null)
            {
                AllLists.ValdLäggTillIRecptIngrediens = (Ingrediens)listview.SelectedItem;
            }
        }

        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBoxMått.Text))
            {
                Ingrediens i = ScrollIngrediensNyttRecept.SelectedItem as Ingrediens;

                AllLists.Nyttrecept.ReceptIngredienser.Add(new ReceptIngrediens(i, KonverteraMåttTillText(ComboBoxMått.Text), int.Parse(TextBoxMått.Text)));
            }
            else MessageBox.Show("Du behöver ange hur mycket");
        }

        private string KonverteraMåttTillText(string text)
        {
            switch (text)
            {
                case "Gram": return "g";
                case "Deciliter": return "dl";
                case "Matsked": return "msk";
                case "Tesked": return "tsk";
                case "Kryddmått": return "krm";
                case "Antal stor": if (int.Parse(TextBoxMått.Text) > 1) return "stora"; else return "stor";
                case "Antal medel": if (int.Parse(TextBoxMått.Text) > 1) return "medelstora"; else return "medelstor";
                case "Antal liten": if (int.Parse(TextBoxMått.Text) > 1) return "små"; else return "liten";
                default: return "";
            }
        }

        private void TextBoxMått_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d+$");
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ReceptIngrediens i = ScrollTillagdaIngredienser.SelectedItem as ReceptIngrediens;
            if (i == null) return;
            AllLists.Nyttrecept.ReceptIngredienser.Remove(i);
            //SaveLoad.Save("Ingrediens", Ingredienslista);
        }

        private void ListViewItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListViewItem l = sender as ListViewItem;
            if (l != null)
            {
                l.IsSelected = true;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            if (box == null) return;
            ComboBoxItem item = (ComboBoxItem)box.SelectedItem;
            AllLists.Nyttrecept.Antalportioner = int.Parse(item.Content.ToString());
            AllLists.Nyttrecept.BeräknaVärden();
        }

        private void Läggtillrecept_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBoxNyReceptNamn.Text))
            {
                
                AllLists.ReceptLista.Add(AllLists.Nyttrecept);
                SaveLoad.SaveRecept("Recept", AllLists.ReceptLista);
                AllLists.ValtRecept = AllLists.Nyttrecept;
                AllLists.Nyttrecept = new Recept(4);
                ScrollIngrediensNyttRecept.SelectedItem = null;
                TextBoxMått.Text = "";

            }
            else MessageBox.Show("Du behöver ange ett namn på receptet");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            AllLists.Nyttrecept = new Recept(4);
            TextBoxMått.Text = "";
        }
    }
}
