
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
        RecipePage recippage;
        AddRecipePage addRecipePage;
        ListClass AllLists;

        public MainWindow()
        {
            InitializeComponent();
            AllLists = new ListClass();
            ingredientPage = new IngredientPage(AllLists);
            recippage = new RecipePage(AllLists);
            addRecipePage = new AddRecipePage(AllLists);
            ContentFrame.Navigate(ingredientPage);
        }

        private void Button_Click_Ingredient(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(ingredientPage);
        }

        private void Button_Click_Recipe(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(recippage);
        }

        private void Button_Click_AddRecipe(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(addRecipePage);
        }
    }
}