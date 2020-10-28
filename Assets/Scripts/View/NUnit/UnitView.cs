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

    UnitFsm fsm;
    UnitInfo info;
    HealthBar healthBar;
    ManaBar manaBar; //TODO: rename manaUI
  }
}