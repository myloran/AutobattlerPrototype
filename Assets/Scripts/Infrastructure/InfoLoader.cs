using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Shared.Abstraction;
using Shared.Primitives;
using UnityEngine;

namespace Infrastructure {
  public class InfoLoader<T> : IInfoGetter<T> where T : IInfo {
    public Dictionary<string, T> Infos { get; private set; }
    
    public void Load(string folderName) {
      var folderPath = Path.Combine(Application.dataPath, "Data", folderName);
      Infos = new Dictionary<string, T>();
      ProcessDirectory(folderPath, Infos);
    }

    void ProcessDirectory(string folderPath, Dictionary<string, T> infos) {
      var filePaths = Directory.GetFiles(folderPath, "*.json");
      foreach (var filePath in filePaths)
        ProcessFile(filePath, infos);

      var subdirectoryEntries = Directory.GetDirectories(folderPath);
      foreach (var subdirectory in subdirectoryEntries)
        ProcessDirectory(subdirectory, infos);
    }
    
    void ProcessFile(string filePath, Dictionary<string, T> infos) {
      var text = File.ReadAllText(filePath);
      var unit = JsonConvert.DeserializeObject<T>(text);
      unit.Name = Path.GetFileNameWithoutExtension(filePath);
      infos[unit.Name] = unit;
    }

  }
}