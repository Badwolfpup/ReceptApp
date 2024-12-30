using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceptApp.Model
{
    public class AppData
    {
        public ObservableCollection<Ingrediens> Ingredienslista { get; set; } = new ObservableCollection<Ingrediens>();
        public ObservableCollection<Recept> ReceptLista { get; set; } = new ObservableCollection<Recept>();


        public void SaveAll()
        {
            string _folderpath = AppDomain.CurrentDomain.BaseDirectory;
            string _filepath = _folderpath + "Appdata.json";

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented 

            };

            string file = JsonConvert.SerializeObject(this, settings);
            File.WriteAllText(_filepath, file);
        }

        public static AppData Load()
        {

            string _folderpath = AppDomain.CurrentDomain.BaseDirectory;
            string _filepath = _folderpath + "Appdata.json";

            if (!File.Exists(_filepath))
            {
                return new AppData();
            }

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            string file = File.ReadAllText(_filepath);
            return JsonConvert.DeserializeObject<AppData>(file, settings);
        }
    }
}
