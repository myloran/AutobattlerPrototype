using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Shared.Primitives;
using UnityEngine;

namespace Infrastructure {
  public class SynergyInfoLoader {
    public Dictionary<string, SynergyInfo> Load() {
      var dataFolderPath = Path.Combine(Application.dataPath, "Data", "Synergies");
      var files = Directory.GetFiles(dataFolderPath, "*.json");
      var abilities = new Dictionary<string, SynergyInfo>();

      foreach (var file in files) {
        var text = File.ReadAllText(file);
        var unit = JsonConvert.DeserializeObject<SynergyInfo>(text);
        abilities[unit.Name] = unit;
      }

      return abilities;
    }
  }
}