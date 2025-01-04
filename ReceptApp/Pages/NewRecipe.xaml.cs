using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace ReceptApp.Pages
{
    /// <summary>
    /// Interaction logic for NewRecipe.xaml
    /// </summary>
    public partial class NewRecipe : Window, INotifyPropertyChanged
    {
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion


        public NewRecipe(ObservableCollection<Ingrediens> ingredienslista)
        {
            InitializeComponent();
            DataContext = this;
            SkapaFiltreradLista(ingredienslista);
            ValdReceptIngrediens = new ReceptIngrediens();
            NyttRecept = new Recept(4);
            ValdIngrediens = new Ingrediens();  
        }

        public NewRecipe(Recept nyttRecept, ObservableCollection<Ingrediens> ingredienslista)
        {
            InitializeComponent();
            DataContext = this;
            NyttRecept = nyttRecept;
            SkapaFiltreradLista(ingredienslista);
        }

       

        App app = (App)Application.Current;

        private bool _valtreceptingrediens = true;

        private Recept _nyttrecept;
        public Recept NyttRecept
        {
            get { return _nyttrecept; }
            set
            {
                _nyttrecept = value;
                OnPropertyChanged(nameof(NyttRecept));
            }
        }

        private Ingrediens _valdingrediens;
        public Ingrediens ValdIngrediens
        {
            get { return _valdingrediens; }
            set
            {
                _valdingrediens = value;
                OnPropertyChanged(nameof(ValdIngrediens));
            }
        }

        private ReceptIngrediens _valdreceptingrediens;
        public ReceptIngrediens ValdReceptIngrediens
        {
            get { return _valdreceptingrediens; }
            set
            {
                _valdreceptingrediens = value;
                OnPropertyChanged(nameof(ValdReceptIngrediens));
            }
        }

        ObservableCollection<Ingrediens> FilteredIngredientList;

        public ICollectionView FilteredCollectionView { get; set; }


        private void SkapaFiltreradLista(ObservableCollection<Ingrediens> ingredienslista) 
        {
            FilteredIngredientList = new ObservableCollection<Ingrediens>(ingredienslista);
            FilteredCollectionView = CollectionViewSource.GetDefaultView(FilteredIngredientList);
            foreach (var item in FilteredCollectionView) 
            {
                if (item is INotifyPropertyChanged inotify)
                {
                    inotify.PropertyChanged += (s, e) => FilteredCollectionView.Refresh();
                }
            }
            FilteredCollectionView.Filter = FilterPredicate;
        }


        private bool FilterPredicate(object obj)
        {
            if (obj is Ingrediens ingrediens)
            {
                return !ingrediens.ÄrTillagdIRecept;
            }
            return false;
        }

        private void ScrollIngrediensNyttRecept_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (sender is ListView listview && listview.SelectedItem is Ingrediens i)
            {
                if (_valtreceptingrediens) ValdReceptIngrediens = new ReceptIngrediens();
                _valtreceptingrediens = false;
                ValdIngrediens = i;
                ComboBoxMått.SelectedIndex = 0;
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

        private void TextBoxMått_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddIngredient_Click(sender, e);
            }
        }

        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(TextBoxMått.Text, out int mängd);
            if (mängd > 0)
            {
                if (ScrollIngrediensNyttRecept.SelectedItem is not Ingrediens i || app.ValtRecept.ReceptIngredienser.Any(x => x.Ingrediens.Namn == ValdIngrediens.Namn))
                {
                    MessageBox.Show("Du har inte valt en ingrediens eller försöker du lägga till en dublett"); return;
                }
                ValdReceptIngrediens.Mått = ValdReceptIngrediens.KonverteraMåttTillText(ComboBoxMått.Text);
                ValdReceptIngrediens.Ingrediens = ValdIngrediens;
                NyttRecept.ReceptIngredienser.Add(ValdReceptIngrediens);
                ScrollTillagdaIngredienser.SelectedItem = ValdReceptIngrediens;
                ValdIngrediens.ÄrTillagdIRecept = true;
                _valtreceptingrediens = true;
            }
            else MessageBox.Show("Du behöver ange hur mycket");
        }

        private void ScrollTillagdaIngredienser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listview && listview.SelectedItem is ReceptIngrediens i)
            {
                _valtreceptingrediens = true;
                ValdReceptIngrediens = i;
                ValdIngrediens = i.Ingrediens;
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

        private void ListViewItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListViewItem l = sender as ListViewItem;
            if (l != null)
            {
                l.IsSelected = true;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ReceptIngrediens i = ScrollTillagdaIngredienser.SelectedItem as ReceptIngrediens;
            if (i == null) return;
            i.Ingrediens.ÄrTillagdIRecept = false;
            int index = NyttRecept.ReceptIngredienser.IndexOf(i);
            if (index >= 0) NyttRecept.ReceptIngredienser.Remove(i);
            ValdReceptIngrediens = NyttRecept.ReceptIngredienser.Count == 0 ? ValdReceptIngrediens = new ReceptIngrediens() : (index >= NyttRecept.ReceptIngredienser.Count ? NyttRecept.ReceptIngredienser[NyttRecept.ReceptIngredienser.Count - 1] : NyttRecept.ReceptIngredienser[index]);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            if (box == null) return;
            ComboBoxItem item = (ComboBoxItem)box.SelectedItem;
            NyttRecept.Antalportioner = int.Parse(item.Content.ToString());
        }

        private void ComboBoxMått_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox box && box.SelectedItem != null)
            {
                if (ValdReceptIngrediens == null) return;
                ValdReceptIngrediens.Mått = ValdReceptIngrediens.KonverteraMåttTillText(box.SelectedItem.ToString());

            }
        }

        private void Läggtillrecept_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBoxNyReceptNamn.Text))
            {
                if (!app.ReceptLista.Contains(NyttRecept))
                {
                    app.ReceptLista.Add(NyttRecept);
                    app.appdata.ReceptLista = new ObservableCollection<Recept>(app.ReceptLista.OrderBy(item => item.Namn)); //Sorterar listan.
                    app.ReceptLista = app.appdata.ReceptLista;
                }
                Close();

            }
            else MessageBox.Show("Du behöver ange ett namn på receptet");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void FilterTextboxNyttRecept_TextChanged(object sender, TextChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(FilteredCollectionView);
            view.Filter = obj =>
            {
                if (obj is Ingrediens ingrediens)
                {
                    return ingrediens.Namn.Contains(FilterTextboxNyttRecept.Text, StringComparison.OrdinalIgnoreCase);
                }
                return false;
            };
        }
    }
}
