using Shared;
using UnityEngine;

namespace View {
  public class UnitView : MonoBehaviour, IUnit {
    public UnitInfo Info;
    public float Height = 0.25f; //TODO: remove when replaced with pivot point
    public EPlayer Player { get; set; }
    public int Level = 1; //TODO: remove  
    
    public UnitView Init(UnitInfo unitInfo, EPlayer player) {
      Info = unitInfo;
      Player = player;
      var meshRenderer = GetComponent<MeshRenderer>();
      meshRenderer.material = material = new Material(meshRenderer.material);
      material.color = player == EPlayer.First ? Color.blue : Color.red;
      return this;
    }
  
    Material material;
  }
}