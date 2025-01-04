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

        
        public bool Nypris { get; set; } //Styr om det är ett nytt pris eller inte

        public IngredientPage()
        {
            InitializeComponent();
            DataContext = app;
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
                }

            }
        }

        private void IngredientList_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {          
            ScrollIngrediens.SelectedIndex = _hasDeletedIngredient ? (_selectedindex >0 ? _selectedindex - 1 : _selectedindex) : ScrollIngrediens.SelectedIndex; //Bestämmer vilket item som ska vara valt i listan efter att en ingrediens har raderats.
            _hasDeletedIngredient = false; //Återställer variabeln.
            if (sender is ListView listView)
            {
                if (listView.SelectedItem == null) return;  
                app.ValdIngrediens = (Ingrediens)listView.SelectedItem; //Sätter den valda ingrediensen.
                app.ValtPris = app.ValdIngrediens.PrisLista.Count > 0 ? app.ValdIngrediens.PrisLista[app.ValdIngrediens.PrisLista.Count-1] : new Priser(app.ValdIngrediens.Namn); //Sätter det valda priset.
            }
        }

        private void KollaSiffor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^$|^\d+$"); //Kollar så att det bara går att skriva siffror.
        }

        private void ÖppnaIngrediens_Click(object sender, RoutedEventArgs e)
        {
            NewIngredient newIngredient = new NewIngredient();
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            newIngredient.Owner = mainWindow;
            newIngredient.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            newIngredient.ShowDialog();
        }
        

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
            NewPrice newprice = new NewPrice(app.ValdIngrediens.Namn);
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            newprice.Owner = mainWindow;
            newprice.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            newprice.ShowDialog();

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


        private void Delete_Pris_Click(object sender, RoutedEventArgs e)
        {
            if (app.ValtPris != null)
            {
                int _index = app.ValdIngrediens.PrisLista.IndexOf(app.ValtPris);
                app.ValdIngrediens.PrisLista.Remove(app.ValtPris);
                app.ValtPris = app.ValdIngrediens.PrisLista.Count > 0 ? app.ValdIngrediens.PrisLista[_index > 0 ? _index - 1 : _index] : new Priser(app.ValdIngrediens.Namn);
            }
        }

        private void PrisListview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid datagrid)
            {
                app.ValtPris = (Priser)datagrid.SelectedItem;
            }
        }

        private void FilterTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(app.Ingredienslista);
            view.Filter = obj =>
            {
                if (obj is Ingrediens ingrediens)
                {
                    return ingrediens.Namn.Contains(FilterTextbox.Text, StringComparison.OrdinalIgnoreCase);
                }
                return false;
            };
        }
    }
}
