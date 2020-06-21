using Shared;
using Shared.Poco;
using UnityEngine;
using View.Exts;
using View.NUnit.States;
using View.Presenters;

namespace View.NUnit {
  public class UnitView : MonoBehaviour {
    public UnitInfo Stats;
    public Animator Animator;
    public float Height = 0.25f; //TODO: remove when replaced with pivot point
    public EPlayer Player;
    public int Level = 1; 
    
    public UnitView Init(UnitInfo info, EPlayer player, HealthBar healthBar) {
      this.info = new UnitInfo(info);
      this.healthBar = healthBar; 
      Stats = info;
      Player = player;
      Animator = GetComponentInChildren<Animator>();
      
      fsm = new UnitFsm(this);

      return this;
    }

    public void ChangeStateTo(EState state) => fsm.ChangeStateTo(state);
    
    public void ResetState(Vector3 position) {
      ChangeStateTo(EState.Idle);
      transform.position = position;
      transform.rotation = Player.ToQuaternion();
      SetHealth(info.Health);
      this.Show();
    }

    public void SetHealth(float health) {
      Stats.Health = health;
      healthBar.SetCurrentHealth(health);
    }

    UnitFsm fsm;
    UnitInfo info;
    HealthBar healthBar;
  }
}