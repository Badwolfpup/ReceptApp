using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ReceptApp
{
    public class Ingrediens : INotifyPropertyChanged, INotifyCollectionChanged
    {
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        #endregion

        #region Properties
        private string _namn;

        private int _kalori;
        private int _protein;
        private int _kolhydrat;
        private int _socker;
        private int _fett;
        //private int[] _viktmått;
        private int _gramperdl;
        private int _liten;
        private int _medel;
        private int _stor;
        private string _bild;

        //public int ID { get; set; } för DB
        public ObservableCollection<string> ViktMått { get; set; }

        public string Namn
        {
            get { return _namn; }
            set
            {
                if (_namn != value)
                {
                    _namn = value;
                    OnPropertyChanged(nameof(Namn));
                }
            }
        }
        public int Kalori
        {
            get { return _kalori; }
            set
            {
                if (_kalori != value)
                {
                    _kalori = value;
                    OnPropertyChanged(nameof(Kalori));
                }
            }
        }
        public int Protein
        {
            get { return _protein; }
            set
            {
                if (_protein != value)
                {
                    _protein = value;
                    OnPropertyChanged(nameof(Protein));
                }
            }
        }
        public int Kolhydrat
        {
            get { return _kolhydrat; }
            set
            {
                if (_kolhydrat != value)
                {
                    _kolhydrat = value;
                    OnPropertyChanged(nameof(Kolhydrat));
                }
            }
        }
        public int Socker
        {
            get { return _socker; }
            set
            {
                if (_socker != value)
                {
                    _socker = value;
                    OnPropertyChanged(nameof(Socker));
                }
            }
        }
        public int Fett
        {
            get { return _fett; }
            set
            {
                if (_fett != value)
                {
                    _fett = value;
                    OnPropertyChanged(nameof(Fett));
                }
            }
        }
        //public int[] Viktmått
        //{
        //    get { return _viktmått; }
        //    set
        //    {
        //        if (_viktmått != value)
        //        {
        //            _viktmått = value;
        //            OnPropertyChanged(nameof(Viktmått));
        //        }
        //    }
        //}
        public int GramPerDl
        {
            get { return _gramperdl; }
            set
            {
                if (_gramperdl != value)
                {
                    _gramperdl = value;
                    LäggTillViktmått();
                    OnPropertyChanged(nameof(GramPerDl));
                }
            }
        }
        public int Liten
        {
            get { return _liten; }
            set
            {
                if (_liten != value)
                {
                    _liten = value;
                    LäggTillViktmått();
                    OnPropertyChanged(nameof(Liten));
                }
            }
        }
        public int Medel
        {
            get { return _medel; }
            set
            {
                if (_medel != value)
                {
                    _medel = value;
                    LäggTillViktmått();
                    OnPropertyChanged(nameof(Medel));
                }

            }
        }
        public int Stor
        {
            get { return _stor; }
            set
            {
                if (_stor != value)
                {
                    _stor = value;
                    LäggTillViktmått();
                    OnPropertyChanged(nameof(Stor));
                }
            }
        }
        public string Bild
        {
            get { return _bild; }
            set
            {
                if (_bild != value)
                {
                    _bild = value;
                    
                    OnPropertyChanged(nameof(Bild));
                }
            }
        }
        #endregion


        //public Ingrediens(string namn, int kalori, int protein, int kolhydrat, int socker, int fett, int[] viktmått)
        //{
        //    Namn = namn;
        //    Kalori = kalori;
        //    Protein = protein;
        //    Kolhydrat = kolhydrat;
        //    Socker = socker;
        //    Fett = fett;
        //    Viktmått = viktmått;
        //    LäggTillViktmått();
           
        //}

        //public Ingrediens(string namn)
        //{
        //    Namn = namn;
            
        //}

        public Ingrediens()
        {
            ViktMått = new ObservableCollection<string> { "Gram" };
        }

        private void LäggTillViktmått()
        {
            ViktMått = new ObservableCollection<string> { "Gram" };
            if (GramPerDl > 0)
            {
                ViktMått.Add("Deciliter");
                ViktMått.Add("Matsked");
                ViktMått.Add("Tesked");
                ViktMått.Add("Kryddmått");
            }
            if (Liten > 0) ViktMått.Add("Antal liten");
            if (Medel > 0) ViktMått.Add("Antal medel");
            if (Stor > 0) ViktMått.Add("Antal stor");
        }
    }
}
