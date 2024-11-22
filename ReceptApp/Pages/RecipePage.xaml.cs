﻿using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }

        public RecipePage(ListClass allLists)
        {
            AllLists = allLists;
        }

        public ListClass AllLists { get; }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Recept r = ScrollRecept.SelectedItem as Recept;
            if (r != null)
            {
                AllLists.ReceptLista.Remove(r);
                SaveLoad.SaveRecept("Recept", AllLists.ReceptLista);
            }
        }
    }
}
