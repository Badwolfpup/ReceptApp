using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceptApp
{
    public class ReceptIngrediens: INotifyPropertyChanged
    {
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        private double _mängd;
        public Ingrediens Ingrediens { get; set; }
        public string Mått { get; set; }

        public double Mängd 
        {
            get { return _mängd; }
            set
            {
                if (_mängd != value)
                {
                    _mängd = value;
                    OnPropertyChanged(nameof(Mängd));
                }
            }
        }


        public double AntalGram { get; set; }

        public ReceptIngrediens(Ingrediens ingrediens, string mått, double mängd)
        {
            Ingrediens = ingrediens;
            Mått = mått;
            Mängd = mängd;
            AntalGram = BeräknaAntalGram(); //Relativt till 100g, ex 125g = 1,25
        }

        public double BeräknaAntalGram()
        {
            switch(Mått)
            {
                case "g": return Mängd / 100.0;                   
                case "dl": return (Ingrediens.GramPerDl * Mängd) / 100.0;
                case "msk": return (Ingrediens.GramPerDl * Mängd * 15 / 100) / 100.0;
                case "tsk": return (Ingrediens.GramPerDl * Mängd * 5 / 100) / 100.0;
                case "krm": return (Ingrediens.GramPerDl * Mängd / 100) / 100.0;
                case "liten": return (Ingrediens.Liten * Mängd) / 100.0;
                case "små": return (Ingrediens.Liten * Mängd) / 100.0;
                case "medelstor": return (Ingrediens.Medel * Mängd) / 100.0;
                case "medelstora": return (Ingrediens.Medel * Mängd) / 100.0;
                case "stor": return (Ingrediens.Stor * Mängd) / 100.0;
                case "stora": return (Ingrediens.Stor * Mängd) / 100.0;
                default: return 0;
            }
        }
    }
}
