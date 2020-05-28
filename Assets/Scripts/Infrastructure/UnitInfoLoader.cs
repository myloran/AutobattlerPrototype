using System.Collections.Generic;
using System.IO;
using MessagePack;
using MessagePack.Resolvers;
using Newtonsoft.Json;
using Shared;
using UnityEngine;

namespace Infrastructure {
  public class UnitInfoLoader {
    public Dictionary<string, UnitInfo> Load() {
      var dataFolderPath = Path.Combine(Application.dataPath, "Data", "Units");
      var files = Directory.GetFiles(dataFolderPath, "*.json");
      var units = new Dictionary<string, UnitInfo>();

      foreach (var file in files) {
        var text = File.ReadAllText(file);
        var bytes = MessagePackSerializer.ConvertFromJson(text);
        var unit = MessagePackSerializer.Deserialize<UnitInfo>(bytes);
        units[unit.Name] = unit;
      }

      return units;
    }
  }
}