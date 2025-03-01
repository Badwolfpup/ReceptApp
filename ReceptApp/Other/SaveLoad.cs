﻿//using Dapper; Used för mapping to DB
namespace ReceptApp
{
    public static class SaveLoad
    {
        // private static string _folderpath = AppDomain.CurrentDomain.BaseDirectory;
        // private static string _filename;
        //// public static bool SkaKopieraBild { get; set; }
        // //private static string connectionString = "Server=(local);Database=master;Integrated Security=True;";





        // public static void SaveIngrediens(string filename, ObservableCollection<Ingrediens> ingrediens)
        // {
        //     _filename = _folderpath + filename + ".json";

        //     JsonSerializerSettings settings = new JsonSerializerSettings
        //     {
        //         TypeNameHandling = TypeNameHandling.All,
        //         PreserveReferencesHandling = PreserveReferencesHandling.Objects
        //     };

        //     string file = JsonConvert.SerializeObject(ingrediens, settings);
        //     File.WriteAllText(_filename, file);
        // }

        // public static void SaveRecept(string filename, ObservableCollection<Recept> recept)
        // {
        //     _filename = _folderpath + filename + ".json";

        //     JsonSerializerSettings settings = new JsonSerializerSettings
        //     {
        //         TypeNameHandling = TypeNameHandling.All,
        //         PreserveReferencesHandling = PreserveReferencesHandling.Objects
        //     };
        //     string file = JsonConvert.SerializeObject(recept, settings);
        //     File.WriteAllText(_filename, file);
        // }

        // public static ObservableCollection<Ingrediens> LoadIngrediens(string filename)
        // {
        //     _filename = _folderpath + filename + ".json";
        //     if (File.Exists(_filename))
        //     {
        //         string file = File.ReadAllText(_filename);
        //         JsonSerializerSettings settings = new JsonSerializerSettings
        //         {
        //             TypeNameHandling = TypeNameHandling.All,
        //             PreserveReferencesHandling = PreserveReferencesHandling.Objects
        //         };
        //         if (!string.IsNullOrEmpty(file))
        //         {
        //             ObservableCollection<Ingrediens> ingrediens = JsonConvert.DeserializeObject<ObservableCollection<Ingrediens>>(file,settings);
        //             return ingrediens;
        //         }
        //     }
        //     return new ObservableCollection<Ingrediens>();
        // }

        // public static ObservableCollection<Recept> LoadRecept(string filename)
        // {
        //     _filename = _folderpath + filename + ".json";
        //     if (File.Exists(_filename))
        //     {
        //         string file = File.ReadAllText(_filename);
        //         JsonSerializerSettings settings = new JsonSerializerSettings
        //         {
        //             TypeNameHandling = TypeNameHandling.All,
        //             PreserveReferencesHandling = PreserveReferencesHandling.Objects
        //         };
        //         if (!string.IsNullOrEmpty(file))
        //         {
        //             ObservableCollection<Recept> recept = JsonConvert.DeserializeObject<ObservableCollection<Recept>>(file, settings);
        //             return recept;
        //         }
        //     }
        //     return new ObservableCollection<Recept>();
        // }





        //public static void KopieraBild(BitmapImage img, string filnamn, string fileextension, bool hasExtension)
        //{
        //    if (!SkaKopieraBild) return;
        //    if (!hasExtension) fileextension = ".png";
        //    filnamn += fileextension;

        //    string bildfolder = _folderpath + @"\Bilder\";
        //    if (!Directory.Exists(bildfolder)) Directory.CreateDirectory(bildfolder);

        //    string filePath = Path.Combine(bildfolder, filnamn);

        //    var file = Directory.GetFiles(bildfolder, filnamn);
        //    if (file.Any()) return;
        //    if (img != null && !string.IsNullOrEmpty(filnamn))
        //    {
        //        // Create a new BitmapEncoder
        //        BitmapEncoder encoder = new PngBitmapEncoder(); // Choose the appropriate encoder based on your requirements

        //        // Create a new MemoryStream to hold the encoded image data
        //        using (MemoryStream memoryStream = new MemoryStream())
        //        {
        //            // Encode the BitmapImage and write the encoded data to the MemoryStream
        //            encoder.Frames.Add(BitmapFrame.Create(img));
        //            encoder.Save(memoryStream);


        //            // Write the encoded data from the MemoryStream to the file
        //            File.WriteAllBytes(filePath, memoryStream.ToArray());
        //        }
        //    }
        //    SkaKopieraBild = false;
        //}
        #region Databas
        //public static void AddIngrediensToDB(Ingrediens i)
        //{
        //    string sql = @"INSERT INTO Ingredienser(IngrediensID, Namn, Kalori, Protein, Kolhydrat, Socker, Fett, GramPerDl, Liten, Medel, Stor, Bild)
        //                VALUES(@ID, @Namn, @Kalori, @Kolhydrat, @Socker, @Protein, @Fett, @GramPerDl, @Liten, @Medel, @Stor, @Bild)";

        //    string sqlantalrader = @"SELECT COUNT(*) FROM Ingredienser";
        //    int antalrader = 0;
        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        if (i != null)
        //        {
        //            connection.ExecuteScalar<int>(sqlantalrader);
        //            i.ID = antalrader + 1;
        //            connection.Execute(sql, i);
        //        }
        //        connection.Close();
        //    }

        //}

        //public static ObservableCollection<Ingrediens> LoadIngrediensFromDB()
        //{
        //    string query = @"SELECT * FROM Ingredienser";
        //    ObservableCollection<Ingrediens> ingred = new ObservableCollection<Ingrediens>();
        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        List<Ingrediens> lista = (connection.Query<Ingrediens>(query)).AsList();
        //        ingred = new ObservableCollection<Ingrediens>(lista);
        //    }
        //    return ingred;
        //}

        //public static ObservableCollection<Recept> LoadReceptFromDB()
        //{
        //    string query = @"SELECT * FROM Recept";
        //    ObservableCollection<Recept> recept = new ObservableCollection<Recept>();
        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        List<Recept> lista = (connection.Query<Recept>(query)).AsList();
        //        recept = new ObservableCollection<Recept>(lista);
        //    }
        //    return recept;
        //}
        #endregion
    }
}
