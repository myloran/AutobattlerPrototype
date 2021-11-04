using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Shared.Primitives;
using UnityEngine;

namespace Infrastructure {
  public class InfoLoader {
    public Dictionary<string, T> Load<T>(string folderName) where T : IInfo {
      var folderPath = Path.Combine(Application.dataPath, "Data", folderName);
      var infos = new Dictionary<string, T>();
      ProcessDirectory(folderPath, infos);
      return infos;
    }

    void ProcessDirectory<T>(string folderPath, Dictionary<string, T> infos) where T : IInfo {
      var filePaths = Directory.GetFiles(folderPath, "*.json");
      foreach (var filePath in filePaths)
        ProcessFile(filePath, infos);

      var subdirectoryEntries = Directory.GetDirectories(folderPath);
      foreach (var subdirectory in subdirectoryEntries)
        ProcessDirectory(subdirectory, infos);
    }
    
    void ProcessFile<T>(string filePath, Dictionary<string, T> infos) where T : IInfo {
      var text = File.ReadAllText(filePath);
      var unit = JsonConvert.DeserializeObject<T>(text);
      unit.Name = Path.GetFileNameWithoutExtension(filePath);
      infos[unit.Name] = unit;
    }
  }
}