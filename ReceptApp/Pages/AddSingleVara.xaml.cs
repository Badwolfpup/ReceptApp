using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for AddSingleVara.xaml
    /// </summary>
    public partial class AddSingleVara : Window, INotifyPropertyChanged
    {
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        //public event NotifyCollectionChangedEventHandler? CollectionChanged;
        #endregion
        private bool ÄrIngrediens { get; set; }
        private bool _visaLösVikt;
        private bool _visaSingleVara;
        public bool VisaLösVikt
        {
            get { return _visaLösVikt; }
            set
            {
                if (_visaLösVikt != value)
                {
                    _visaLösVikt = value;
                    OnPropertyChanged(nameof(VisaLösVikt));
                }
            }
        }
        public bool VisaSingleVara
        {
            get { return _visaSingleVara; }
            set
            {
                if (_visaSingleVara != value)
                {
                    _visaSingleVara = value;
                    OnPropertyChanged(nameof(VisaSingleVara));
                }
            }
        }

        public AddSingleVara(ReceptIngrediens r,  bool ärlösvikt, bool äringrediens)
        {
            InitializeComponent();
            Recept = r;
            DataContext = this;
            ÄrIngrediens = äringrediens;
            VisaLösVikt = ärlösvikt;
            VisaSingleVara = !ärlösvikt;
            r.Vara.ÄrÖvrigVara = !ÄrIngrediens;
            if (VisaLösVikt) this.Loaded += (s, e) =>  TextBoxMängd.Focus();
            else this.Loaded += (s, e) => TextBoxAntal.Focus();
        }


        public string EnteredName { get; private set; }
        public ReceptIngrediens Recept { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (VisaLösVikt)
            {
                if (VisaLösVikt && ComboBoxMåttNamn.SelectedItem == null || ComboBoxMåttNamn.SelectedItem.ToString() == "")
                {
                    MessageBox.Show("Du behöver välja ett mått");
                    return;
                }

                if (VisaLösVikt && Recept.Mängd == null || Recept.Mängd <= 0)
                {
                    MessageBox.Show("Du behöver ange mängd");
                    return;
                }
                Recept.Mått = Recept.KonverteraMåttTillText(ComboBoxMåttNamn.Text);
            }
            else
            {
                if (VisaSingleVara && Recept.AntalProdukter == null || Recept.AntalProdukter <= 0)
                {
                    MessageBox.Show("Du behöver ange antal produkter");
                    return;
                }
                if (!Recept.Vara.ÄrÖvrigVara)
                {
                    Recept.Mått = Recept.Vara.Mått;
                    Recept.Mängd = (double)(Recept.AntalProdukter * Recept.Vara.Mängd);
                }
            }
            DialogResult = true; // Closes the dialog with a positive result.
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void KollaSiffor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^$|^\d+$"); //Kollar så att det bara går att skriva siffror.
        }
    }
}
