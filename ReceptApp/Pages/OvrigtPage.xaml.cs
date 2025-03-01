using ReceptApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
    /// Interaction logic for OvrigtPage.xaml
    /// </summary>
    public partial class OvrigtPage : Page
    {
        App app = (App)Application.Current;

        public OvrigtPage()
        {
            InitializeComponent();
            DataContext = app;
        }

        private void LäggTillNyVara_Click(object sender, RoutedEventArgs e)
        {
            NewOvrigaVaror newIngredient = new NewOvrigaVaror();
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
                    NewOvrigaVaror newing = new NewOvrigaVaror(rowDataContext);
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

        private void AddVaraToCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Tag is Ingrediens ing && button.DataContext is Vara vara)
                {
                    AddSingleVara popup = new AddSingleVara(new ReceptIngrediens(vara, "", 0), false, false);
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
