using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace AppUnrar.Presenter
{
    public static class AppDataView
    {
        public static void appdata_fetch_json()
        {
            string json_path = System.IO.File.ReadAllText(@"appdata.json");
            var data = JsonSerializer.Deserialize<MapAppData>(json_path);
            data?.MapToStaticClass(data);
        }

        public static void appdata_save_changes()
        {
            MapAppData _data = new MapAppData();
            _data.Passwords = AppData.Passwords;
            _data.FileExtensionsToExtract = AppData.FileExtensionsToExtract;

            string json = JsonSerializer.Serialize(_data);
            System.IO.File.WriteAllText(@"appdata.json", json);
        }
    }
}
