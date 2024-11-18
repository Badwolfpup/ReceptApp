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

        private int _mängd;
        public Ingrediens Ingrediens { get; set; }
        public string Mått { get; set; }

        public int Mängd 
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

        public ReceptIngrediens(Ingrediens ingrediens, string mått, int mängd)
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
                case "dl": return Ingrediens.GramPerDl * Mängd / 100.0;
                case "msk": return Ingrediens.GramPerDl / 100.0 * 15 * Mängd;
                case "tsk": return Ingrediens.GramPerDl / 100.0 * 5 * Mängd;
                case "krm": return Ingrediens.GramPerDl / 100.0 * 1 * Mängd;
                case "liten": return Ingrediens.Liten / 100.0 * Mängd;
                case "små": return Ingrediens.Liten / 100.0 * Mängd;
                case "medelstor": return Ingrediens.Medel / 100.0 * Mängd;
                case "medelstora": return Ingrediens.Medel / 100.0 * Mängd;
                case "stor": return Ingrediens.Stor / 100.0 * Mängd;
                case "stora": return Ingrediens.Stor / 100.0 * Mängd;
                default: return 0;
            }
        }
    }
}
