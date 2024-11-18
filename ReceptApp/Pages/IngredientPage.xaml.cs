using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.IO;
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
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Windows.Threading;

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
        private string _fileextension;
        private int _listviewselectedindex;
        private int _selectedindex;
        private bool _hasDeletedIngredient;
        private string _addKnapp = "Lägg till";
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
        public string AddKnapp
        {
            get => _addKnapp;
            set
            {
                if (_addKnapp != value)
                {
                    _addKnapp = value;
                    OnPropertyChanged(nameof(AddKnapp));
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
        public bool Nyingrediens { get; set; }
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
            if (AllLists.ValdIngrediens != null)
            {
                _selectedindex = ScrollIngrediens.SelectedIndex;
                _hasDeletedIngredient = true;
                //string filepath = AppDomain.CurrentDomain.BaseDirectory + @"\Bilder\" + AllLists.ValdIngrediens.Namn;
                //if (File.Exists(filepath)) File.Delete(filepath);
                AllLists.Ingredienslista.Remove(AllLists.ValdIngrediens);
                
                SaveLoad.SaveIngrediens("Ingrediens", AllLists.Ingredienslista);
            }
        }

        private void IngredientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {          
            ScrollIngrediens.SelectedIndex = _hasDeletedIngredient ? (_selectedindex >0 ? _selectedindex - 1 : _selectedindex) : ScrollIngrediens.SelectedIndex;
            _hasDeletedIngredient = false;
            var listview = sender as ListView;
            if (listview != null)
            {
                AllLists.ValdIngrediens = (Ingrediens)listview.SelectedItem;
            }
        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox t = sender as TextBox;
            if (t != null)
            {
                t.Focus();
                t.SelectAll();
                e.Handled = true;
            }
        }

        private void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBox t = sender as TextBox;
            if (t != null)
            {
                t.Focus();
                t.SelectAll();
                e.Handled = true;
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Nyingrediens)
            {
                OpenFileDialog open = new OpenFileDialog()
                {
                    InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Bilder\"
                };
                open.Multiselect = false;
                if (open.ShowDialog() == true)
                {
                    string filgenväg = open.FileName;
                    _fileextension = System.IO.Path.GetExtension(filgenväg);
                    if (System.IO.Path.GetExtension(filgenväg) == ".jpg" || System.IO.Path.GetExtension(filgenväg) == ".jpeg" || System.IO.Path.GetExtension(filgenväg) == ".png")
                    {

                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.UriSource = new Uri(filgenväg);
                        img.EndInit();

                        AllLists.TempBild = img;
                        BildRuta.Source = AllLists.TempBild;
                        BildRuta.Visibility = Visibility.Visible;
                        BindadBild.Visibility = Visibility.Collapsed;

                        AllLists.HasAddedImage = true;
                        AllLists.HasExtension = true;
                    }
                }
            }
        }


        private void NyKalori_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d+$");
        }

        private void LäggTillIngrediens_Click(object sender, RoutedEventArgs e)
        {
            if (Nyingrediens)
            {
                if (string.IsNullOrWhiteSpace(NyNamn.Text) || string.IsNullOrWhiteSpace(NyKalori.Text)
                    || string.IsNullOrWhiteSpace(NyFett.Text) || string.IsNullOrWhiteSpace(NyKolhydrat.Text)
                    || string.IsNullOrWhiteSpace(NyProtein.Text) || string.IsNullOrWhiteSpace(NySocker.Text)) { MessageBox.Show("Du måste fylla i alla fält"); return; }
                string bildnamn = !AllLists.HasExtension ? @"\Bilder\" + NyNamn.Text + ".png" : @"\Bilder\" + NyNamn.Text + _fileextension;
                AllLists.ValdIngrediens.Bild = AppDomain.CurrentDomain.BaseDirectory + bildnamn;
                AllLists.Ingredienslista.Add(AllLists.ValdIngrediens);
                //SaveLoad.AddIngrediensToDB(_main.KlassMedListor.Ingredienslista[_main.KlassMedListor.Ingredienslista.Count - 1]);
                AllLists.Ingredienslista = new ObservableCollection<Ingrediens>(AllLists.Ingredienslista.OrderBy(item => item.Namn));
                Nyingrediens = false;
                ScrollIngrediens.SelectedItem = AllLists.ValdIngrediens;
                AddKnapp = "Lägg till";
                ButtonCancelTillIngrediens.Visibility = Visibility.Collapsed;
                ScrollIngrediens.IsEnabled = true;
                FilterTextbox.IsEnabled = true;
                KollCheckBoxIsChecked();
                if (AllLists.HasAddedImage) SaveLoad.KopieraBild(AllLists.TempBild, NyNamn.Text, _fileextension, AllLists.HasExtension);
                
                
                AllLists.HasAddedImage = false;
                SaveLoad.SaveIngrediens("Ingrediens", AllLists.Ingredienslista);
                BildRuta.Source = null;
                BildRuta.Visibility = Visibility.Collapsed;
                BindadBild.Visibility = Visibility.Visible;
            }
            else
            {
                TempValdIngrediens = AllLists.ValdIngrediens;
                AddKnapp = "OK";
                ButtonCancelTillIngrediens.Visibility = Visibility.Visible;
                AllLists.ValdIngrediens = new Ingrediens();
                ScrollIngrediens.IsEnabled = false;
                FilterTextbox.IsEnabled = false;
                KollCheckBoxIsChecked();
                Nyingrediens = true;
                AllLists.ValdIngrediens.Bild = "pack://application:,,,/ReceptApp;component/Bilder/dummybild.png";
            }
        }
        

        public void KollCheckBoxIsChecked()
        {
            if (AllLists.ValdIngrediens.GramPerDl > 0) CheckBoxGramPerDL.IsChecked = true; else CheckBoxGramPerDL.IsChecked = false;
            if (AllLists.ValdIngrediens.Liten > 0) CheckBoxGramLiten.IsChecked = true; else CheckBoxGramLiten.IsChecked = false;
            if (AllLists.ValdIngrediens.Medel > 0) CheckBoxGramMedel.IsChecked = true; else CheckBoxGramMedel.IsChecked = false;
            if (AllLists.ValdIngrediens.Stor > 0) CheckBoxGramStor.IsChecked = true; else CheckBoxGramMedel.IsChecked = false;
        }

        private void CancelTillIngrediens_Click(object sender, RoutedEventArgs e)
        {
            AllLists.ValdIngrediens = TempValdIngrediens;
            AddKnapp = "Lägg till";
            //ButtonLäggTillIngrediens.Content = "Lägg till";
            ButtonCancelTillIngrediens.Visibility = Visibility.Collapsed;
            ScrollIngrediens.IsEnabled = true;
            FilterTextbox.IsEnabled = true;
            KollCheckBoxIsChecked();
            Nyingrediens = false;
            AllLists.HasAddedImage = false;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var control in Tab1Grid2.Children)
            {
                if (control is TextBox textBox)
                {
                    textBox.TextChanged += TextBox_TextChanged;
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                SaveLoad.SaveIngrediens("Ingrediens", AllLists.Ingredienslista);
            }
        }
    }
}
