using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Shared;
using UnityEngine;

namespace Infrastructure {
  public class UnitDataLoader {
    public Dictionary<string, UnitInfo> Load() {
      var dataFolderPath = Path.Combine(Application.dataPath, "Data");
      var files = Directory.GetFiles(dataFolderPath, "*.json");
      var units = new Dictionary<string, UnitInfo>();
      
      foreach (var file in files) {
        var text = File.ReadAllText(file);
        var unit = JsonConvert.DeserializeObject<UnitInfo>(text);
        units[unit.Name] = unit;
      }

      return units;
    }
  }
}