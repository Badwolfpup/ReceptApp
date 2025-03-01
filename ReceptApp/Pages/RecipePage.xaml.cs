using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ReceptApp.Pages
{
    /// <summary>
    /// Interaction logic for RecipePage.xaml
    /// </summary>
    public partial class RecipePage : Page
    {
        App app = (App)Application.Current;


        public RecipePage()
        {
            InitializeComponent();
            DataContext = app;
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Recept r = ScrollRecept.SelectedItem as Recept;
            int index = ScrollRecept.SelectedIndex;
            if (r != null)
            {
                app.ReceptLista.Remove(r);
                if (index < app.ReceptLista.Count)
                {
                    ScrollRecept.SelectedIndex = index;
                }
                else
                {
                    ScrollRecept.SelectedIndex = app.ReceptLista.Count > 0 ? index - 1 : 0;
                }
            }
        }


        private void AddAllToCart_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in app.ValtRecept.ReceptIngredienser)
            {
                app.TillagdaVarorShoppingList.Add(item.Copy());
            }
            app.TillagdaReceptShoppingList.Add(app.ValtRecept.Copy());
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.ContentFrame.Navigate(mainWindow.shoppingList);
        }


        private void EditRecept_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                NewRecipe newrecipe = new NewRecipe(app.ValtRecept, app.Ingredienslista);
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                newrecipe.Owner = mainWindow;
                newrecipe.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                newrecipe.ShowDialog();
            }
        }

        private void LäggTillNyttRecept_Click(object sender, RoutedEventArgs e)
        {
            NewRecipe newrecipe = new NewRecipe(app.Ingredienslista);
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            newrecipe.Owner = mainWindow;
            newrecipe.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            newrecipe.ShowDialog();
        }

        private void FilterTextboxRecept_TextChanged(object sender, TextChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(app.ReceptLista);
            view.Filter = obj =>
            {
                if (obj is Recept recept)
                {
                    return recept.Namn.Contains(FilterTextboxRecept.Text, StringComparison.OrdinalIgnoreCase);
                }
                return false;
            };
        }

    }
}
