using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class NamePopupWindow : Window, INotifyPropertyChanged
    {
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion


        public NamePopupWindow()
        {
            InitializeComponent();
            DataContext = this;
            this.Loaded += (s,e) => NameTextBox.Focus();
        }

        private string _enteredName;
        public string EnteredName
        {
            get => _enteredName;
            set
            {
                if (_enteredName != value)
                {
                    _enteredName = value;
                    OnPropertyChanged(nameof(EnteredName));
                }
            }
        }


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
