using System.Collections.Generic;
using Shared;
using UnityEngine;

namespace View {
  public class UnitViewFactory : MonoBehaviour {
    public UnitView UnitPrefab;

    public void Init(Dictionary<string, UnitInfo> unitInfos) {
      this.unitInfos = unitInfos;
    }

    public UnitView Create(string name, Vector3 position) {
      var unit = Instantiate(UnitPrefab, position + new Vector3(0, 0.25f, 0), Quaternion.identity);
      unit.Info = unitInfos[name];
      return unit;
    }
    
    Dictionary<string, UnitInfo> unitInfos;
  }
}