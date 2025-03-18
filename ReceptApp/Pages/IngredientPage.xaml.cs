using ReceptApp.Model;
using ReceptApp.ViewModel;
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

        public ObservableCollection<Ingrediens> IngrediensLista => AppData.Instance.IngrediensLista;



        public IngredientPage()
        {
            InitializeComponent();
            DataContext = new VMIngredientPage();
            Loaded += (s, e) => { FilterTextbox.Clear(); FilterTextbox.Focus(); };
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






    }
}
