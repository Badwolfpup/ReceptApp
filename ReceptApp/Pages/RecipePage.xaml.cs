using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
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
            if (r != null)
            {
                app.ReceptLista.Remove(r);
                SaveLoad.SaveRecept("Recept", app.ReceptLista);
            }
        }

        private void FilterTextboxRecept_TextChanged(object sender, TextChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(app.Ingredienslista);
            view.Filter = FilterMethod;
        }

        private bool FilterMethod(object obj)
        {
            if (obj is Ingrediens ingrediens)
            {
                return ingrediens.Namn.Contains(app.RecipeFilterText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private void AddAllToCart_Click(object sender, RoutedEventArgs e)
        {
            foreach (ReceptIngrediens i in app.ValtRecept.ReceptIngredienser)
            {
                app.ShoppingIngredienser.Add(i);
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.DataContext is ReceptIngrediens i)
                {
                    app.ShoppingIngredienser.Add(i);
                }
            }
        }
    }

    
}
