using Newtonsoft.Json;
using ReceptApp.Model;
using ReceptApp.Pages;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public Ingrediens Ingrediens { get; set; }

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

        public double? AntalGram { get; set; }

        public ReceptIngrediens(Ingrediens ingrediens, string mått, double mängd)
        {
            Ingrediens = ingrediens;
            Mängd = mängd;
            Mått = mått;
            BeräknaAntalGram(); //Relativt till 100g, ex 125g = 1,25
        }

        public ReceptIngrediens()
        {
            Ingrediens = new Ingrediens();
            Mängd = 0;
            Mått = "";
            AntalGram = 0;
        }

        public void LäggTillInfo(Ingrediens ingrediens, string mått, double mängd)
        {
            Ingrediens = ingrediens;
            Mängd = mängd;
            Mått = mått;
            BeräknaAntalGram(); //Relativt till 100g, ex 125g = 1,25
        }

        //Beräknar det relativa antalet gram av ingrediensen beroende på valt viktmått
        public void BeräknaAntalGram()
        {

            switch (Mått)
            {
                case "g": AntalGram = Mängd / 100.0; break;
                case "dl": AntalGram = (Ingrediens.GramPerDl * Mängd) / 100.0; break;
                case "msk": AntalGram = (Ingrediens.GramPerDl * Mängd * 15 / 100) / 100.0; break;
                case "tsk": AntalGram = (Ingrediens.GramPerDl * Mängd * 5 / 100) / 100.0; break;
                case "krm": AntalGram = (Ingrediens.GramPerDl * Mängd / 100) / 100.0; break;
                //case "liten": AntalGram = (Ingrediens.Liten * Mängd) / 100.0; break;
                //case "små": AntalGram = (Ingrediens.Liten * Mängd) / 100.0; break;
                //case "medelstor": AntalGram = (Ingrediens.Medel * Mängd) / 100.0; break;
                //case "medelstora": AntalGram = (Ingrediens.Medel * Mängd) / 100.0; break;
                //case "stor": AntalGram = (Ingrediens.Stor * Mängd) / 100.0; break;
                //case "stora": AntalGram = (Ingrediens.Stor * Mängd) / 100.0; break;
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
