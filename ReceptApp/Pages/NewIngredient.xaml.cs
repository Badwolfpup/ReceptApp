using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ReceptApp.Pages
{
    /// <summary>
    /// Interaction logic for NewIngredient.xaml
    /// </summary>
    public partial class NewIngredient : Window, INotifyPropertyChanged
    {

            
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        public NewIngredient()
        {
            InitializeComponent();
            DataContext = this;
            NyIngrediens = new Ingrediens();
            NyIngrediens.Bild = "pack://application:,,,/ReceptApp;component/Bilder/dummybild.png";
        }

        private Ingrediens _nyingrediens;
        public Ingrediens NyIngrediens
        {
            get { return _nyingrediens; }
            set
            {
                _nyingrediens = value;
                OnPropertyChanged("NyIngrediens");
            }
        }
        App app = (App)Application.Current;
        private string _fileextension; //Håller koll på filändelsen på bilden.
        private bool SkaKopieraBild { get; set; } = true; //Styr om bilden ska kopieras eller inte.
        private bool HasAddedImage { get; set; }
        private bool HasExtension { get; set; }
        private BitmapImage TempBild { get; set; } = new BitmapImage();


        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog()
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Bilder\"
            };
            open.Multiselect = false;
            if (open.ShowDialog() == true)
            {
                string filgenväg = open.FileName;
                string directiryname = System.IO.Path.GetDirectoryName(filgenväg) + "\\";
                if (open.InitialDirectory == System.IO.Path.GetDirectoryName(filgenväg) + "\\")
                {
                    NyNamn.Text = System.IO.Path.GetFileNameWithoutExtension(filgenväg);
                    NyNamn.IsEnabled = false;
                    SkaKopieraBild = false;
                }
                _fileextension = System.IO.Path.GetExtension(filgenväg);
                if (System.IO.Path.GetExtension(filgenväg) == ".jpg" || System.IO.Path.GetExtension(filgenväg) == ".jpeg" || System.IO.Path.GetExtension(filgenväg) == ".png")
                {

                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri(filgenväg);
                    img.EndInit();

                    TempBild = img;
                    BindadBild.Source = TempBild;
                    HasAddedImage = true;
                    HasExtension = true;
                }
            }
        }

        private void KollaSiffor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^$|^\d+$"); //Kollar så att det bara går att skriva siffror.
        }

        private void LäggTillIngrediens_Click(object sender, RoutedEventArgs e)
        {
            //Kollar så att alla fält är ifyllda
            if (string.IsNullOrWhiteSpace(NyNamn.Text) || string.IsNullOrWhiteSpace(NyKalori.Text)
                || string.IsNullOrWhiteSpace(NyFett.Text) || string.IsNullOrWhiteSpace(NyKolhydrat.Text)
                || string.IsNullOrWhiteSpace(NyProtein.Text))
            {
                MessageBox.Show("Du måste fylla i alla fält");
                return;
            }
            NyIngrediens.Namn = NyIngrediens.Namn.Trim(); //Tar bort mellanslag i början och slutet av namnet.
            string bildnamn = !HasExtension ? @"\Bilder\" + NyNamn.Text + ".png" : @"\Bilder\" + NyNamn.Text + _fileextension; //Genvägg till bilden.
            NyIngrediens.Bild = AppDomain.CurrentDomain.BaseDirectory + bildnamn; //Sparar bildens sökväg i Ingrediensobjektet.
            app.Ingredienslista.Add(NyIngrediens); //Lägger till ingrediensen i listan.
            app.FilteredIngredienslista.Add(NyIngrediens.Copy()); //Lägger till ingrediensen i den filtrerade listan.
            app.appdata.Ingredienslista = new ObservableCollection<Ingrediens>(app.Ingredienslista.OrderBy(item => item.Namn)); //Sorterar listan.
            app.Ingredienslista = app.appdata.Ingredienslista;

            if (HasAddedImage) KopieraBild(TempBild, NyNamn.Text, _fileextension, HasExtension); //Kopierar bilden till mappen om man la till en bild .
            app.ValdIngrediens = NyIngrediens; //Sätter den nya ingrediensen som vald.
            Close();
        }

        private void CancelLäggTillIngrediens_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnPasteExecuted(object sender, ExecutedRoutedEventArgs e)
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
                        TempBild = bitmapImage;
                        BindadBild.Source = bitmapImage;
                        HasAddedImage = true;
                        HasExtension = false;
                    }
                }
            }
        }

        public void KopieraBild(BitmapImage img, string filnamn, string fileextension, bool hasExtension)
        {
            if (!SkaKopieraBild)
            {
                SkaKopieraBild = true; return;
            }
            if (!hasExtension) fileextension = ".png";
            filnamn += fileextension;
            string _folderpath = AppDomain.CurrentDomain.BaseDirectory;
            string bildfolder = _folderpath + @"\Bilder\";
            if (!Directory.Exists(bildfolder)) Directory.CreateDirectory(bildfolder);

            string filePath = System.IO.Path.Combine(bildfolder, filnamn);

            var file = Directory.GetFiles(bildfolder, filnamn);
            if (file.Any()) return;
            if (img != null && !string.IsNullOrEmpty(filnamn))
            {
                // Create a new BitmapEncoder
                BitmapEncoder encoder = new PngBitmapEncoder(); // Choose the appropriate encoder based on your requirements

                // Create a new MemoryStream to hold the encoded image data
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Encode the BitmapImage and write the encoded data to the MemoryStream
                    encoder.Frames.Add(BitmapFrame.Create(img));
                    encoder.Save(memoryStream);


                    // Write the encoded data from the MemoryStream to the file
                    File.WriteAllBytes(filePath, memoryStream.ToArray());
                }
            }

        }


    }
}
