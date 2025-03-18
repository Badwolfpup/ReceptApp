using ReceptApp.Model;
using ReceptApp.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

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

        public NewRecipe(bool nyttrecept, Recept recept)
        {
            InitializeComponent();
            NyttRecept = recept;

            KnappText = nyttrecept ? "Lägg till recept" : "Spara ändringar";
            DataContext = this;
        }

        private Recept _nyttrecept;
        public Recept NyttRecept
        {
            get { return _nyttrecept; }
            set
            {
                if (_nyttrecept != value)
                {
                    _nyttrecept = value;
                    OnPropertyChanged(nameof(NyttRecept));
                }
            }
        }

        public ObservableCollection<Ingrediens> IngrediensLista => AppData.Instance.IngrediensLista;

        private string _knapptext;
        public string KnappText
        {
            get { return _knapptext; }
            set
            {
                _knapptext = value;
                OnPropertyChanged(nameof(KnappText));
            }
        }


        private bool _valtreceptingrediens = true;
        private ListView _lastSelectedListView;


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


        private void ScrollIngrediensNyttRecept_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (sender is ListView listview && listview.SelectedItem is Vara i)
            {
                if (_valtreceptingrediens) ValdReceptIngrediens = new ReceptIngrediens();
                _valtreceptingrediens = false;
                ValdVara = i;
                //if (comb is ComboBox ComboBoxMått)
                //{
                    ComboBoxMått.SelectedIndex = 0;
                //}
            }
        }

        private void TextBoxMått_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox box)
            {
                string newText = box.Text.Insert(box.SelectionStart, e.Text);

                e.Handled = !Regex.IsMatch(newText, @"^\d+(\,\d{0,2})?$");
            }
        }

        private void TextBoxMått_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddIngredient_Click(sender, null);
            }
        }



        private void ScrollTillagdaIngredienser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listview && listview.SelectedItem is ReceptIngrediens i)
            {
                _valtreceptingrediens = true;
                ValdReceptIngrediens = i;
                ValdVara = i.Vara;
                //if ( is ComboBox ComboBoxMått)
                //{
                    switch (i.Mått)
                    {
                        case "g": ComboBoxMått.SelectedItem = "Gram"; break;
                        case "dl": ComboBoxMått.SelectedItem = "Deciliter"; break;
                        case "msk": ComboBoxMått.SelectedItem = "Matsked"; break;
                        case "tsk": ComboBoxMått.SelectedItem = "Tesked"; break;
                        case "krm": ComboBoxMått.SelectedItem = "Kryddmått"; break;
                        case "st": ComboBoxMått.SelectedItem = "Stycken"; break;
                        case "stora": ComboBoxMått.SelectedItem = "Antal stor"; break;
                        case "stor": ComboBoxMått.SelectedItem = "Antal stor"; break;
                        case "medelstora": ComboBoxMått.SelectedItem = "Antal medel"; break;
                        case "medelstor": ComboBoxMått.SelectedItem = "Antal medel"; break;
                        case "små": ComboBoxMått.SelectedItem = "Antal liten"; break;
                        case "liten": ComboBoxMått.SelectedItem = "Antal liten"; break;
                    }
                //}

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

        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null) return;

            int.TryParse(TextBoxMått.Text, out int mängd);
            if (mängd > 0)
            {
                if (_lastSelectedListView.SelectedItem is not Vara i || NyttRecept.ReceptIngredienser.Any(x => x.Vara.Namn == ValdVara.Namn))
                {
                    MessageBox.Show("Du har inte valt en vara eller försöker du lägga till en dublett"); return;
                }
                ValdReceptIngrediens.Mått = ValdReceptIngrediens.KonverteraMåttTillText(ComboBoxMått.Text);
                ValdReceptIngrediens.Vara = ValdVara;
                ValdReceptIngrediens.BeräknaAntalGram();
                NyttRecept.ReceptIngredienser.Add(ValdReceptIngrediens);
                ScrollTillagdaIngredienser.SelectedItem = ValdReceptIngrediens;
                _valtreceptingrediens = true;
            }
            else MessageBox.Show("Du behöver ange hur mycket");
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ReceptIngrediens i = ScrollTillagdaIngredienser.SelectedItem as ReceptIngrediens;
            if (i == null) return;
            //i.Vara.ÄrTillagdIRecept = false;
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
                if (!AppData.Instance.ReceptLista.Contains(NyttRecept))
                {
                    AppData.Instance.ReceptLista.Add(NyttRecept);
                    var sorted = new ObservableCollection<Recept>(AppData.Instance.ReceptLista.OrderBy(item => item.Namn)); //Sorterar listan.
                    AppData.Instance.ReceptLista.Clear();
                    foreach (var item in sorted)
                    {
                        AppData.Instance.ReceptLista.Add(item);
                    }
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
            ICollectionView view = CollectionViewSource.GetDefaultView(IngrediensLista);
            view.Filter = obj =>
            {
                if (obj is Ingrediens ingrediens)
                {
                    return ingrediens.Namn.Contains(FilterTextboxNyttRecept.Text, StringComparison.OrdinalIgnoreCase);
                }
                return false;
            };
        }

        private void ListView_GotFocus(object sender, RoutedEventArgs e)
        {

            if (_lastSelectedListView != null && _lastSelectedListView != sender)
            {
                _lastSelectedListView.SelectedItem = null;
            }
            _lastSelectedListView = sender as ListView;
        }


    }
}
