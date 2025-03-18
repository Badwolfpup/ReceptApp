using ReceptApp.Model;
using ReceptApp.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Windows.Input;

namespace ReceptApp.ViewModel
{
    public class VMRecipePage
    {
        public ObservableCollection<Recept> ReceptLista => AppData.Instance.ReceptLista;

        public ICommand TaBortRecept{ get; set; }
        public ICommand AddAllToCart { get; set; }
        public ICommand EditRecept { get; set; }
        public ICommand LäggTillNyttRecept { get; set; }
        public ICommand FilterText { get; }

        public VMRecipePage()
        {
            TaBortRecept = new RelayCommand(TaBortRecept_Click);
            AddAllToCart = new RelayCommand(AddAllToCart_Click);
            EditRecept = new RelayCommand(EditRecept_Click);
            LäggTillNyttRecept = new RelayCommand(LäggTillNyttRecept_Click);
            FilterText = new RelayCommand(FilterTextboxRecept_TextChanged);
        }

        private void TaBortRecept_Click(object sender)
        {
            if (sender is Button button && button.DataContext is Recept r) ReceptLista.Remove(r);
        }


        private void AddAllToCart_Click(object sender)
        {
            if (sender is ListView listView && listView.SelectedItem is Recept r && r != null)
            {
                if (r == null) return;
                foreach (var item in r.ReceptIngredienser)
                {
                    AppData.Instance.ShoppingListaIngredienser.Add(item.Copy());
                }
                AppData.Instance.ShoppingListaRecept.Add(r.Copy());
            }
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.ContentFrame.Navigate(mainWindow.shoppingList);

        }


        private void EditRecept_Click(object sender)
        {
            if (sender is ListView listView && listView.SelectedItem is Recept r && r != null)
            {
                NewRecipe newrecipe = new NewRecipe(false, r);
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                newrecipe.Owner = mainWindow;
                newrecipe.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                newrecipe.ShowDialog();
            }

        }

        private void LäggTillNyttRecept_Click(object sender)
        {
            if (sender is ListView listView && listView.SelectedItem is Recept r && r != null)
            {
                NewRecipe newrecipe = new NewRecipe(true, new Recept(4));
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                newrecipe.Owner = mainWindow;
                newrecipe.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                newrecipe.ShowDialog();
            }
        }

        private void FilterTextboxRecept_TextChanged(object sender)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(AppData.Instance.ReceptLista);
            var textbox = sender as TextBox;
            if (textbox == null) return;
            view.Filter = obj =>
            {
                if (obj is Recept recept)
                {
                    return recept.Namn.Contains(textbox.Text, StringComparison.OrdinalIgnoreCase);
                }
                return false;
            };
        }
    }
}
