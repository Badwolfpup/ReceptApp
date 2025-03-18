using ReceptApp.Model;
using ReceptApp.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ReceptApp.Pages
{
    /// <summary>
    /// Interaction logic for RecipePage.xaml
    /// </summary>
    public partial class RecipePage : Page
    {

        public RecipePage()
        {
            InitializeComponent();
            DataContext = new VMRecipePage();
            Loaded += (s, e) => { FilterTextboxRecept.Clear(); FilterTextboxRecept.Focus(); };

        }
    }
}
