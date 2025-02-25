using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for NamePopupWindow.xaml
    /// </summary>
    public partial class NamePopupWindow : Window
    {
        public NamePopupWindow()
        {
            InitializeComponent();
        }

        public string EnteredName { get; private set; }


        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            EnteredName = NameTextBox.Text;
            DialogResult = true; // Closes the dialog with a positive result.
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
