using System;
using System.Collections.Generic;
using System.IO;
using MessagePack;
using UnityEngine;
using static System.IO.File;

namespace Controller.DecisionTree.Data {
  public class DecisionTreeLoader {
    public IDecisionTreeComponent Load() {
      var path = Path.Combine(Application.dataPath, "Data", "DecisionTree", "current" + ".json");
      var bytes = ReadAllBytes(path);
      return MessagePackSerializer.Deserialize<IDecisionTreeComponent>(bytes);
    }
    
    //TODO: create directory if does not exist
    public bool Save(IDecisionTreeComponent component) {
      //TODO: add confirmation if file already exists
      try {
        var path = Path.Combine(Application.dataPath, "Data", "DecisionTree", "current" + ".json");
        // var text = MessagePackSerializer.SerializeToJson(component);
        // WriteAllText(path, text);
        var bytes = MessagePackSerializer.Serialize(component);
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