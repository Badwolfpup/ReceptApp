using Microsoft.Win32;
using ReceptApp.Model;
using System.Collections.ObjectModel;
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
            NyVara = new Vara();
            NyVara.Bild = "pack://application:,,,/ReceptApp;component/Bilder/dummybild.png";
            //IngrediensLista = IngrediensLista;
            ÄrNyvara = false;
        }

        public NewIngredient(Vara vara)
        {
            InitializeComponent();
            DataContext = this;
            NyVara = vara;
            
            ComboBoxNamn.SelectedItem = IngrediensLista.FirstOrDefault(x => x.Varor.Contains(vara));
            MattCombobox.SelectedItem = vara.Mått;
            ForpackningsCombobox.SelectedItem = vara.Förpackningstyp;
            ÄrNyvara = true;
        }

        private Vara _nyvara;
        public Vara NyVara
        {
            get { return _nyvara; }
            set
            {
                _nyvara = value;
                OnPropertyChanged(nameof(NyVara));
            }
        }

        //public App app { get; } = (App)Application.Current;
        private Ingrediens NyIngrediens;
        public ObservableCollection<Ingrediens> IngrediensLista => AppData.Instance.IngrediensLista;
        private string _fileextension; //Håller koll på filändelsen på bilden.
        private bool SkaKopieraBild { get; set; } //Styr om bilden ska kopieras eller inte.
        private bool HasAddedImage { get; set; }
        private bool HasExtension { get; set; }
        private bool ÄrNyvara { get; set; }
        private BitmapImage TempBild { get; set; } = new BitmapImage();
        private string NameText { get; set; }
        private string NewIngredintName { get; set; }
        private string _sparadförpackningsTyp = "";
        private string _sparadMått;
        public List<string> PrisMåttLista { get; } = new List<string> { "g", "kg", "dl", "L", "st" };
        public List<string> PrisFörpackningstypLista { get; } = new List<string> { "", "lösvikt", "stycken", "flaska", "tub", "påse", "burk", "förpackning" };

        private void KollaSiffor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^$|^\d+$"); //Kollar så att det bara går att skriva siffror.
        }

        private void LäggTillIngrediens_Click(object sender, RoutedEventArgs e)
        {
            NameText = NameText.Trim(); //Tar bort mellanslag i början och slutet av namnet.
            NyVara.Namn = NameText;
            
            if (ÄrNyvara){
                if (SkaKopieraBild) KopieraBild(TempBild, $"{NameText}", _fileextension, HasExtension); //Kopierar bilden
                Close();
                return;
            }
            //Kollar så att alla fält är ifyllda
            if (string.IsNullOrWhiteSpace(NameText) || string.IsNullOrWhiteSpace(NyKalori.Text)
                || string.IsNullOrWhiteSpace(NyFett.Text) || string.IsNullOrWhiteSpace(NyKolhydrat.Text)
                || string.IsNullOrWhiteSpace(NyProtein.Text))
            {
                MessageBox.Show("Du måste fylla i alla fält");
                return;
            }
            if (NyVara.Mängd == 0 || NyVara.Mängd == null)
            {
                MessageBox.Show("Du måste ange en mängd.");
                return;
            }
            if (NyVara.Pris == 0 || NyVara.Pris == null)
            {
                MessageBox.Show("Du måste ange ett pris.");
                return;
            }
            if (NyVara.Förpackningstyp == "")
            {
                MessageBox.Show("Du behöver ange förpackningstyp.");
                return;
            }
            if (NyVara.Mått == "")
            {
                MessageBox.Show("Du måste ange ett mått"); return;
            }

            if ((bool)JmfrprisCheckbox.IsChecked)
            {
                NyVara.JämförelsePris = NyVara.Pris;
                NyVara.PrisSomJmfrPris();
            }

            if (NewIngredintName != null && NewIngredintName != "" && NameText != NewIngredintName) IngrediensLista.Remove(IngrediensLista[IngrediensLista.IndexOf(IngrediensLista.FirstOrDefault(i => i.Namn == NewIngredintName))]); //Tar bort nytillagd ingredienstyp, om den inte användes
            IngrediensLista[IngrediensLista.IndexOf(IngrediensLista.FirstOrDefault(i => i.Namn == NameText))].Varor.Add(NyVara); //Lägger till ingrediensen i listan.
            var sorted  = new ObservableCollection<Ingrediens>(IngrediensLista.OrderBy(item => item.Namn)); //Sorterar listan.
            IngrediensLista.Clear();
            foreach (var item in sorted)
            {
                IngrediensLista.Add(item);
            }
            if (HasAddedImage) KopieraBild(TempBild, $"{NameText}", _fileextension, HasExtension); //Kopierar bilden till mappen om man la till en bild .
            Close();
        }

        private void CancelLäggTillIngrediens_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Decimal_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox box)
            {
                string newText = box.Text.Insert(box.SelectionStart, e.Text);

                // Allow empty string or a valid decimal number
                e.Handled = !Regex.IsMatch(newText, @"^$|^\d+(\,\d{0,2})?$");
            }
        }

        private void Prismått_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox box && box.SelectedItem != null && box.SelectedItem.ToString() != "")
            {
                NyVara.Mått = box.SelectedItem.ToString();
            }
        }

        private void JmfrprisCheckbox_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null && sender is CheckBox box)
            {
                if ((bool)box.IsChecked)
                {
                    _sparadförpackningsTyp = NyVara.Förpackningstyp != null ? NyVara.Förpackningstyp : "";
                    _sparadMått = NyVara.Mått;
                    ForpackningsCombobox.SelectedItem = "lösvikt";
                    NyVara.Mängd = 1;
                    MattCombobox.SelectedItem = "kg";
                    NyVara.ÄrInteLösvikt = false;
                }
                else
                {
                    ForpackningsCombobox.SelectedItem = _sparadförpackningsTyp;
                    MattCombobox.SelectedItem = _sparadMått;
                    NyVara.ÄrInteLösvikt = true;
                }
            }
        }

        private void Förpackningstyp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox box && box.SelectedItem.ToString() != null && box.SelectedItem.ToString() != "")
            {
                NyVara.Förpackningstyp = box.SelectedItem.ToString();
            }
        }

        private void NyTypAvIngrediens_Click(object sender, RoutedEventArgs e)
        {
            NamePopupWindow popup = new NamePopupWindow
            {
                Owner = this 
            };

            bool? result = popup.ShowDialog();
            NewIngredintName = popup.EnteredName;
            
            if (result == true)
            {
                if (NyIngrediens != null && IngrediensLista.Any(x => x == NyIngrediens)) IngrediensLista.Remove(NyIngrediens);
                NyIngrediens = new Ingrediens(NewIngredintName);               
                IngrediensLista.Add(NyIngrediens);
                ComboBoxNamn.SelectedItem = NyIngrediens;
            }
        }

        private void ComboBoxNamn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox box && box.SelectedItem != null && box.SelectedItem.ToString() != "")
            {
                var selectedItem = box.SelectedItem as Ingrediens;
                NyIngrediens = selectedItem;
                if (ÄrNyvara)
                {
                    IngrediensLista[IngrediensLista.IndexOf(IngrediensLista.FirstOrDefault(i => i.Varor.Any(x => x.Namn == NyVara.Namn && x.Typ == NyVara.Typ &&  x.Info == NyVara.Info)))].Varor.Remove(NyVara);
                    NyIngrediens.Varor.Add(NyVara);
                }
                NameText = selectedItem.Namn;
            }
        }

        #region Bildhanetering
        private void OnPasteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (ComboBoxNamn.SelectedItem == null || ComboBoxNamn.SelectedItem.ToString() == "")
            {
                MessageBox.Show("Du måste välja en ingrediens först.");
                return;
            }
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
                        SkaKopieraBild = true;
                    }
                }
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ComboBoxNamn.SelectedItem == null ||ComboBoxNamn.SelectedItem.ToString() == "")
            {
                MessageBox.Show("Du måste välja en ingrediens först.");
                return;
            }
            OpenFileDialog open = new OpenFileDialog()
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Bilder\"
            };
            open.Multiselect = false;
            if (open.ShowDialog() == true)
            {
                string filgenväg = open.FileName;
                var OldImage = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + @"Bilder\").Any(x => x == System.IO.Path.GetDirectoryName(filgenväg));
                if (!OldImage)
                {
                    SkaKopieraBild = true;
                }
                _fileextension = System.IO.Path.GetExtension(filgenväg);
                if (System.IO.Path.GetExtension(filgenväg) == ".jpg" || System.IO.Path.GetExtension(filgenväg) == ".jpeg" || System.IO.Path.GetExtension(filgenväg) == ".png")
                {

                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri(filgenväg);
                    img.EndInit();
                    NyVara.Bild = filgenväg;
                    TempBild = img;
                    BindadBild.Source = TempBild;
                    HasAddedImage = true;
                    HasExtension = true;
                }
            }
        }


        public void KopieraBild(BitmapImage img, string filnamn, string fileextension, bool hasExtension)
        {
            if (!SkaKopieraBild)
            {
               return;
            }
            filnamn += !hasExtension ? ".png" : fileextension;
            string _folderpath = AppDomain.CurrentDomain.BaseDirectory;
            string bildfolder = _folderpath + @"\Bilder\" + $@"\{NyVara.Namn}\";
            if (!Directory.Exists(bildfolder)) Directory.CreateDirectory(bildfolder);

            string filePath = System.IO.Path.Combine(bildfolder, filnamn);

            if (File.Exists(filePath)) filePath = filePath.Insert(Path.GetExtension(filePath) == ".jpeg" ? filePath.Length - 5 : filePath.Length - 4, Directory.GetFiles(bildfolder).Count().ToString());
            NyVara.Bild = filePath;
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

        #endregion

        private void EditIngrediens_Click(object sender, RoutedEventArgs e)
        {
            if (NyIngrediens != null)
            {
                NamePopupWindow popup = new NamePopupWindow
                {
                    Owner = this
                };
                popup.EnteredName = NyIngrediens.Namn;
                bool? result = popup.ShowDialog();
                NewIngredintName = popup.EnteredName;
                if (result == true)
                {
                    NyIngrediens.Namn = NewIngredintName;
                    var sortedList = new ObservableCollection<Ingrediens>(IngrediensLista.OrderBy(item => item.Namn));
                    IngrediensLista.Clear();
                    foreach (var item in sortedList)
                    {
                        IngrediensLista.Add(item);
                    }
                    ComboBoxNamn.SelectedItem = NyIngrediens;
                }
            }
        }
    }
}
