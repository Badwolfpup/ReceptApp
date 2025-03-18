using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ReceptApp.Other
{
    public static class DependencyHelper
    {
        #region Filter Text Changed
        public static readonly DependencyProperty FilterTextCommandProperty = DependencyProperty.RegisterAttached(
            "FilterTextCommand",
            typeof(ICommand),
            typeof(DependencyHelper),
            new PropertyMetadata(null, OnFilterTextChanged));

        public static ICommand GetFilterTextCommand(DependencyObject obj) => (ICommand)obj.GetValue(FilterTextCommandProperty);
        public static void SetFilterTextCommand(DependencyObject obj, ICommand value) => obj.SetValue(FilterTextCommandProperty, value);

        private static void OnFilterTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                if (e.NewValue != null)
                {
                    textBox.TextChanged += TextBox_TextChanged;
                }
                else
                {
                    textBox.TextChanged -= TextBox_TextChanged;
                }
            }
        }

        private static void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                ICommand command = GetFilterTextCommand(textBox);
                if (command != null)
                {
                    command.Execute(textBox);
                }
            }
        }
        #endregion

        public static readonly DependencyProperty GenericCommandProperty = DependencyProperty.RegisterAttached(
            "GenericCommandProperty",
            typeof(ICommand),
            typeof(DependencyHelper),
            new PropertyMetadata(null, OnPassElementChanged));

        public static ICommand GetGenericCommand(DependencyObject obj) => (ICommand)obj.GetValue(GenericCommandProperty);
        public static void SetGenericCommand(DependencyObject obj, ICommand value) => obj.SetValue(GenericCommandProperty, value);

        private static void OnPassElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                if (e.NewValue != null)
                {
                    textBox.TextChanged += TextBox_TextChanged;
                }
                else
                {
                    textBox.TextChanged -= TextBox_TextChanged;
                }
            }
        }

        private static void Generic_Event(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                ICommand command = GetGenericCommand(dataGrid);
                if (command != null)
                {
                    command.Execute(dataGrid);
                }
            }
            else if (sender is TextBox textBox)
            {
                ICommand command = GetGenericCommand(textBox);
                if (command != null)
                {
                    command.Execute(textBox);
                }
            }
        }
    }
}
