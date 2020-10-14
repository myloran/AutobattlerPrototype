using System.Collections.Generic;
using System.Linq;
using Shared.Shared.Client;
using UnityEngine;

namespace View.NUnit {
  public class UnitViewInfoHolder : MonoBehaviour {
    public List<UnitViewInfo> Data;
    public UnitView Prefab;
    
    public Dictionary<string, UnitViewInfo> Infos => Data.ToDictionary(d => d.name);
  }
}