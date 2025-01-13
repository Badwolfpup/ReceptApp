using ReceptApp.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ReceptApp.Pages
{
    /// <summary>
    /// Interaction logic for ShoppingList.xaml
    /// </summary>
    public partial class ShoppingList : Page
    {
        App app = (App)Application.Current;

        private Priser _valtpris = null;

        public List<bool> HarFleraPriser { get; set; } = new List<bool>();

        public ShoppingList()
        {
            InitializeComponent();
            DataContext = app;
            Loaded += ShoppingList_Loaded;
        }

        private void ShoppingList_Loaded(object sender, RoutedEventArgs e)
        {
            AggregeraRceptIngredienser();
        }

        private void AggregeraRceptIngredienser()
        {

            //Slår ihop alla ingredienser i shoppinglistan så att varje ingrediens bara förekommer en gång
            app.ReceptIngrediensShoppingList = new ObservableCollection<ReceptIngrediens>(app.ShoppingIngredienser
                .SelectMany(r => r.ReceptIngredienser)
                .GroupBy(item => item.Ingrediens.Namn)
                .Select(group =>
                {
                    string störstmått = group.OrderByDescending(ing => ing.AntalGram).First().Mått;
                    double totalmängd = group.Sum(ing =>
                    {
                        return KonverteraMått(ing.Mått, ing.Mängd, ing.AntalGram, ing.Ingrediens);
                    });


                    return new ReceptIngrediens
                    {

                        Mått = störstmått,
                        Ingrediens = group.First().Ingrediens,
                        Mängd = totalmängd,
                        AntalGram = group.First().AntalGram,
                    };
                })
                .ToList()
            );

            //Konverterar mängden tillbaka till det mått som är hade mest mängd
            foreach (var item in app.ReceptIngrediensShoppingList)
            {
                if (item.Mått == "st")
                {
                    item.Mängd = (double)(item.Mängd / item.Ingrediens.Styck);
                }
                else
                {
                    if (item.Mått == "dl")
                    {
                        item.Mängd = (double)(item.Mängd / 100 / (item.Ingrediens.GramPerDl / 100));
                    }
                    else if (item.Mått == "msk")
                    {
                        item.Mängd = (double)(item.Mängd / 100 / (item.Ingrediens.GramPerDl / 100) / 0.15);
                    }
                    else if (item.Mått == "tsk")
                    {
                        item.Mängd = (double)(item.Mängd / 100 / (item.Ingrediens.GramPerDl / 100) / 0.05);
                    }
                    else if (item.Mått == "krm")
                    {
                        item.Mängd = (double)(item.Mängd / 100 / (item.Ingrediens.GramPerDl / 100) / 0.01);
                    }
                }
            }
            app.PriserIShoppingList.Clear();
            if (RadioBilligast.IsChecked == true)
            {
                foreach (var item in app.ReceptIngrediensShoppingList)
                {
                    if (item.Ingrediens.PrisLista.Count <= 0) break;
                    if (item.Ingrediens.PrisLista.Count > 1) HarFleraPriser.Add(true);
                    else HarFleraPriser.Add(false);
                    app.PriserIShoppingList.Add(item.Ingrediens.PrisLista.OrderBy(pris => pris.JämförelsePris).First());

                    if (app.PriserIShoppingList.Count > 0)
                    {
                        if (app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Förpackningstyp != "lösvikt")
                        {
                            var receptmängd = item.AntalGram * 100;
                            var prismängd = app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mängd;
                            prismängd = KonverteraPrismängd(app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mått, prismängd, app.PriserIShoppingList.Count - 1, item);

                            app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].AntalProdukter = (int)Math.Ceiling((decimal)(receptmängd / prismängd));
                            app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Summa = app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].AntalProdukter > 1 ? app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].AntalProdukter * app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Pris : app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Pris;
                        }
                        else
                        {
                            app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mängd = item.Mängd;
                            app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].SkaÄndraJmfrPris = false;
                            app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mått = item.Mått;
                            app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].SkaÄndraJmfrPris = true;

                            if (app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mått == "g")
                            {

                                app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Summa = app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].JämförelsePris * app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mängd / 1000;
                            }
                            else if (app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mått == "st")
                            {

                                app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Summa = app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].JämförelsePris * (app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mängd * item.Ingrediens.Styck / 1000);
                            }
                            else
                            {
                                app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Summa = app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].JämförelsePris * app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mängd;

                            }


                        }
                    }
                }
            }
            else
            {
                foreach (var item in app.ReceptIngrediensShoppingList)
                {
                    if (item.Ingrediens.PrisLista.Count <= 0) break;
                    if (item.Ingrediens.PrisLista.Count > 1) HarFleraPriser.Add(true);
                    else HarFleraPriser.Add(false);
                    app.PriserIShoppingList.Add(item.Ingrediens.PrisLista.OrderBy(vikt =>
                    {
                        double? prisvikt = 0;
                        if (vikt.Mått == "g") prisvikt = vikt.Mängd;
                        else if (vikt.Mått == "kg") prisvikt = vikt.Mängd * 1000;
                        else if (vikt.Mått == "dl") prisvikt = vikt.Mängd * item.Ingrediens.GramPerDl / 100;
                        else if (vikt.Mått == "l") prisvikt = vikt.Mängd * item.Ingrediens.GramPerDl / 100 * 10;
                        return item.AntalGram * 100 / prisvikt;
                    }).First());
                    if (app.PriserIShoppingList.Count > 0)
                    {
                        if (app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Förpackningstyp != "lösvikt")
                        {
                            var receptmängd = item.AntalGram * 100;
                            var prismängd = app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mängd;
                            prismängd = KonverteraPrismängd(app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mått, prismängd, app.PriserIShoppingList.Count - 1, item);

                            app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].AntalProdukter = (int)Math.Ceiling((decimal)(receptmängd / prismängd));
                            app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Summa = app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].AntalProdukter > 1 ? app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].AntalProdukter * app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Pris : app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Pris;
                        }
                        else
                        {
                            app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mängd = item.Mängd; //item.AntalGram * 100;
                            app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].SkaÄndraJmfrPris = false;
                            app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mått = item.Mått;
                            app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].SkaÄndraJmfrPris = true;

                            if (app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mått == "g")
                            {

                                app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Summa = app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].JämförelsePris * app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mängd / 1000;
                            }
                            else if (app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mått == "st")
                            {

                                app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Summa = app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].JämförelsePris * (app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mängd * item.Ingrediens.Styck / 1000);
                            }
                            else
                            {
                                app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Summa = app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].JämförelsePris * app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mängd;

                            }
                        }
                    }
                }

            }
            if (app.PriserIShoppingList.Count > 0)
            {
                app.TotalSumma = (double)app.PriserIShoppingList.Sum(x => x.Summa);
            }
        }


        private double KonverteraMått(string mått, double mängd, double? antalgram, Ingrediens ingrediens)
        {

            if (mått == "g")
            {
                return (double)(antalgram * 100);
            }
            else if (mått == "st")
            {
                return (double)(mängd * ingrediens.Styck);
            }
            else
            {
                return (double)(antalgram * 100 * (ingrediens.GramPerDl / 100));
            }
        }

        private void DeleteIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.DataContext is Priser i)
                {
                    var hittaRecept = app.ShoppingIngredienser.FirstOrDefault(r => r.ReceptIngredienser.Any(x => x.Ingrediens.PrisLista.Contains(i)));
                    if (hittaRecept != null)
                    {
                        var prislista = hittaRecept.ReceptIngredienser.FirstOrDefault(x => x.Ingrediens.PrisLista.Contains(i));
                        if (prislista != null)
                        {
                            hittaRecept.ReceptIngredienser.Remove(prislista);
                            AggregeraRceptIngredienser();
                        }

                    }

                }
            }
        }

        private void AddToClipboard_Click(object sender, RoutedEventArgs e)
        {
            string clipboard = "";
            foreach (var item in app.PriserIShoppingList)
            {
                if (item.AntalProdukter > 0 && (item.Förpackningstyp != "" || item.Förpackningstyp != "lösvikt"))
                {
                    clipboard += $"{item.AntalProdukter} {item.Förpackningstyp}{(item.Antal > 1 ? $"({item.Antal}st)" : "")} {item.Namn.ToLower()} á {item.Mängd}{item.Mått} ({item.Summa:F2}kr) \n";
                }
                else clipboard += $"{item.Mängd}{item.Mått} {item.Namn.ToLower()} ({item.Summa:F2}kr)\n";

            }
            Clipboard.SetText(clipboard);
        }

        private void DeleteRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.DataContext is Recept r)
                {
                    app.ShoppingIngredienser.Remove(r);
                    AggregeraRceptIngredienser();
                }
            }
        }

        private void EditVara_Click(object sender, RoutedEventArgs e)
        {

            ObservableCollection<Priser> priser = null;
            if (sender is Button button)
            {
                if (button.DataContext is Priser i)
                {

                    var hittaRecept = app.ShoppingIngredienser.FirstOrDefault(r => r.ReceptIngredienser.Any(x => x.Ingrediens.PrisLista.Contains(i)));
                    if (hittaRecept != null)
                    {

                        var prislista = hittaRecept.ReceptIngredienser.FirstOrDefault(x => x.Ingrediens.PrisLista.Contains(i));
                        if (prislista != null)
                        {
                            var receptmängd = prislista.AntalGram * 100;


                            priser = prislista.Ingrediens.PrisLista;
                            foreach (var item in priser)
                            {
                                //var prismängd = item.Mängd;
                                var prismängd = (double)KonverteraPrismängd(item.Mått, item.Mängd, item, prislista);
                                item.AntalProdukter = (int)Math.Ceiling((decimal)(receptmängd / prismängd));
                                item.Summa = item.AntalProdukter > 1 ? item.AntalProdukter * item.Pris : item.Pris;
                            }
                            _valtpris = i;
                        }
                    }
                }
            }
            if (priser == null) return;
            ChangePrice changeprice = new ChangePrice(priser, _valtpris);
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            changeprice.Owner = mainWindow;
            changeprice.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            changeprice.ShowDialog();

        }


        private void SorteringsRadio_Checked(object sender, RoutedEventArgs e)
        {
            AggregeraRceptIngredienser();
        }


        //private void ÄndraInköpsLista_Click(object sender, RoutedEventArgs e)
        //{
        //    //PriserIShoppingList
        //    if (sender is Button button) 
        //    {
        //        if (app.PriserIShoppingList.Count >= 0) {
        //            var olditem = app.PriserIShoppingList.First(x => x.Namn == _valtpris.Namn);
        //            var index = app.PriserIShoppingList.IndexOf(olditem);
        //            app.PriserIShoppingList[index] = _valtpris;
        //            var hittaRecept = app.ShoppingIngredienser.FirstOrDefault(r => r.ReceptIngredienser.Any(x => x.Ingrediens.PrisLista.Contains(_valtpris)));
        //            if (hittaRecept != null)
        //            {
        //                var prislista = hittaRecept.ReceptIngredienser.FirstOrDefault(x => x.Ingrediens.PrisLista.Contains(_valtpris));
        //                if (prislista != null)
        //                {
        //                    var receptmängd = prislista.AntalGram * 100;
        //                    var prismängd = app.PriserIShoppingList[index].Mängd;
        //                    prismängd = KonverteraPrismängd(app.PriserIShoppingList[index].Mått, prismängd, index, prislista);
        //                    //if (app.PriserIShoppingList[index].Mått == "kg") { prismängd *= 1000; }
        //                    //else if (app.PriserIShoppingList[index].Mått == "dl") { prismängd *= prislista.Ingrediens.GramPerDl / 100; }
        //                    //else if (app.PriserIShoppingList[index].Mått == "l") { prismängd *= prislista.Ingrediens.GramPerDl / 100 * 10; }
        //                    app.PriserIShoppingList[index].AntalProdukter = (int)Math.Ceiling((decimal)(receptmängd / prismängd));
        //                    app.PriserIShoppingList[index].Summa = app.PriserIShoppingList[index].AntalProdukter > 1 ? app.PriserIShoppingList[index].AntalProdukter * app.PriserIShoppingList[index].Pris : app.PriserIShoppingList[index].Pris;
        //                }
        //            }
        //            app.TotalSumma = (double)app.PriserIShoppingList.Sum(x => x.Summa);


        //         }
        //    }
        //}

        private double? KonverteraPrismängd(string mått, double? prismängd, int index, ReceptIngrediens prislista)
        {
            if (app.PriserIShoppingList[index].Mått == "kg") { return prismängd *= 1000; }
            else if (app.PriserIShoppingList[index].Mått == "dl") { return prismängd *= prislista.Ingrediens.GramPerDl / 100; }
            else if (app.PriserIShoppingList[index].Mått == "l") { return prismängd *= prislista.Ingrediens.GramPerDl / 100 * 10; }
            return prismängd;
        }

        private double? KonverteraPrismängd(string mått, double? prismängd, Priser pris, ReceptIngrediens prislista)
        {
            if (pris.Mått == "kg") { return prismängd *= 1000; }
            else if (pris.Mått == "dl") { return prismängd *= prislista.Ingrediens.GramPerDl / 100; }
            else if (pris.Mått == "l") { return prismängd *= prislista.Ingrediens.GramPerDl / 100 * 10; }
            return prismängd;
        }


        private void CellEditEnding_CurrentCellChanged(object sender, EventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                if (dataGrid.SelectedItem is Priser pris)
                {
                    dataGrid.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (pris.Förpackningstyp == "lösvikt") pris.Summa = (double)pris.AntalProdukter / 1000 * pris.JämförelsePris;
                    else pris.Summa = pris.AntalProdukter > 1 ? pris.AntalProdukter * pris.Pris : pris.Pris;
                    }), System.Windows.Threading.DispatcherPriority.Background);
                }
                app.TotalSumma = (double)app.PriserIShoppingList.Sum(x => x.Summa);
            }
        }


    }
}
