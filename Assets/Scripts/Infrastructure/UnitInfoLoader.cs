using System.Collections.Generic;
using System.IO;
using MessagePack;
using MessagePack.Resolvers;
using Newtonsoft.Json;
using Shared;
using Shared.Primitives;
using UnityEngine;

namespace Infrastructure {
  public class UnitInfoLoader {
    public Dictionary<string, UnitInfo> Load() {
      var dataFolderPath = Path.Combine(Application.dataPath, "Data", "Units");
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