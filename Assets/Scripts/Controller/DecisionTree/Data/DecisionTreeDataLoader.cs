using System;
using System.Collections.Generic;
using System.IO;
using MessagePack;
using Newtonsoft.Json;
using UnityEngine;
using static System.IO.File;

namespace Controller.DecisionTree.Data {
  public class DecisionTreeDataLoader {
    public DecisionTreeComponent Load() {
      var path = Path.Combine(Application.dataPath, "Data", "DecisionTree", "current" + ".json");
      var text = ReadAllText(path);
      var settings = new JsonSerializerSettings 
      { 
        TypeNameHandling = TypeNameHandling.Auto,
        NullValueHandling = NullValueHandling.Ignore
      };
      
      return JsonConvert.DeserializeObject<DecisionTreeComponent>(text, settings);
    }
    
    //TODO: create directory if does not exist
    public bool Save(DecisionTreeComponent component) {
      //TODO: add confirmation if file already exists
      try {
        var path = Path.Combine(Application.dataPath, "Data", "DecisionTree", "current" + ".json");
        
        JsonSerializer serializer = new JsonSerializer();
        serializer.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
        serializer.NullValueHandling = NullValueHandling.Ignore;
        serializer.TypeNameHandling = TypeNameHandling.Auto;
        serializer.Formatting = Formatting.Indented;

        using (StreamWriter sw = new StreamWriter(path))
        using (JsonWriter writer = new JsonTextWriter(sw))
        {
          serializer.Serialize(writer, component, typeof(DecisionTreeComponent));
        }

        return true;
      }
      catch (Exception e) {
        Debug.LogError($"e: {e}");
        return false;
      }
    }
  }
}