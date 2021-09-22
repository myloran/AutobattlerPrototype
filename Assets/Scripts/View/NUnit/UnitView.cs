using System.Collections.Generic;
using Addons.Assets.src.Scripts;
using Shared.Addons.Examples.FixMath;
using Shared.Primitives;
using UnityEngine;
using View.Exts;
using View.NUnit.States;

namespace View.NUnit {
  public class UnitView : MonoBehaviour {
    public UnitStats Stats;
    public Animator Animator { get; set; }
    public float Height = 0.25f; //TODO: remove when replaced with pivot point
    public EPlayer Player;
    public int Level = 1; 
    
    public UnitView Init(UnitInfo info, EPlayer player, HealthBar healthBar, ManaBar manaBar) {
      this.info = new UnitInfo(info);
      this.healthBar = healthBar;
      this.manaBar = manaBar;
      Player = player;
      Stats = new UnitStats(info);
      fsm = new UnitFsm(this);
#if UNITY_EDITOR
      foreach (var meshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>()) {
        meshRenderer.material = new Material(meshRenderer.material);
        debugMaterials.Add(meshRenderer.material);
        debugMaterialInitialColors.Add(meshRenderer.material.color);
      }
#endif
      return this;
    }

    public void ChangeStateTo(EState state) => fsm.ChangeStateTo(state);
    
    public void ResetState(Vector3 position) {
      ChangeStateTo(EState.Idle);
      transform.position = position;
      transform.rotation = Player.ToQuaternion();
      SetHealth(info.Health);
      SetMana(0);
      this.Show();
    }

    public void SetHealth(float amount) {
      Stats.Health = amount;
      healthBar.SetCurrentHealth(amount);
    }
    
    public void SetMana(float amount) {
      Stats.Mana = amount;
      manaBar.SetCurrentMana(amount);
    }
    
    public void UpdateSilenceDuration(float duration) {
      //TODO: do the thing  
    }
    
    public void Highlight() {
      foreach (var material in debugMaterials) {
        material.color = Color.green;
      }
    }
    
    public void Unhighlight() {
      for (var i = 0; i < debugMaterials.Count; i++) {
        var material = debugMaterials[i];
        material.color = debugMaterialInitialColors[i];
      }
    }

    readonly List<Material> debugMaterials = new List<Material>();
    readonly List<Color> debugMaterialInitialColors = new List<Color>();
    UnitFsm fsm;
    UnitInfo info;
    HealthBar healthBar;
    ManaBar manaBar; //TODO: rename manaUI
  }
}