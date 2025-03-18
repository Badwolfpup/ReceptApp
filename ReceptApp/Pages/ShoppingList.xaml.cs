using ReceptApp.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace ReceptApp.Pages
{
    /// <summary>
    /// Interaction logic for ShoppingList.xaml
    /// </summary>
    public partial class ShoppingList : Page, INotifyPropertyChanged
    {
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        public ObservableCollection<ReceptIngrediens> ShoppingListaIngredienser => AppData.Instance.ShoppingListaIngredienser;
        public ObservableCollection<Recept> ShoppingListaRecept => AppData.Instance.ShoppingListaRecept;

        public ShoppingList()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += ShoppingList_Loaded;
        }

        private double _totalsumma;
        public double TotalSumma
        {
            get => _totalsumma;
            set
            {
                if (_totalsumma != value)
                {
                    _totalsumma = value;
                    OnPropertyChanged(nameof(TotalSumma));
                }
            }
        }

        private void ShoppingList_Loaded(object sender, RoutedEventArgs e)
        {
            AggregeraInitialaReceptIngredienser();
        }

        private void AggregeraInitialaReceptIngredienser()
        {
            //Slår ihop alla ingredienser i shoppinglistan så att varje ingrediens bara förekommer en gång
            var newlist = ShoppingListaIngredienser
                .GroupBy(item => new { item.Vara.Namn, item.Vara.Typ, item.Vara.Info })
                .Select(group =>
                {
                    string störstmått = group.OrderByDescending(ing => ing.AntalGram).First().Mått;
                    double totalmängd = group.Sum(ing => (double)ing.AntalGram * 100);
                    return new ReceptIngrediens
                    {
                        Mått = störstmått,
                        Vara = group.First().Vara,                       
                        Mängd = totalmängd,
                        AntalGram = group.First().AntalGram,                       
                        AntalProdukter = group.First().Vara.ÄrÖvrigVara ? group.First().AntalProdukter : null,
                    };
                })
                .ToList();
            
            ShoppingListaIngredienser.Clear();
            foreach (var item in newlist)
            {
                ShoppingListaIngredienser.Add(item);
            }

            //Konverterar mängden tillbaka till det mått som är hade mest mängd
            foreach (var item in ShoppingListaIngredienser)
            {
                if (item.Vara.ÄrÖvrigVara) continue;
                if (item.Mått == "st")
                {
                    item.Mängd = (double)(item.Mängd / item.Vara.Naring.Styck);
                }
                else
                {
                    if (item.Mått == "dl")
                    {
                        item.Mängd = (double)(item.Mängd / 100.0 / (item.Vara.Naring.GramPerDl / 100.0));
                    }
                    else if (item.Mått == "msk")
                    {
                        item.Mängd = (double)(item.Mängd / 100.0 / (item.Vara.Naring.GramPerDl / 100.0) / 0.15);
                    }
                    else if (item.Mått == "tsk")
                    {
                        item.Mängd = (double)(item.Mängd / 100.0 / (item.Vara.Naring.GramPerDl / 100.0) / 0.05);
                    }
                    else if (item.Mått == "krm")
                    {
                        item.Mängd = (double)(item.Mängd / 100.0 / (item.Vara.Naring.GramPerDl / 100.0) / 0.01);
                    }
                }
            }
            RäknaAntalProdukter();


        }


        private void RäknaAntalProdukter()
        {
            foreach (var item in ShoppingListaIngredienser)
            {
                if (item.Vara.ÄrÖvrigVara) continue;
                if (!item.Vara.ÄrInteLösvikt) continue;
                switch(item.Mått)
                {
                    case "kg": item.AntalProdukter = (int)Math.Ceiling((double)item.Mängd / (double)item.Vara.Mängd / 1000.0); item.Mängd = (double)(item.AntalProdukter * item.Vara.Mängd); item.Mått = item.Vara.Mått; break;
					case "st": item.AntalProdukter = (int)Math.Ceiling((double)item.Mängd / (double)item.Vara.Naring.Styck); item.Mängd = (double)(item.AntalProdukter * item.Vara.Mängd); item.Mått = item.Vara.Mått; break; 
                    case "dl": item.AntalProdukter = (int)Math.Ceiling((double)item.Mängd / (double)item.Vara.Mängd); item.Mängd = (double)(item.AntalProdukter * item.Vara.Mängd); item.Mått = item.Vara.Mått; break;
                    case "msk": item.AntalProdukter = (int)Math.Ceiling((double)item.Mängd * 15 / 100 / (double)item.Vara.Mängd); item.Mängd = (double)(item.AntalProdukter * item.Vara.Mängd); item.Mått = item.Vara.Mått; break;
                    case "tsk": item.AntalProdukter = (int)Math.Ceiling((double)item.Mängd * 5 / 100 / (double)item.Vara.Mängd); item.Mängd = (double)(item.AntalProdukter * item.Vara.Mängd); item.Mått = item.Vara.Mått; break;
                    case "krm": item.AntalProdukter = (int)Math.Ceiling((double)item.Mängd * 1 / 100 / (double)item.Vara.Mängd); item.Mängd = (double)(item.AntalProdukter * item.Vara.Mängd); item.Mått = item.Vara.Mått; break;
                    default: item.AntalProdukter = (int)Math.Ceiling((double)item.Mängd / (double)item.Vara.Mängd); item.Mängd = (double)(item.AntalProdukter * item.Vara.Mängd); item.Mått = item.Vara.Mått; break;
                }
            }
            RäknaSumma();
        }


        private void RäknaSumma()
        {
            foreach (var item in ShoppingListaIngredienser)
            {
                if (item.AntalProdukter > 0 && (item.Vara.Förpackningstyp != "" || item.Vara.Förpackningstyp != "lösvikt"))
                {
                    item.Summa = (double)(item.AntalProdukter * item.Vara.Pris);
                }
                else item.Summa = (double)(item.Vara.Pris * item.AntalGram / 10);
            }
            TotalSumma = (double)ShoppingListaIngredienser.Sum(x => x.Summa);
        }

        private void DeleteIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ReceptIngrediens vara) ShoppingListaIngredienser.Remove(vara);
        }

        private void AddToClipboard_Click(object sender, RoutedEventArgs e)
        {
            string clipboard = "";
            foreach (var item in ShoppingListaIngredienser)
            {
                if (item.AntalProdukter > 0 && (item.Vara.Förpackningstyp != "" || item.Vara.Förpackningstyp != "lösvikt"))
                {
                    clipboard += $"{item.AntalProdukter} {KonverteraFörpackningTillPlural(item.AntalProdukter, item.Vara.Förpackningstyp)} {item.Vara.Namn.ToLower()} {(item.Vara.Typ != "" ? item.Vara.Typ : "")} {(item.Vara.Info != "" ? item.Vara.Info : "")} {(!item.Vara.ÄrÖvrigVara ? $"á {item.Mängd}{item.Mått}" : "")} ({item.Summa:F2}kr) \n";
                }
                else clipboard += $"{item.Mängd}{(item.Mått == "st" ? " " : "")}{item.Mått} {item.Vara.Namn.ToLower()} {(item.Vara.Typ != "" ? item.Vara.Typ : "")} {(item.Vara.Info != "" ? item.Vara.Info : "")} ({item.Summa:F2}kr)\n";

            }
            Clipboard.SetText(clipboard);
        }

        private void DeleteRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.DataContext is Recept r)
                {
                    foreach (var item in r.ReceptIngredienser)
                    {
                        var hittaingrediens = ShoppingListaIngredienser.FirstOrDefault(x => x.Vara.Namn == item.Vara.Namn && x.Vara.Typ == item.Vara.Typ && x.Vara.Info == item.Vara.Info);
                        if (hittaingrediens != default)
                        {
                            if (item.Mått == hittaingrediens.Mått)
                            {
                                hittaingrediens.Mängd -= item.Mängd;
                            }
                            else
                            {
                                if (item.Mått == "st" && hittaingrediens.Mått == "g")
                                {
                                    hittaingrediens.Mängd -= (double)(item.Mängd * hittaingrediens.Vara.Naring.Styck);
                                }
                                else if (item.Mått == "g" && hittaingrediens.Mått == "st")
                                {
                                    hittaingrediens.Mängd -= (double)(item.Mängd / hittaingrediens.Vara.Naring.Styck);
                                }
                                else if (item.Mått == "g" && hittaingrediens.Mått == "dl")
                                {
                                    hittaingrediens.Mängd -= (double)(item.Mängd / item.Vara.Naring.GramPerDl);
                                }
                                else if (item.Mått == "dl" && hittaingrediens.Mått == "g")
                                {
                                    hittaingrediens.Mängd -= (double)(item.Mängd * item.Vara.Naring.GramPerDl);
                                }
                                else if (item.Mått == "g" && hittaingrediens.Mått == "msk")
                                {
                                    hittaingrediens.Mängd -= (double)(item.Mängd / item.Vara.Naring.GramPerDl / 0.15);
                                }
                                else if (item.Mått == "msk" && hittaingrediens.Mått == "g")
                                {
                                    hittaingrediens.Mängd -= (double)(item.Mängd * item.Vara.Naring.GramPerDl * 0.15);
                                }
                                else if (item.Mått == "g" && hittaingrediens.Mått == "tsk")
                                {
                                    hittaingrediens.Mängd -= (double)(item.Mängd / item.Vara.Naring.GramPerDl / 0.05);
                                }
                                else if (item.Mått == "tsk" && hittaingrediens.Mått == "g")
                                {
                                    hittaingrediens.Mängd -= (double)(item.Mängd * item.Vara.Naring.GramPerDl * 0.05);
                                }
                                else if (item.Mått == "g" && hittaingrediens.Mått == "krm")
                                {
                                    hittaingrediens.Mängd -= (double)(item.Mängd / item.Vara.Naring.GramPerDl / 0.01);
                                }
                                else if (item.Mått == "krm" && hittaingrediens.Mått == "g")
                                {
                                    hittaingrediens.Mängd -= (double)(item.Mängd * item.Vara.Naring.GramPerDl * 0.01);
                                }
                            }
                        }
                    }
                    RäknaAntalProdukter();
                    AppData.Instance.ShoppingListaRecept.Remove(r);
                    List<ReceptIngrediens> toRemove = new List<ReceptIngrediens>();
                    foreach (var item in ShoppingListaIngredienser)
                    {
                        if (item.Vara.ÄrÖvrigVara) continue;
                        if (!AppData.Instance.ShoppingListaRecept.Any(x => x.ReceptIngredienser.Any(y => y.Vara.Namn == item.Vara.Namn && y.Vara.Typ == item.Vara.Typ && y.Vara.Info == y.Vara.Info))) toRemove.Add(item);
                    }
                    foreach (var item in toRemove)
                    {
                        ShoppingListaIngredienser.Remove(item);
                    }
                }
            }
        }

        private void EditVara_Click(object sender, RoutedEventArgs e)
        {


            if (sender is Button button)
            {
                if (button.DataContext is ReceptIngrediens r)
                {
                    var hittaingrediens = AppData.Instance.IngrediensLista.FirstOrDefault(x => x.Varor.Any(y => x.Namn == r.Vara.Namn && y.Info == r.Vara.Info));
                    ChangePrice changeprice = new ChangePrice(hittaingrediens.CopyVaror(), r.Vara, r);
                    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                    changeprice.Owner = mainWindow;
                    changeprice.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                    changeprice.ShowDialog();
                    RäknaAntalProdukter();
                }
            }



        }


        private string KonverteraFörpackningTillPlural(int? antal, string typ)
        {
            if (antal == null) return typ;
            if (antal > 1)
            {
                switch (typ)
                {
                    case "påse": return "påsar";
                    case "burk": return "burkar";
                    case "förp": return "förpackningar";
                    case "tub": return "tuber";
                    case "flaska": return "flaskor";
                    default: return typ;
                }
            }
            else return typ;
        }

        private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (sender is IntegerUpDown integerUpDown && integerUpDown.DataContext is ReceptIngrediens vara)
            {
                vara.AntalProdukter = (int)integerUpDown.Value;
                if (!vara.Vara.ÄrÖvrigVara) vara.Mängd = (double)(vara.AntalProdukter * vara.Vara.Mängd);
				RäknaSumma();
            }
        }

        private void SaveList_Click(object sender, RoutedEventArgs e)
        {
            AppData.Instance.SaveAll();
        }
    }
}
