using System;
using System.Collections.Generic;
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
    /// Interaction logic for AddSingleOvrigVara.xaml
    /// </summary>
    public partial class AddSingleOvrigVara : Window
    {
        public AddSingleOvrigVara(ReceptIngrediens r, bool arovrig)
        {
            InitializeComponent();
            Recept = r;
            DataContext = Recept;
            _ärövrig = arovrig;
            this.Loaded += (s, e) => TextBoxMängd.Focus();
        }


        public string EnteredName { get; private set; }
        public ReceptIngrediens Recept { get; set; }
        private bool _ärövrig;
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (Recept.AntalProdukter == null || Recept.AntalProdukter <= 0)
            {
                MessageBox.Show("Du behöver ange antal produkter");
                return;
            }
            if (!_ärövrig)
            {
                Recept.Mått = Recept.Vara.Mått;
                Recept.Mängd = (double)(Recept.AntalProdukter * Recept.Vara.Mängd);
                
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
