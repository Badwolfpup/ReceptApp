using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ShoppingList.xaml
    /// </summary>
    public partial class ShoppingList : Page
    {
        App app = (App)Application.Current;




        public ShoppingList()
        {
            InitializeComponent();
            DataContext = app;
            Loaded += ShoppingList_Loaded;
        }

        private void ShoppingList_Loaded(object sender, RoutedEventArgs e)
        {
            AggregeraRceptIngredienser();
        }

        private void AggregeraRceptIngredienser()
        {
            app.ReceptIngrediensShoppingList = new ObservableCollection<ReceptIngrediens>(app.ShoppingIngredienser
                .SelectMany(r => r.ReceptIngredienser)
                .GroupBy(item => new {item.Ingrediens.Namn, item.Mått} )
                .Select(group => new ReceptIngrediens
                {
                    Mått = group.Key.Mått,
                    Ingrediens = group.First().Ingrediens,
                    Mängd = group.Sum(ing => ing.Mängd),
                    AntalGram = group.First().AntalGram,
                })
                .ToList()
            );
        }

        private void DeleteIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.DataContext is ReceptIngrediens i)
                {
                    app.ReceptIngrediensShoppingList.Remove(i);
                }
            }
        }

        private void AddToShoppingCart_Click(object sender, RoutedEventArgs e)
        {
            string clipboard = "";
            //foreach (ReceptIngrediens i in app.ShoppingIngredienser)
            //{
            //    clipboard += $"{i.Mängd}{i.Mått} {i.Ingrediens.Namn}\n";
            //}
            Clipboard.SetText(clipboard);
        }

        private void DeleteRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.DataContext is Recept r)
                {
                    app.ShoppingIngredienser.Remove(r);
                    AggregeraRceptIngredienser();
                }
            }
        }
    }


}
