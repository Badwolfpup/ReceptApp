using Microsoft.Win32;
using ReceptApp.Model;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ReceptApp.Pages
{
    /// <summary>
    /// Interaction logic for NewPrice.xaml
    /// </summary>
    //public partial class NewPrice : Window, INotifyPropertyChanged
    //{
    //    #region InotifyPropertyChanged
    //    protected virtual void OnPropertyChanged(string propertyName)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //    }

    //    public event PropertyChangedEventHandler? PropertyChanged;
    //    #endregion

    //    App app = (App)Application.Current;

    //    public NewPrice(string namn)
    //    {
    //        InitializeComponent();
    //        DataContext = this;
    //        NyPris = new Priser(namn);
    //        NyPris.Bild = "pack://application:,,,/ReceptApp;component/Bilder/dummybild.png";
    //    }

    //    public NewPrice(Priser pris, Ingrediens ingrediens)
    //    {
    //        InitializeComponent();
    //        DataContext = this;
    //        NyPris = pris;
    //        IsNewPrice = false;
    //        ValdIngrediens = ingrediens;
    //        if (NyPris.Mått != "") MattCombobox.SelectedItem = NyPris.Mått;
    //        if (NyPris.Förpackningstyp != "") ForpackningsCombobox.SelectedItem = NyPris.Förpackningstyp;
    //        if (NyPris.Förpackningstyp == "Lösvikt")
    //        {
    //            JmfrprisCheckbox.IsChecked = true;
    //            ForpackningsCombobox.IsEnabled = false;
    //            ForpackningsCombobox.SelectedItem = "Lösvikt";
    //        }
    //    }
    //    public Ingrediens ValdIngrediens { get; set; }
    //    public List<string> PrisMåttLista { get; } = new List<string> { "g", "kg", "dl", "L", "st" };
    //    public List<string> PrisFörpackningstypLista { get; } = new List<string> { "", "lösvikt", "stycken", "flaska", "tub", "påse", "burk", "förp" };
    //    private string _fileextension; //Håller koll på filändelsen på bilden.
    //    private string _sparadförpackningsTyp;
    //    private string _sparadMått;
    //    private bool SkaKopieraBild { get; set; } = true; //Styr om bilden ska kopieras eller inte.
    //    private bool HasAddedImage { get; set; }
    //    private bool HasExtension { get; set; }
    //    private bool IsNewPrice { get; set; } = true;
    //    private BitmapImage TempBild { get; set; } = new BitmapImage();

    //    private Priser _nypris;
    //    public Priser NyPris
    //    {
    //        get { return _nypris; }
    //        set
    //        {
    //            _nypris = value;
    //            OnPropertyChanged(nameof(NyPris));
    //        }
    //    }

    //    private void LäggTillPris_Click(object sender, RoutedEventArgs e)
    //    {
    //        if (NyPris.Mängd == 0 || NyPris.Mängd == null)
    //        {
    //            MessageBox.Show("Du måste ange en mängd.");
    //            return;
    //        }
    //        if (NyPris.Pris == 0 || NyPris.Pris == null)
    //        {
    //            MessageBox.Show("Du måste ange ett pris.");
    //            return;
    //        }
    //        if (NyPris.Förpackningstyp == "")
    //        {
    //            MessageBox.Show("Du behöver ange förpackningstyp.");
    //            return;
    //        }
    //        if (NyPris.Mått == "")
    //        {
    //            MessageBox.Show("Du måste ange ett mått"); return;
    //        }

    //        if ((bool)JmfrprisCheckbox.IsChecked)
    //        {
    //            NyPris.JämförelsePris = NyPris.Pris;
    //            NyPris.PrisSomJmfrPris();
    //        }
    //        if (IsNewPrice)
    //        {
    //            string filnamm = $"{NyPris.Namn}{NyPris.Info}{NyPris.Mängd}{NyPris.Mått}{NyPris.Förpackningstyp}{NyPris.Pris.ToString()}";
    //            if (HasAddedImage) KopieraBild(TempBild, $"{filnamm}", _fileextension, HasExtension); //Kopierar bilden till mappen om man la till en bild .
    //            string bildnamn = !HasExtension ? $@"\Bilder\{NyPris.Namn}\{filnamm}.png" : $@"\Bilder\{NyPris.Namn}\{filnamm}{_fileextension}"; //Genvägg till bilden.
    //            NyPris.Bild = AppDomain.CurrentDomain.BaseDirectory + bildnamn; //Sparar bildens sökväg i Ingrediensobjektet.
    //            NyPris.JämförelsePriser();
    //            //app.ValdIngrediens.PrisLista.Add(NyPris);
    //            app.ValtPris = NyPris;
    //        }

    //        Close();
    //    }

    //    private void CancelLäggTillPris_Click(object sender, RoutedEventArgs e)
    //    {
    //        Close();
    //    }

    //    private void Decimal_PreviewTextInput(object sender, TextCompositionEventArgs e)
    //    {
    //        if (sender is TextBox box)
    //        {
    //            string newText = box.Text.Insert(box.SelectionStart, e.Text);

    //            // Allow empty string or a valid decimal number
    //            e.Handled = !Regex.IsMatch(newText, @"^$|^\d+(\,\d{0,2})?$");
    //        }
    //    }

    //    private void KollaSiffor_PreviewTextInput(object sender, TextCompositionEventArgs e)
    //    {
    //        e.Handled = !Regex.IsMatch(e.Text, @"^$|^\d+$"); //Kollar så att det bara går att skriva siffror.
    //    }

    //    private void Förpackningstyp_SelectionChanged(object sender, SelectionChangedEventArgs e)
    //    {
    //        if (sender is ComboBox box && box.SelectedItem.ToString() != "")
    //        {
    //            NyPris.Förpackningstyp = box.SelectedItem.ToString();
    //        }
    //    }

    //    private void Prismått_SelectionChanged(object sender, SelectionChangedEventArgs e)
    //    {
    //        if (sender is ComboBox box && box.SelectedItem.ToString() != "")
    //        {
    //            NyPris.Mått = box.SelectedItem.ToString();
    //        }
    //    }

    //    private void JmfrprisCheckbox_Click(object sender, RoutedEventArgs e)
    //    {
    //        if (sender != null && sender is CheckBox box)
    //        {
    //            if ((bool)box.IsChecked)
    //            {
    //                _sparadförpackningsTyp = NyPris.Förpackningstyp;
    //                _sparadMått = NyPris.Mått;
    //                ForpackningsCombobox.IsEnabled = false;
    //                ForpackningsCombobox.SelectedItem = "lösvikt";
    //                NyPris.Mängd = 1;
    //                MattCombobox.SelectedItem = "kg";
    //            } else
    //            {
    //                ForpackningsCombobox.IsEnabled = true;
    //                ForpackningsCombobox.SelectedItem = _sparadförpackningsTyp;
    //                MattCombobox.SelectedItem = _sparadMått;
    //            }
    //        }
    //    }

    //    #region Hantering av bilder
    //    private void Image_MouseDown(object sender, MouseButtonEventArgs e)
    //    {
    //        OpenFileDialog open = new OpenFileDialog()
    //        {
    //            InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + $@"Bilder\"
    //        };
    //        open.Multiselect = false;
    //        if (open.ShowDialog() == true)
    //        {
    //            string filgenväg = open.FileName;
    //            var OldImage = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + $@"Bilder\").Any(x => x == System.IO.Path.GetDirectoryName(filgenväg));
    //            //string directiryname = System.IO.Path.GetDirectoryName(filgenväg) + "\\";
    //            if (OldImage)
    //            {
    //                //SkaKopieraBild = false;
    //            }
    //            _fileextension = System.IO.Path.GetExtension(filgenväg);
    //            if (System.IO.Path.GetExtension(filgenväg) == ".jpg" || System.IO.Path.GetExtension(filgenväg) == ".jpeg" || System.IO.Path.GetExtension(filgenväg) == ".png")
    //            {

    //                BitmapImage img = new BitmapImage();
    //                img.BeginInit();
    //                img.UriSource = new Uri(filgenväg);
    //                img.EndInit();
    //                TempBild = img;
    //                BindadBild.Source = img;
    //                HasAddedImage = true;
    //                HasExtension = true;
    //            }
    //        }
    //    }

    //    private void OnPasteExecuted(object sender, ExecutedRoutedEventArgs e)
    //    {

    //        if (Clipboard.ContainsImage())
    //        {
    //            BitmapImage bitmapImage = new BitmapImage();

    //            BitmapSource imageSource = Clipboard.GetImage();
    //            if (imageSource != null)
    //            {
    //                using (MemoryStream memoryStream = new MemoryStream())
    //                {
    //                    // Encode BitmapSource to memory stream
    //                    BitmapEncoder encoder = new PngBitmapEncoder(); // Change encoder type if needed
    //                    encoder.Frames.Add(BitmapFrame.Create(imageSource));
    //                    encoder.Save(memoryStream);

    //                    // Set memory stream position to beginning
    //                    memoryStream.Seek(0, SeekOrigin.Begin);
    //                    bitmapImage.BeginInit();
    //                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
    //                    bitmapImage.StreamSource = memoryStream;
    //                    bitmapImage.EndInit();
    //                    TempBild = bitmapImage;
    //                    BindadBild.Source = bitmapImage;
    //                    HasAddedImage = true;
    //                    HasExtension = false;
    //                }
    //            }
    //        }
    //    }

    //    public void KopieraBild(BitmapImage img, string filnamn, string fileextension, bool hasExtension)
    //    {
    //        if (!SkaKopieraBild)
    //        {
    //            SkaKopieraBild = true; return;
    //        }
    //        if (!hasExtension) fileextension = ".png";
    //        filnamn += fileextension;
    //        string _folderpath = AppDomain.CurrentDomain.BaseDirectory;
    //        string bildfolder = $@"{_folderpath}\Bilder\{NyPris.Namn}\";
    //        if (!Directory.Exists(bildfolder)) Directory.CreateDirectory(bildfolder);

    //        string filePath = System.IO.Path.Combine(bildfolder, filnamn);

    //        var file = Directory.GetFiles(bildfolder, filnamn);
    //        if (file.Any()) return;
    //        if (img != null && !string.IsNullOrEmpty(filnamn))
    //        {
    //            // Create a new BitmapEncoder
    //            BitmapEncoder encoder = new PngBitmapEncoder(); // Choose the appropriate encoder based on your requirements

    //            // Create a new MemoryStream to hold the encoded image data
    //            using (MemoryStream memoryStream = new MemoryStream())
    //            {
    //                // Encode the BitmapImage and write the encoded data to the MemoryStream
    //                encoder.Frames.Add(BitmapFrame.Create(img));
    //                encoder.Save(memoryStream);


    //                // Write the encoded data from the MemoryStream to the file
    //                File.WriteAllBytes(filePath, memoryStream.ToArray());
    //            }
    //        }

    //    }
    //    #endregion

    //    private void VisaNäringsvärdeCheckbox_Checked(object sender, RoutedEventArgs e)
    //    {
    //        //if (sender is CheckBox box)
    //        //{
    //        //    if ((bool)box.IsChecked)
    //        //    {
    //        //        ValdIngrediens.CustomNaring = ValdIngrediens.Naring;
    //        //        ValdIngrediens.Naring = new Naringsvarde();
    //        //    }
    //        //    else
    //        //    {
    //        //        ValdIngrediens.Naring = ValdIngrediens.CustomNaring;
    //        //    }
    //        //}
    //    }
    //}
}
