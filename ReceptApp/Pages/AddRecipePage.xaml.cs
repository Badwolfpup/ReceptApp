using ReceptApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Printing;
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
            FilterTextboxNyttRecept.TextChanged += ((App)Application.Current).TextBox_FilterText_Changed;
            Loaded += AddRecipePage_Loaded;
        }

        private void AddRecipePage_Loaded(object sender, RoutedEventArgs e)
        {
            app.Nyttrecept.BeräknaVärden();
        }


        private void ScrollIngrediensNyttRecept_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ScrollIngrediensNyttRecept.SelectedIndex = _hasDeletedIngredient ? (_selectedindex > 0 ? _selectedindex - 1 : _selectedindex) : ScrollIngrediens.SelectedIndex;
            //var listview = sender as ListView;
            if (sender is ListView listview && listview.SelectedItem is Ingrediens i)
            {
                app.ValdReceptIngrediens = new ReceptIngrediens();
                app.ValdIngrediensIRecept = i;
                ComboBoxMått.SelectedIndex = 0;
            }
        }

        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(TextBoxMått.Text, out int mängd);
            if (mängd > 0)
            {
                if (ScrollIngrediensNyttRecept.SelectedItem is not Ingrediens i || app.Nyttrecept.ReceptIngredienser.Any(x => x.Ingrediens.Namn == app.ValdIngrediensIRecept.Namn))
                {
                    MessageBox.Show("Du har inte valt en ingrediens eller försöker du lägga till en dublett"); return;
                }
                app.ValdReceptIngrediens.Mått = app.ValdReceptIngrediens.KonverteraMåttTillText(ComboBoxMått.Text);
                app.ValdReceptIngrediens.Ingrediens = app.ValdIngrediensIRecept;
                app.Nyttrecept.ReceptIngredienser.Add(app.ValdReceptIngrediens);
                ScrollTillagdaIngredienser.SelectedItem = app.ValdReceptIngrediens;
                app.ValdIngrediensIRecept.ÄrTillagdIRecept = true;
                app.ValdIngrediensIRecept = new Ingrediens();
                app.ValdReceptIngrediens = new ReceptIngrediens();
            }
            else MessageBox.Show("Du behöver ange hur mycket");
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
            i.Ingrediens.ÄrTillagdIRecept = false;
            int index = app.Nyttrecept.ReceptIngredienser.IndexOf(i);
            app.Nyttrecept.ReceptIngredienser.Remove(i);
            app.ValdReceptIngrediens = app.Nyttrecept.ReceptIngredienser.Count == 0 ? app.ValdReceptIngrediens = new ReceptIngrediens() : (index >= app.Nyttrecept.ReceptIngredienser.Count ? app.Nyttrecept.ReceptIngredienser[app.Nyttrecept.ReceptIngredienser.Count - 1] : app.Nyttrecept.ReceptIngredienser[index]);
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
                foreach (var item in app.Nyttrecept.ReceptIngredienser)
                {
                    item.Ingrediens.ÄrTillagdIRecept = false;
                }
                //SaveLoad.SaveRecept("Recept", app.ReceptLista);
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

        private void ScrollTillagdaIngredienser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listview && listview.SelectedItem is ReceptIngrediens i)
            {
                app.ValdReceptIngrediens = i;
                app.ValdIngrediensIRecept = i.Ingrediens;
                //ComboBoxMått.SelectedItem = i.Mått;
                //TextBoxMått.Text = i.Mängd.ToString();
                switch (i.Mått)
                {
                    case "g": ComboBoxMått.SelectedItem = "Gram"; break;
                    case "dl": ComboBoxMått.SelectedItem = "Deciliter"; break;
                    case "msk": ComboBoxMått.SelectedItem = "Matsked"; break;
                    case "tsk": ComboBoxMått.SelectedItem = "Tesked"; break;
                    case "krm": ComboBoxMått.SelectedItem = "Kryddmått"; break;
                    case "st": ComboBoxMått.SelectedItem = "Sttycken"; break;
                    case "stora": ComboBoxMått.SelectedItem = "Antal stor"; break;
                    case "stor": ComboBoxMått.SelectedItem = "Antal stor"; break;
                    case "medelstora": ComboBoxMått.SelectedItem = "Antal medel"; break;
                    case "medelstor": ComboBoxMått.SelectedItem = "Antal medel"; break;
                    case "små": ComboBoxMått.SelectedItem = "Antal liten"; break;
                    case "liten": ComboBoxMått.SelectedItem = "Antal liten"; break;
                }

            }
        }

        private void ComboBoxMått_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox box && box.SelectedItem != null)
            {
                if (app.ValdReceptIngrediens == null) return;   
                app.ValdReceptIngrediens.Mått = app.ValdReceptIngrediens.KonverteraMåttTillText(box.SelectedItem.ToString());

            }
        }

        private void TextBoxMått_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddIngredient_Click(sender, e);
            }
        }
    }
}
