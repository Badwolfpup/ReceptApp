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
    public partial class IngredientPage : Page
    {
        App app = (App)Application.Current;
        private string _fileextension;
        private int _selectedindex;
        private bool _hasDeletedIngredient;      
        private Ingrediens _tempValdIngrediens { get; set; }

        public bool Nyingrediens { get; set; }
        
        public IngredientPage()
        {
            InitializeComponent();
            DataContext = app;
            FilterTextbox.TextChanged += ((App)Application.Current).TextBox_FilterText_Changed;

        }


        //private void TextBox_FilterText_Changed(object sender, TextChangedEventArgs e)
        //{
        //    ICollectionView view = CollectionViewSource.GetDefaultView(app.Ingredienslista);
        //    view.Filter = FilterMethod;

        //}


        //private bool FilterMethod(object obj) => obj is Ingrediens ingrediens && ingrediens.Namn.Contains(app.IngredientFilterText, StringComparison.OrdinalIgnoreCase);



        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (app.ValdIngrediens != null)
            {
                _selectedindex = ScrollIngrediens.SelectedIndex;
                _hasDeletedIngredient = true;
                app.Ingredienslista.Remove(app.ValdIngrediens);              
                SaveLoad.SaveIngrediens("Ingrediens", app.Ingredienslista);
            }
        }

        private void IngredientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {          
            ScrollIngrediens.SelectedIndex = _hasDeletedIngredient ? (_selectedindex >0 ? _selectedindex - 1 : _selectedindex) : ScrollIngrediens.SelectedIndex;
            _hasDeletedIngredient = false;
            if (sender is ListView listView)
            {
                app.ValdIngrediens = (Ingrediens)listView.SelectedItem;
            }
        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Focus();
                textBox.SelectAll();
                e.Handled = true;
            }
        }



        private void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox textBox && !textBox.IsFocused)
            {
                textBox.Focus();
                textBox.SelectAll();
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

                        app.TempBild = img;
                        BildRuta.Source = app.TempBild;
                        BildRuta.Visibility = Visibility.Visible;
                        BindadBild.Visibility = Visibility.Collapsed;

                        app.HasAddedImage = true;
                        app.HasExtension = true;
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
                    || string.IsNullOrWhiteSpace(NyProtein.Text) || string.IsNullOrWhiteSpace(NySocker.Text)) 
                { 
                    MessageBox.Show("Du måste fylla i alla fält"); 
                    return; 
                }

                string bildnamn = !app.HasExtension ? @"\Bilder\" + NyNamn.Text + ".png" : @"\Bilder\" + NyNamn.Text + _fileextension;
                app.ValdIngrediens.Bild = AppDomain.CurrentDomain.BaseDirectory + bildnamn;
                app.Ingredienslista.Add(app.ValdIngrediens);
                //SaveLoad.AddIngrediensToDB(_main.KlassMedListor.Ingredienslista[_main.KlassMedListor.Ingredienslista.Count - 1]);
                app.Ingredienslista = new ObservableCollection<Ingrediens>(app.Ingredienslista.OrderBy(item => item.Namn));
                Nyingrediens = false;
                ScrollIngrediens.SelectedItem = app.ValdIngrediens;
                app.AddKnapp = "Lägg till";
                ButtonCancelTillIngrediens.Visibility = Visibility.Collapsed;
                ScrollIngrediens.IsEnabled = true;
                FilterTextbox.IsEnabled = true;
                KollCheckBoxIsChecked();
                if (app.HasAddedImage) SaveLoad.KopieraBild(app.TempBild, NyNamn.Text, _fileextension, app.HasExtension);
                
             
                app.HasAddedImage = false;
                SaveLoad.SaveIngrediens("Ingrediens", app.Ingredienslista);
                BildRuta.Source = null;
                BildRuta.Visibility = Visibility.Collapsed;
                BindadBild.Visibility = Visibility.Visible;
            }
            else
            {
                _tempValdIngrediens = app.ValdIngrediens;
                app.AddKnapp = "OK";
                ButtonCancelTillIngrediens.Visibility = Visibility.Visible;
                app.ValdIngrediens = new Ingrediens();
                ScrollIngrediens.IsEnabled = false;
                FilterTextbox.IsEnabled = false;
                KollCheckBoxIsChecked();
                Nyingrediens = true;
                app.ValdIngrediens.Bild = "pack://application:,,,/ReceptApp;component/Bilder/dummybild.png";
            }
        }
        

        public void KollCheckBoxIsChecked()
        {
            CheckBoxGramPerDL.IsChecked = app.ValdIngrediens.GramPerDl > 0;
            CheckBoxGramLiten.IsChecked = app.ValdIngrediens.Liten > 0;
            CheckBoxGramMedel.IsChecked = app.ValdIngrediens.Medel > 0;
            CheckBoxGramStor.IsChecked = app.ValdIngrediens.Stor > 0;
        }

        private void CancelTillIngrediens_Click(object sender, RoutedEventArgs e)
        {
            app.ValdIngrediens = _tempValdIngrediens;
            app.AddKnapp = "Lägg till";
            ButtonCancelTillIngrediens.Visibility = Visibility.Collapsed;
            BildRuta.Source = null;
            BildRuta.Visibility = Visibility.Collapsed;
            BindadBild.Visibility = Visibility.Visible;
            ScrollIngrediens.IsEnabled = true;
            FilterTextbox.IsEnabled = true;
            KollCheckBoxIsChecked();
            Nyingrediens = false;
            app.HasAddedImage = false;
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
                SaveLoad.SaveIngrediens("Ingrediens", app.Ingredienslista);
            }
        }


        private void NyNamn_PreviewNameInput(object sender, KeyEventArgs e)
        {
            if (sender is TextBox textBox && e.Key == Key.Space)
            {
                string newText = textBox.Text.Insert(textBox.SelectionStart, " ");
                e.Handled = Regex.IsMatch(newText, @"^\s+|\s+$");
            }
        }


    }
}
