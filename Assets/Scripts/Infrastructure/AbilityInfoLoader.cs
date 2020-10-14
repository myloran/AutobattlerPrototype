using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Shared.Primitives;
using UnityEngine;

namespace Infrastructure {
  public class AbilityInfoLoader {
    public Dictionary<string, AbilityInfo> Load() {
      var dataFolderPath = Path.Combine(Application.dataPath, "Data", "Units");
      var files = Directory.GetFiles(dataFolderPath, "*.json");
      var abilities = new Dictionary<string, AbilityInfo>();

      foreach (var file in files) {
        var text = File.ReadAllText(file);
        var unit = JsonConvert.DeserializeObject<AbilityInfo>(text);
        abilities[unit.Name] = unit;
      }

      return abilities;
    }
  }
}