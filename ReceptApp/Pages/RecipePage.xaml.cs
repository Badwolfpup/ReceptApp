using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
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


        public RecipePage(ListClass allLists)
        {
            InitializeComponent();
            AllLists = allLists;
            DataContext = allLists;
        }

        public ListClass AllLists { get; }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Recept r = ScrollRecept.SelectedItem as Recept;
            if (r != null)
            {
                AllLists.ReceptLista.Remove(r);
                SaveLoad.SaveRecept("Recept", AllLists.ReceptLista);
            }
        }

        private void FilterTextboxRecept_TextChanged(object sender, TextChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(AllLists.Ingredienslista);
            view.Filter = FilterMethod;
        }

        private bool FilterMethod(object obj)
        {
            if (obj is Ingrediens ingrediens)
            {
                return ingrediens.Namn.Contains(AllLists.RecipeFilterText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private void AddAllToCart_Click(object sender, RoutedEventArgs e)
        {
            foreach (ReceptIngrediens i in AllLists.ValtRecept.ReceptIngredienser)
            {
                AllLists.ShoppingIngredienser.Add(i);
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.DataContext is ReceptIngrediens i)
                {
                    AllLists.ShoppingIngredienser.Add(i);
                }
            }
        }
    }
}
