namespace View {
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
//using UnityEngine.AddressableAssets;

public class AutoReferencer<T> : MonoBehaviour where T : Component {
  const char Separator = '#';
  const int MaxDepth = 10;

  protected virtual void Reset() {
    //FindButtons();
    SetReferences();
  }

  public void SetReferences() {
    foreach (var field in typeof(T).GetFields()) { //.Where(_ => _.GetValue(this) == null)
      var names = transform.name.Split(Separator);
      foreach (var name in names)
        SetReference(field, name);
    }

    void SetReference(FieldInfo field, string name) {
//      if (field.FieldType == typeof(AssetReference)) {
//        SetAssetReference(field);
//        return;
//      }
      
      var obj = field.Name == name 
        ? transform
        : FindReference(field, new List<Transform> {transform}, MaxDepth);
      if (obj == null) return;

      try {
        if (field.FieldType == typeof(GameObject))
          field.SetValue(this, obj.gameObject);
        else
          field.SetValue(this, obj.GetComponent(field.FieldType));
      }
      catch (System.Exception ex) {
        Debug.LogWarning($"obj.name {obj.name} has no component of type {field.FieldType} ex: {ex}");
      }
    }
  }

//  void SetAssetReference(FieldInfo field) { //Think how can you optimize it
//    var guids = AssetDatabase.FindAssets(field.Name, new[] {"Assets/Addressables"});
//    if (guids.Length <= 0) return;
//    
//    var reference = new AssetReference(guids[0]);
//    field.SetValue(this, reference);
//  }

  Transform FindReference(FieldInfo field, List<Transform> children, int depth) {
    foreach (var child in children) {
      var subChild = Enumerable.Range(0, child.childCount)
        .Select(_ => child.GetChild(_))
        .FirstOrDefault(_ => _.name.Split(Separator).Contains(field.Name));
      if (subChild != null) return subChild;
    }

    var newChildren = new Transform[children.Count];
    children.CopyTo(newChildren);
    children.Clear();

    foreach (var child in newChildren)
      for (var j = 0; j < child.childCount; j++)
        children.Add(child.GetChild(j));

    return --depth >= 0 ? FindReference(field, children, depth) : null;
  }
}
}