using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Shared.Primitives;
using UnityEngine;

namespace Infrastructure {
  public class EffectInfoLoader {
    public Dictionary<string, EffectInfo> Load() {
      var dataFolderPath = Path.Combine(Application.dataPath, "Data", "Effects");
      var files = Directory.GetFiles(dataFolderPath, "*.json");
      var abilities = new Dictionary<string, EffectInfo>();

      foreach (var file in files) {
        var text = File.ReadAllText(file);
        var unit = JsonConvert.DeserializeObject<EffectInfo>(text);
        abilities[unit.Name] = unit;
      }

      return abilities;
    }
  }
}