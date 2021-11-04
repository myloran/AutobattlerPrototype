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
      var folderPath = Path.Combine(Application.dataPath, "Data", "Units");
      ProcessDirectory(folderPath);
      return units;
    }

    void ProcessDirectory(string folderPath) {
      var files = Directory.GetFiles(folderPath, "*.json");
      foreach (var file in files)
        ProcessFile(file);

      var subdirectoryEntries = Directory.GetDirectories(folderPath);
      foreach (var subdirectory in subdirectoryEntries)
        ProcessDirectory(subdirectory);
    }
    
    void ProcessFile(string filePath) {
      var text = File.ReadAllText(filePath);
      var unit = JsonConvert.DeserializeObject<UnitInfo>(text);
      units[unit.Name] = unit;
    }

    readonly Dictionary<string, UnitInfo> units = new Dictionary<string, UnitInfo>();
  }
}