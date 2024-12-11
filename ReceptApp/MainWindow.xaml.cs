
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

        App app = (App)Application.Current;

        IngredientPage ingredientPage;
        RecipePage recipepage;
        AddRecipePage addRecipePage;
        ShoppingList shoppingList;
  


        public MainWindow()
        {
            InitializeComponent();
            ingredientPage = new IngredientPage();
            recipepage = new RecipePage();
            addRecipePage = new AddRecipePage();
            shoppingList = new ShoppingList();
            ContentFrame.Navigate(ingredientPage);

            
        }

        private void Button_Click_Ingredient(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(ingredientPage);
        }

        private void Button_Click_Recipe(object sender, RoutedEventArgs e)
        {
           // ContentFrame.Navigate(recipepage);
            ContentFrame.Navigate(recipepage);
        }

        private void Button_Click_AddRecipe(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(addRecipePage);
        }

        private void Button_Click_Inköpslista(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(shoppingList);

        }

        private void OnPasteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (ingredientPage.Nyingrediens)
            {
                var currentPage = ContentFrame.Content as Page;
                string pageTypeName = "";
                if (currentPage != null) pageTypeName = currentPage.GetType().Name;


                if (pageTypeName == "IngredientPage")
                {
                    if (Clipboard.ContainsImage())
                    {
                        BitmapImage bitmapImage = new BitmapImage();

                        BitmapSource imageSource = Clipboard.GetImage();
                        if (imageSource != null)
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                // Encode BitmapSource to memory stream
                                BitmapEncoder encoder = new PngBitmapEncoder(); // Change encoder type if needed
                                encoder.Frames.Add(BitmapFrame.Create(imageSource));
                                encoder.Save(memoryStream);

                                // Set memory stream position to beginning
                                memoryStream.Seek(0, SeekOrigin.Begin);
                                bitmapImage.BeginInit();
                                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                                bitmapImage.StreamSource = memoryStream;
                                bitmapImage.EndInit();
                                app.TempBild = bitmapImage;
                                //AllLists.ValdIngrediens.Bild = AppDomain.CurrentDomain.BaseDirectory + @"\Bilder\" + NyNamn.Text;
                                ingredientPage.BildRuta.Source = app.TempBild;
                                ingredientPage.BildRuta.Visibility = Visibility.Visible;
                                ingredientPage.BindadBild.Visibility = Visibility.Collapsed;
                                app.HasAddedImage = true;
                                app.HasExtension = false;
                            }
                        }

                    }
                }
            }

        }



    }
}