using System;
using Shared;
using Shared.Abstraction;
using UnityEngine;
using View.Exts;
using View.NUnit.States;

namespace View.NUnit {
  public class UnitView : MonoBehaviour, IUnit {
    public UnitInfo Info;
    public Animator Animator;
    HealthBar HealthBar;
    public float Height = 0.25f; //TODO: remove when replaced with pivot point
    public EPlayer Player { get; set; }
    public int Level = 1; //TODO: remove  
    
    public UnitView Init(UnitInfo info, EPlayer player) {
      this.info = new UnitInfo(info);
      Info = info;
      Player = player;
      Animator = GetComponentInChildren<Animator>();
      HealthBar = GetComponentInChildren<HealthBar>()
        .Init(player.ToColor(), Info.Health);
      
      var meshRenderer = GetComponent<MeshRenderer>();
      meshRenderer.material = material = new Material(meshRenderer.material);
      material.color = player.ToColor();
      fsm = new UnitFsm(this);

      return this;
    }

    public void ResetHealth() => SetHealth(info.Health);

    public void SetHealth(float health) {
      Info.Health = health;
      HealthBar.SetCurrentHealth(health);
    }

    void Update() {
      HealthBar.transform.LookAt(Camera.main.transform);
      var rotation = HealthBar.transform.rotation.eulerAngles;
      HealthBar.transform.rotation = Quaternion.Euler(new Vector3(rotation.x + 90, 0, 0));
    }

    public void ChangeStateTo(EState state) => fsm.ChangeStateTo(state);

    UnitFsm fsm;
    Material material;
    UnitInfo info;
  }
}