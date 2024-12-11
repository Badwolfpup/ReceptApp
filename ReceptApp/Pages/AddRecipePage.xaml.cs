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

        App app = (App)Application.Current;



        public AddRecipePage()
        {
            InitializeComponent();
            DataContext = app;
            
        }



        private void TextBox_FilterText_Changed(object sender, TextChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(app.Ingredienslista);
            view.Filter = FilterMethod;

        }

        private bool FilterMethod(object obj)
        {
            if (obj is Ingrediens ingrediens)
            {
                return ingrediens.Namn.Contains(app.AddRecipeFilterText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private void ScrollIngrediensNyttRecept_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ScrollIngrediensNyttRecept.SelectedIndex = _hasDeletedIngredient ? (_selectedindex > 0 ? _selectedindex - 1 : _selectedindex) : ScrollIngrediens.SelectedIndex;
            //var listview = sender as ListView;
            if (sender is ListView listview)
            {
                app.ValdLäggTillIRecptIngrediens = (Ingrediens)listview.SelectedItem;
            }
        }

        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBoxMått.Text))
            {
                Ingrediens i = ScrollIngrediensNyttRecept.SelectedItem as Ingrediens;

                app.Nyttrecept.ReceptIngredienser.Add(new ReceptIngrediens(i, KonverteraMåttTillText(ComboBoxMått.Text), int.Parse(TextBoxMått.Text)));
                TextBoxMått.Text = "";
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
            if (sender is TextBox box)
            {
                string newText = box.Text.Insert(box.SelectionStart, e.Text);

                e.Handled = !Regex.IsMatch(newText, @"^\d+(\,\d{0,1})?$");
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ReceptIngrediens i = ScrollTillagdaIngredienser.SelectedItem as ReceptIngrediens;
            if (i == null) return;
            app.Nyttrecept.ReceptIngredienser.Remove(i);
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
            app.Nyttrecept.Antalportioner = int.Parse(item.Content.ToString());
            app.Nyttrecept.BeräknaVärden();
        }

        private void Läggtillrecept_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBoxNyReceptNamn.Text))
            {
                
                app.ReceptLista.Add(app.Nyttrecept);
                SaveLoad.SaveRecept("Recept", app.ReceptLista);
                app.ValtRecept = app.Nyttrecept;
                app.Nyttrecept = new Recept(4);
                ScrollIngrediensNyttRecept.SelectedItem = null;
                TextBoxMått.Text = "";

            }
            else MessageBox.Show("Du behöver ange ett namn på receptet");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            app.Nyttrecept = new Recept(4);
            TextBoxMått.Text = "";
        }
    }
}
