using ReceptApp.Model;
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
    /// Interaction logic for NewPrice.xaml
    /// </summary>
    public partial class NewPrice : Window, INotifyPropertyChanged
    {
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        App app = (App)Application.Current;

        public NewPrice(string namn)
        {
            InitializeComponent();
            DataContext = this;
            NyPris = new Priser(namn);
            PrisMåttLista = app.PrisMåttLista;
            PrisFörpackningstypLista = app.PrisFörpackningstypLista;
        }

        public List<string> PrisMåttLista { get; }
        public List<string> PrisFörpackningstypLista { get; }

        private Priser _nypris;
        public Priser NyPris
        {
            get { return _nypris; }
            set
            {
                _nypris = value;
                OnPropertyChanged(nameof(NyPris));
            }
        }

        private void LäggTillPris_Click(object sender, RoutedEventArgs e)
        {
            if (NyPris.Mängd == 0 || NyPris.Mängd == null)
            {
                MessageBox.Show("Du måste ange en mängd.");
                return;
            }
            if (NyPris.Pris == 0 || NyPris.Pris == null)
            {
                MessageBox.Show("Du måste ange ett pris.");
                return;
            }
            if (NyPris.Förpackningstyp == "")
            {
                MessageBox.Show("Du behöver ange förpackningstyp.");
                return;
            }
            if (NyPris.Förpackningstyp == "lösvikt")
            {
                NyPris.Antal = null;
            }
            app.ValdIngrediens.PrisLista.Add(NyPris);
            app.ValtPris = NyPris;
            Close();
        }

        private void CancelLäggTillPris_Click(object sender, RoutedEventArgs e)
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

        private void KollaSiffor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^$|^\d+$"); //Kollar så att det bara går att skriva siffror.
        }

        private void Förpackningstyp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox box && box.SelectedItem.ToString() != "")
            {
                NyPris.Förpackningstyp = box.SelectedItem.ToString();
            }
        }

        private void Prismått_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox box && box.SelectedItem.ToString() != "")
            {
                NyPris.Mått = box.SelectedItem.ToString();
            }
        }
    }
}
