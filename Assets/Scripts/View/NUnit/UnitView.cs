using Shared;
using Shared.Abstraction;
using UnityEngine;
using View.NUnit.States;

namespace View.NUnit {
  public class UnitView : MonoBehaviour, IUnit {
    public UnitInfo Info;
    public float Height = 0.25f; //TODO: remove when replaced with pivot point
    public EPlayer Player { get; set; }
    public int Level = 1; //TODO: remove  
    public Animator Animator;
    
    public UnitView Init(UnitInfo unitInfo, EPlayer player) {
      Info = unitInfo;
      Player = player;
      Animator = GetComponentInChildren<Animator>();
      var meshRenderer = GetComponent<MeshRenderer>();
      meshRenderer.material = material = new Material(meshRenderer.material);
      material.color = player == EPlayer.First ? Color.blue : Color.red;
      fsm = new UnitFsm(this);

      return this;
    }

    public void ChangeStateTo(EState state) => fsm.ChangeStateTo(state);

    UnitFsm fsm;
    Material material;
  }
}