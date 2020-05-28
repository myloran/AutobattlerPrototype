using System;
using System.Collections.Generic;
using System.IO;
using MessagePack;
using UnityEngine;
using static System.IO.File;

namespace Controller.Save {
  public class SaveInfoLoader {
    public Dictionary<string, SaveInfo> Load() {
      var dataFolderPath = Path.Combine(Application.dataPath, "Data", "Saves");
      var files = Directory.GetFiles(dataFolderPath, "*.json");
      var saves = new Dictionary<string, SaveInfo>();
      
      foreach (var file in files) {
        var bytes = ReadAllBytes(file);
        var save = MessagePackSerializer.Deserialize<SaveInfo>(bytes);
        saves[save.Name] = save;
      }

      return saves;
    }
    
    public bool Save(SaveInfo save) {
      //TODO: add confirmation if file already exists
      try {
        var path = Path.Combine(Application.dataPath, "Data", "Saves", save.Name + ".json");
        var bytes = MessagePackSerializer.Serialize(save);
        WriteAllBytes(path, bytes);
        return true;
      }
      catch (Exception e) {
        Debug.LogError($"e: {e}");
        return false;
      }
    }
  }
}