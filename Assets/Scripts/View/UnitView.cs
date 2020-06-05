using Shared;
using UnityEngine;

namespace View {
  public class UnitView : MonoBehaviour, IUnit {
    public UnitInfo Info;
    public float Height = 0.25f; //TODO: remove when replaced with pivot point
    public EPlayer Player { get; set; }
    public int Level = 1; //TODO: remove  
  }
}