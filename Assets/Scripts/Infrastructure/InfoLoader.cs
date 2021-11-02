using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Shared.Primitives;
using UnityEngine;

namespace Infrastructure {
  public class InfoLoader {
    public Dictionary<string, T> Load<T>(string folderName) where T : IInfo {
      var dataFolderPath = Path.Combine(Application.dataPath, "Data", folderName);
      var files = Directory.GetFiles(dataFolderPath, "*.json");
      var abilities = new Dictionary<string, T>();

      foreach (var file in files) {
        var text = File.ReadAllText(file);
        var unit = JsonConvert.DeserializeObject<T>(text);
        unit.Name = Path.GetFileNameWithoutExtension(file);
        abilities[unit.Name] = unit;
      }

      return abilities;
    }
  }
}