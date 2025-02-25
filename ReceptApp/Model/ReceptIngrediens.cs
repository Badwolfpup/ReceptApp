using ReceptApp.Model;
using System.ComponentModel;

namespace ReceptApp
{
    public class ReceptIngrediens : INotifyPropertyChanged
    {

        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        private Vara _vara;
        public Vara Vara
        {
            get { return _vara; }
            set
            {
                if (_vara != value)
                {
                    _vara = value;
                    OnPropertyChanged(nameof(Vara));
                }
            }
        }

        private double _mängd = 0;
        public double Mängd
        {
            get { return _mängd; }
            set
            {
                if (_mängd != value)
                {
                    _mängd = value;
                    BeräknaAntalGram();
                    OnPropertyChanged(nameof(Mängd));

                }
            }
        }

        private string _mått; //Valt mått för ingrediensen
        public string Mått
        {
            get { return _mått; }
            set
            {
                if (_mått != value)
                {
                    _mått = value;
                    BeräknaAntalGram();
                    OnPropertyChanged(nameof(Mått));
                }
            }
        }

        private int? _antalprodukter;
        public int? AntalProdukter
        {
            get { return _antalprodukter; }
            set
            {
                if (_antalprodukter != value)
                {
                    _antalprodukter = value;
                    OnPropertyChanged(nameof(AntalProdukter));
                }
            }
        }

        private double? _summa;
        public double? Summa
        {
            get { return _summa; }
            set
            {
                if (_summa != value)
                {
                    _summa = value;
                    OnPropertyChanged(nameof(Summa));
                }
            }
        }

        public double? AntalGram { get; set; }

        public ReceptIngrediens(Vara varan, string mått, double mängd)
        {
            Vara = varan;
            Mängd = mängd;
            Mått = mått;
            BeräknaAntalGram(); //Relativt till 100g, ex 125g = 1,25
        }

        public ReceptIngrediens()
        {
            Vara = new Vara();
            Mängd = 0;
            Mått = "";
            AntalGram = 0;
        }

        public void LäggTillInfo(Vara varan, string mått, double mängd)
        {
            Vara = varan;
            Mängd = mängd;
            Mått = mått;
            BeräknaAntalGram(); //Relativt till 100g, ex 125g = 1,25
        }

        //Beräknar det relativa antalet gram av ingrediensen beroende på valt viktmått relativt till 100g
        public void BeräknaAntalGram()
        {

            switch (Mått)
            {
                case "g": AntalGram = Mängd / 100.0; break;
                case "kg": AntalGram = Mängd * 10; break;
                case "st": AntalGram = Mängd * Vara.Naring.Styck / 100; break;
                case "dl": AntalGram = (Vara.Naring.GramPerDl * Mängd) / 100.0; break;
                case "msk": AntalGram = (Vara.Naring.GramPerDl * Mängd * 15 / 100) / 100.0; break;
                case "tsk": AntalGram = (Vara.Naring.GramPerDl * Mängd * 5 / 100) / 100.0; break;
                case "krm": AntalGram = (Vara.Naring.GramPerDl * Mängd / 100) / 100.0; break;

                default: AntalGram = 0; break;
            }
        }

        public string KonverteraMåttTillText(string text)
        {
            switch (text)
            {
                case "Gram": return "g";
                case "Deciliter": return "dl";
                case "Matsked": return "msk";
                case "Tesked": return "tsk";
                case "Kryddmått": return "krm";
                case "Stycken": return "st";
                case "Antal stor": if (Mängd > 1) return "stora"; else return "stor";
                case "Antal medel": if (Mängd > 1) return "medelstora"; else return "medelstor";
                case "Antal liten": if (Mängd > 1) return "små"; else return "liten";
                default: return "";
            }
        }

        //public ReceptIngrediens Copy()
        //{
        //    var json = JsonConvert.SerializeObject(this);
        //    return JsonConvert.DeserializeObject<ReceptIngrediens>(json);
        //}

    }
}
