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
using ReceptApp.Model;

namespace ReceptApp.Pages
{
    /// <summary>
    /// Interaction logic for IngredientPage.xaml
    /// </summary>
    public partial class IngredientPage : Page
    {
        App app = (App)Application.Current;
        private string _fileextension; //Håller koll på filändelsen på bilden.
        private int _selectedindex; //Håller koll på vilken ingrediens som är vald i listan. Används för att behålla det index som är Selected i listan efter att en ingrediens har raderats.
        private bool _hasDeletedIngredient;  //Håller koll på om en ingrediens precis har blivit raderard    
        private Ingrediens _tempValdIngrediens { get; set; } //Sparar temporärt den valda ingrediensen
        private Priser _tempValtPris { get; set; } //Sparar temporärt det valda priset

        public bool Nyingrediens { get; set; } //Styr om det är en ny ingrediens eller inte.
        public bool Nypris { get; set; } //Styr om det är ett nytt pris eller inte

        public IngredientPage()
        {
            InitializeComponent();
            DataContext = app;
            FilterTextbox.TextChanged += ((App)Application.Current).TextBox_FilterText_Changed;

        }




        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

            if (app.ValdIngrediens != null)
            {
                if (MessageBoxResult.OK == MessageBox.Show($"Är du säker på att du vill radera {app.ValdIngrediens.Namn}?", "Radera", MessageBoxButton.OKCancel))
                {
                    _selectedindex = ScrollIngrediens.SelectedIndex; //Sparar indexet på den ingrediensen som ska raderas.
                    _hasDeletedIngredient = true; //Sätter att en ingrediens har raderats.
                    app.Ingredienslista.Remove(app.ValdIngrediens); //Raderar ingrediensen från listan.              
                    //SaveLoad.SaveIngrediens("Ingrediens", app.Ingredienslista); //Sparar om listan med ingredienser.
                }

            }
        }

        private void IngredientList_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {          
            ScrollIngrediens.SelectedIndex = _hasDeletedIngredient ? (_selectedindex >0 ? _selectedindex - 1 : _selectedindex) : ScrollIngrediens.SelectedIndex; //Bestämmer vilket item som ska vara valt i listan efter att en ingrediens har raderats.
            _hasDeletedIngredient = false; //Återställer variabeln.
            if (sender is ListView listView)
            {
                app.ValdIngrediens = (Ingrediens)listView.SelectedItem; //Sätter den valda ingrediensen.
                app.ValtPris = app.ValdIngrediens.PrisLista.Count > 0 ? app.ValdIngrediens.PrisLista[app.ValdIngrediens.PrisLista.Count-1] : new Priser(app.ValdIngrediens.Namn); //Sätter det valda priset.
            }
        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) //Markerar all text i textboxen när man tabbar till den.
        {
            if (sender is TextBox textBox)
            {
                textBox.Focus();
                textBox.SelectAll();
                e.Handled = true;
            }
        }

        private void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e) //Markerar all text i textboxen när man klickar i den, on den inte redan är fokuserad
        {
            if (sender is TextBox textBox && !textBox.IsFocused)
            {
                textBox.Focus();
                textBox.SelectAll();
                e.Handled = true;
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e) //Öppnar en dialogruta för att välja bild.
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
                    if (open.InitialDirectory == System.IO.Path.GetDirectoryName(filgenväg)) SaveLoad.SkaKopieraBild = true;
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


        private void KollaSiffor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^$|^\d+$"); //Kollar så att det bara går att skriva siffror.
        }

        private void LäggTillIngrediens_Click(object sender, RoutedEventArgs e)
        {
            if (Nyingrediens)
            {
                //Kollar så att alla fält är ifyllda
                if (string.IsNullOrWhiteSpace(NyNamn.Text) || string.IsNullOrWhiteSpace(NyKalori.Text)
                    || string.IsNullOrWhiteSpace(NyFett.Text) || string.IsNullOrWhiteSpace(NyKolhydrat.Text)
                    || string.IsNullOrWhiteSpace(NyProtein.Text)) 
                { 
                    MessageBox.Show("Du måste fylla i alla fält"); 
                    return; 
                }
                app.ValdIngrediens.Namn = app.ValdIngrediens.Namn.Trim(); //Tar bort mellanslag i början och slutet av namnet.
                string bildnamn = !app.HasExtension ? @"\Bilder\" + NyNamn.Text + ".png" : @"\Bilder\" + NyNamn.Text + _fileextension; //Genvägg till bilden.
                app.ValdIngrediens.Bild = AppDomain.CurrentDomain.BaseDirectory + bildnamn; //Sparar bildens sökväg i Ingrediensobjektet.
                app.Ingredienslista.Add(app.ValdIngrediens); //Lägger till ingrediensen i listan.
                app.FilteredIngredienslista.Add(app.ValdIngrediens); //Lägger till ingrediensen i den filtrerade listan.
                app.Ingredienslista = new ObservableCollection<Ingrediens>(app.Ingredienslista.OrderBy(item => item.Namn)); //Sorterar listan.
                ÄndraTextOchVisibilityPåKnapparna();
                EnableIngredientListAndFilterTextbox();
                Nyingrediens = false; 
                ScrollIngrediens.SelectedItem = app.ValdIngrediens; //Markerar den nya ingrediensen i listan.                
                if (app.HasAddedImage) SaveLoad.KopieraBild(app.TempBild, NyNamn.Text, _fileextension, app.HasExtension); //Kopierar bilden till mappen om man la till en bild .
                app.HasAddedImage = false; //Återställer variabler (för när man lägger itll en en ny ingrediens igen.
                //SaveLoad.SaveIngrediens("Ingrediens", app.Ingredienslista); //Sparar listan med ingrediesner.
                BildRuta.Source = null; //Nollställer bilden.
                BildRuta.Visibility = Visibility.Collapsed; //Gömmer bilden.
                BindadBild.Visibility = Visibility.Visible; //Visar den bindade bilden.
            }
            else
            {
                _tempValtPris = app.ValtPris; //Sparar det valda priset i en temporär variabel.
                app.ValtPris = new Priser(""); //Skapar ett nytt prisobjekt.
                _tempValdIngrediens = app.ValdIngrediens; //Sparar den valda ingrediensen i en temporär variabel.
                app.ValdIngrediens = new Ingrediens(); //Skapar en ny tom ingrediens.
                ÄndraTextOchVisibilityPåKnapparna();
                EnableIngredientListAndFilterTextbox();
                Nyingrediens = true; //Sätter att det är en ny ingrediens. Relevant för nästa "klick" på knappen.
                app.ValdIngrediens.Bild = "pack://application:,,,/ReceptApp;component/Bilder/dummybild.png"; //Sätter en dummybild.
            }
        }
        

        private void EnableIngredientListAndFilterTextbox()
        {
            if (Nyingrediens)
            {
                ScrollIngrediens.IsEnabled = true; //Gör listan klickbar igen.
                FilterTextbox.IsEnabled = true; //Gör sökfältet klickbart igen.
            }
            else
            {
                ScrollIngrediens.IsEnabled = false; //Gör listan med ingredienser oklickbar.
                FilterTextbox.IsEnabled = false; //Gör sökfältet oklickbart.
            }
        }

        private void ÄndraTextOchVisibilityPåKnapparna()
        {
            if (Nyingrediens)
            {
                app.AddKnapp = "Ny ingrediens";
                ButtonCancelTillIngrediens.Visibility = Visibility.Collapsed;
            }
            else
            {
                app.AddKnapp = "OK";
                ButtonCancelTillIngrediens.Visibility = Visibility.Visible;
            }
        }

        private void CancelTillIngrediens_Click(object sender, RoutedEventArgs e)
        {
            app.ValdIngrediens = _tempValdIngrediens; //Återställer den valda ingrediensen.
            app.AddKnapp = "Ny ingrediens"; //Ändrar tillbaka texten på knappen.
            ButtonCancelTillIngrediens.Visibility = Visibility.Collapsed; //Gömmer cancel-knappen.
            BildRuta.Source = null; //Nollställer bilden.
            BildRuta.Visibility = Visibility.Collapsed; //Gömmer bilden.
            BindadBild.Visibility = Visibility.Visible; //Visar den bindade bilden.
            ÄndraTextOchVisibilityPåKnapparna();
            EnableIngredientListAndFilterTextbox();
            Nyingrediens = false; //Sätter att det inte är en ny ingrediens.
            app.HasAddedImage = false; //Återställer variabler.
            app.ValtPris = _tempValtPris; //Återställer det valda priset.
        }

        //private void Grid_Loaded(object sender, RoutedEventArgs e)
        //{
        //    foreach (var control in Tab1Grid2.Children)
        //    {
        //        if (control is TextBox textBox)
        //        {
        //            textBox.TextChanged += TextBox_TextChanged; //Lägger till eventhanterare för textboxarna i den valda griden.
        //        }
        //    }
        //}

        //private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (sender is TextBox textBox)
        //    {
        //        SaveLoad.SaveIngrediens("Ingrediens", app.Ingredienslista); //Sparar listan med ingredienser varje gång textn ändras.
        //    }
        //}


        private void NyNamn_PreviewNameInput(object sender, KeyEventArgs e) //Kollar att mellanslaget inte är först eller sist i namnet
        {
            if (sender is TextBox textBox && e.Key == Key.Space)
            {
                string newText = textBox.Text.Insert(textBox.SelectionStart, " ");
                e.Handled = Regex.IsMatch(newText, @"^\s+|\s+$");
            }
        }

        private void Add_Price_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                if (Nypris)
                {
                    app.AddNyttPrisKnapp = "Lägg till pris";
                    AddPrisOKButton.Width = 100;
                    Nypris = false;
                    AddPrisCANCELButton.Visibility = Visibility.Collapsed;
                    app.ValdIngrediens.PrisLista.Add(app.ValtPris);
                    //SaveLoad.SaveIngrediens("Ingrediens", app.Ingredienslista); //Sparar om listan med ingredienser.
                }
                else
                {
                    app.AddNyttPrisKnapp = "OK";
                    Nypris = true;
                    AddPrisOKButton.Width = 50;
                    AddPrisCANCELButton.Visibility = Visibility.Visible;
                    _tempValtPris = app.ValtPris;
                    app.ValtPris = new Priser(app.ValdIngrediens.Namn);
                }
            }
            
        }

        private void Decimal_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox box)
            {
                string newText = box.Text.Insert(box.SelectionStart, e.Text);

                // Allow empty string or a valid decimal number
                e.Handled = !Regex.IsMatch(newText, @"^$|^\d+(\,\d{0,2})?$");
            }
        }

        private void Add_Price_Cancel_Click(object sender, RoutedEventArgs e)
        {
            app.AddNyttPrisKnapp = "Lägg till pris";
            AddPrisOKButton.Width = 100;
            AddPrisCANCELButton.Visibility = Visibility.Collapsed;
            app.ValtPris = _tempValtPris;
            Nypris = false;
        }

        private void Delete_Pris_Click(object sender, RoutedEventArgs e)
        {
            if (app.ValtPris != null)
            {
                int _index = app.ValdIngrediens.PrisLista.IndexOf(app.ValtPris);
                app.ValdIngrediens.PrisLista.Remove(app.ValtPris);
                app.ValtPris = app.ValdIngrediens.PrisLista.Count > 0 ? app.ValdIngrediens.PrisLista[_index > 0 ? _index - 1 : _index] : new Priser(app.ValdIngrediens.Namn);
                //SaveLoad.SaveIngrediens("Ingrediens", app.Ingredienslista); //Sparar om listan med ingredienser.
            }
        }

        private void PrisListview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView)
            {
                app.ValtPris = (Priser)listView.SelectedItem;
            }
        }
    }
}
