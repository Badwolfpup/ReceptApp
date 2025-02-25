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
    /// Interaction logic for AddSingleVara.xaml
    /// </summary>
    public partial class AddSingleVara : Window
    {

        public AddSingleVara(ReceptIngrediens r)
        {
            InitializeComponent();
            Recept = r;
            DataContext = Recept;
        }


        public string EnteredName { get; private set; }
        public ReceptIngrediens Recept { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxMåttNamn.SelectedItem == null || ComboBoxMåttNamn.SelectedItem.ToString() == "")
            {
                MessageBox.Show("Du behöver välja ett mått");
                return;
            }

            if (Recept.Mängd == null || Recept.Mängd <= 0)
            {
                MessageBox.Show("Du behöver ange mängd");
                return;
            }
            Recept.Mått = Recept.KonverteraMåttTillText(ComboBoxMåttNamn.Text);
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
