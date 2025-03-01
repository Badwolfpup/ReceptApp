using ReceptApp.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace ReceptApp.Pages
{
    /// <summary>
    /// Interaction logic for IngredientPage.xaml
    /// </summary>
    public partial class IngredientPage : Page
    {
        App app = (App)Application.Current;

        public IngredientPage()
        {
            InitializeComponent();
            DataContext = app;
        }

        private void LäggTillNyVara_Click(object sender, RoutedEventArgs e)
        {
            NewIngredient newIngredient = new NewIngredient();
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            newIngredient.Owner = mainWindow;
            newIngredient.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            newIngredient.ShowDialog();
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

        private void DataGridCell_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Get the DataGridCell that was clicked
            var cell = sender as DataGridCell;
            if (cell != null)
            {
                // Find the DataGridRow containing this cell
                var row = FindParent<DataGridRow>(cell);
                if (row != null)
                {
                    var rowDataContext = row.DataContext as Vara;
                    NewIngredient newing = new NewIngredient(rowDataContext);
                    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                    newing.Owner = mainWindow;
                    newing.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    newing.ShowDialog();
                }
            }

        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as T;
        }

        private void Delete_price_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Tag is Ingrediens ing && button.DataContext is Vara vara)
                {
                    ing.Varor.Remove(vara);
                }
            }
        }

        private void Delete_ingrediens_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Ingrediens ing)
            {
                if (ing.Varor.Count == 0)
                {                
                    app.Ingredienslista.Remove(ing);
                } else
                {
                    MessageBox.Show("Du kan inte ta bort en ingrediens som har varor kopplade till sig.\nRadera alla vara först.");
                }
            }
        }

        private void AddVaraToCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Tag is Ingrediens ing && button.DataContext is Vara vara)
                {

                        AddSingleVara popup = new AddSingleVara(new ReceptIngrediens(vara, "", 0), vara.ÄrInteLösvikt ? false : true, true);
                        popup.Owner = Application.Current.MainWindow;
                        bool? result = popup.ShowDialog();
                        if (result == true)
                        {
                            app.TillagdaVarorShoppingList.Add(popup.Recept);
                        }
                }
            }
        }
    }
}
