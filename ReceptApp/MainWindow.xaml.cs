
using ReceptApp;
using ReceptApp.Pages;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        App app = (App)Application.Current;

        IngredientPage ingredientPage;
        RecipePage recipepage;
        public ShoppingList shoppingList {  get; set; }
        private DispatcherTimer _timer;


        public MainWindow()
        {
            InitializeComponent();
            ingredientPage = new IngredientPage();
            recipepage = new RecipePage();
            shoppingList = new ShoppingList();
            ContentFrame.Navigate(ingredientPage);
            //_timer = new DispatcherTimer();
            //_timer.Interval = TimeSpan.FromSeconds(5);
            //_timer.Tick += Timer_Tick;
            //_timer.Start();
        }

        //private void Timer_Tick(object? sender, EventArgs e)
        //{
        //    if (app.HasChangedData)
        //    {
        //        app.HasChangedData = false;
        //        SaveLoad.SaveIngrediens("Ingredienser", app.Ingredienslista);
        //        SaveLoad.SaveRecept("Recept", app.ReceptLista);
        //        _timer.Stop();
        //        _timer.Start();
        //    }
        //}

        private void Button_Click_Ingredient(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(ingredientPage);
        }

        private void Button_Click_Recipe(object sender, RoutedEventArgs e)
        {
           // ContentFrame.Navigate(recipepage);
            ContentFrame.Navigate(recipepage);
        }

        private void Button_Click_Inköpslista(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(shoppingList);

        }




    }
}