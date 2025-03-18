using ReceptApp.ViewModel;
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
    public partial class AddSingleVara : Window
    {
        public readonly VMAddSingleVara _vmAddSingleVara;

        public ReceptIngrediens ReceptIng => _vmAddSingleVara.ReceptIng;
        public bool VisaLösVikt => _vmAddSingleVara.VisaLösVikt;

        public AddSingleVara(ReceptIngrediens r,  bool ärlösvikt, bool äringrediens)
        {
            InitializeComponent();
            _vmAddSingleVara = new VMAddSingleVara(r, ärlösvikt, äringrediens);
            DataContext = _vmAddSingleVara;

            _vmAddSingleVara.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_vmAddSingleVara.DialogResult) && _vmAddSingleVara.DialogResult.HasValue)
                {
                    // Set the dialog result and close the window
                    DialogResult = _vmAddSingleVara.DialogResult;
                    Close();
                }
            };

            if (VisaLösVikt) this.Loaded += (s, e) => TextBoxMängd.Focus();
            else this.Loaded += (s, e) => TextBoxAntal.Focus();
        }


        private void KollaSiffor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^$|^\d+$"); //Kollar så att det bara går att skriva siffror.
        }
    }
}
