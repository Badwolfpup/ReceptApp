using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceptApp.Model
{
    public class Viktmått: INotifyPropertyChanged
    {
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        #endregion

        private string _namn;
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

        public Viktmått(string namn)
        {
            Namn = namn;
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnPropertyChanged(nameof(IsActive));
                }
            }
        }
    }
}
