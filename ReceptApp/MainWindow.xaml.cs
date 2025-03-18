using ReceptApp.Model;
using ReceptApp.Pages;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace ReceptApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;


        #endregion


        IngredientPage ingredientPage;
        RecipePage recipepage;
        OvrigtPage ovrigtPage;
        public ShoppingList shoppingList { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            ingredientPage = new IngredientPage();
            recipepage = new RecipePage();
            shoppingList = new ShoppingList();
            ovrigtPage = new OvrigtPage();
            ContentFrame.Navigate(ingredientPage);

        }


        private void Button_Click_Ingredient(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(ingredientPage);
        }

        private void Button_Click_Övrigt(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(ovrigtPage);

        }

        private void Button_Click_Recipe(object sender, RoutedEventArgs e)
        {
            if (AppData.Instance.ReceptLista.Count > 0)
            {
               foreach(var item in AppData.Instance.ReceptLista)
                {
                    item.BeräknaVärden();
                }
            }
            ContentFrame.Navigate(recipepage);
        }

        private void Button_Click_Inköpslista(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(shoppingList);

        }




    }
}