using ReceptApp.Model;
using ReceptApp.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace ReceptApp.ViewModel
{
    public class VMIngredientPage
    {
        #region Commands
        public ICommand LäggTillNyVara { get; set; }
        public ICommand TaBortVara { get; set; }
        public ICommand AddVaraToCart { get; set; }
        public ICommand TaBortIngrediens { get; set; }
        public ICommand FilterText { get;  }

        #endregion


        public ObservableCollection<Ingrediens> IngrediensLista => AppData.Instance.IngrediensLista;

        public VMIngredientPage()
        {
            LäggTillNyVara = new RelayCommand(LäggTillNyVara_Click);
            TaBortVara = new RelayCommand(TaBortVara_Click);
            AddVaraToCart = new RelayCommand(AddVaraToCart_Click);
            TaBortIngrediens = new RelayCommand(TaBortIngrediens_Click);
            FilterText = new RelayCommand(FilterTextbox_TextChanged);
        }

        private void LäggTillNyVara_Click(object sender)
        {
            NewIngredient newIngredient = new NewIngredient();
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            newIngredient.Owner = mainWindow;
            newIngredient.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            newIngredient.ShowDialog();
        }

        private void TaBortVara_Click(object sender)
        {
            if (sender is Button button)
            {
                if (button.Tag is Ingrediens ing && button.DataContext is Vara vara)
                {
                    ing.Varor.Remove(vara);
                }
            }
        }

        private void AddVaraToCart_Click(object sender)
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
                        AppData.Instance.ShoppingListaIngredienser.Add(popup.ReceptIng);
                    }
                }
            }
        }

        private void TaBortIngrediens_Click(object sender)
        {
            if (sender is Button button && button.DataContext is Ingrediens ing)
            {
                if (ing.Varor.Count == 0)
                {
                    IngrediensLista.Remove(ing);
                }
                else
                {
                    MessageBox.Show("Du kan inte ta bort en ingrediens som har varor kopplade till sig.\nRadera alla vara först.");
                }
            }
        }

        private void FilterTextbox_TextChanged(object sender)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(IngrediensLista);
            var textbox = sender as TextBox;
            if (textbox == null) return;
            view.Filter = obj =>
            {
                if (obj is Ingrediens ingrediens)
                {
                    return ingrediens.Namn.Contains(textbox.Text, StringComparison.OrdinalIgnoreCase);
                }
                return false;
            };
        }

    }
}
