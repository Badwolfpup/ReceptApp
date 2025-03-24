using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ReceptApp.ViewModel
{
    public class VMAddSingleVara : INotifyPropertyChanged
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

        private ReceptIngrediens _receptIng;
        public ReceptIngrediens ReceptIng
        {
            get { return _receptIng; }
            set
            {
                if (_receptIng != value)
                {
                    _receptIng = value;
                    OnPropertyChanged(nameof(ReceptIng));
                }
            }
        }

        public ICommand AddVara { get; set; }
        public ICommand CancelAddVara { get; set; }
        public ICommand KollaSiffor { get; set; }

        private bool? _dialogResult;
        public bool? DialogResult
        {
            get => _dialogResult;
            set
            {
                if (_dialogResult != value)
                {
                    _dialogResult = value;
                    OnPropertyChanged(nameof(DialogResult));
                }
            }
        }

        public string EnteredName { get; private set; }

        public VMAddSingleVara(ReceptIngrediens r, bool ärlösvikt, bool äringrediens)
        {
            ReceptIng = r;
            ÄrIngrediens = äringrediens;
            VisaLösVikt = ärlösvikt;
            VisaSingleVara = !ärlösvikt;
            r.Vara.ÄrÖvrigVara = !ÄrIngrediens;
            AddVara = new RelayCommand(AddVara_Click);
            CancelAddVara = new RelayCommand(CancelAddVara_Click);
            KollaSiffor = new RelayCommand(KollaSiffor_PreviewTextInput);
        }

        private void AddVara_Click(object sender)
        {
            if (sender is ComboBox comboBox && comboBox != null)
            {

                if (VisaLösVikt)
                {
                    if (VisaLösVikt && comboBox.SelectedItem == null || comboBox.SelectedItem.ToString() == "")
                    {
                        MessageBox.Show("Du behöver välja ett mått");
                        return;
                    }

                    if (VisaLösVikt && ReceptIng.Mängd == null || ReceptIng.Mängd <= 0)
                    {
                        MessageBox.Show("Du behöver ange mängd");
                        return;
                    }
                    ReceptIng.Mått = ReceptIng.KonverteraMåttTillText(comboBox.Text);
                }
                else
                {
                    if (VisaSingleVara && ReceptIng.AntalProdukter == null || ReceptIng.AntalProdukter <= 0)
                    {
                        MessageBox.Show("Du behöver ange antal produkter");
                        return;
                    }
                    if (!ReceptIng.Vara.ÄrÖvrigVara)
                    {
                        ReceptIng.Mått = ReceptIng.Vara.Mått;
                        ReceptIng.Mängd = (double)(ReceptIng.AntalProdukter * ReceptIng.Vara.Mängd);
                    }
                }
                DialogResult = true; // Closes the dialog with a positive result.
                                     //Close();
            }
        }

        private void CancelAddVara_Click(object sender)
        {
            DialogResult = false; // Closes the dialog with a negative result.
            //Close();
        }

        private void KollaSiffor_PreviewTextInput(object sender)
        {
            if (sender is TextCompositionEventArgs e)
            {
                e.Handled = !Regex.IsMatch(e.Text, @"^$|^\d+$"); //Kollar så att det bara går att skriva siffror.
            }
        }
    }
}
