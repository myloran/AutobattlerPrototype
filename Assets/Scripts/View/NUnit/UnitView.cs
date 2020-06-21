using Shared;
using Shared.Poco;
using UnityEngine;
using View.Exts;
using View.NUnit.States;

namespace View.NUnit {
  public class UnitView : MonoBehaviour {
    public UnitInfo Info;
    public Animator Animator; //TODO: make property instead
    HealthBar HealthBar;
    public float Height = 0.25f; //TODO: remove when replaced with pivot point
    public EPlayer Player;
    public int Level = 1; 
    
    public UnitView Init(UnitInfo info, EPlayer player, Camera mainCamera) {
      this.info = new UnitInfo(info);
      Info = info;
      Player = player;
      Animator = GetComponentInChildren<Animator>();
      HealthBar = GetComponentInChildren<HealthBar>()
        .Init(player.ToColor(), Info.Health, mainCamera);
      fsm = new UnitFsm(this);

      return this;
    }

    public void ResetState(Vector3 position) {
      //TODO: reset state?
      transform.position = position;
      SetHealth(info.Health);
      this.Show();
    }

    public void SetHealth(float health) {
      Info.Health = health;
      HealthBar.SetCurrentHealth(health);
    }

    public void ChangeStateTo(EState state) => fsm.ChangeStateTo(state);

    UnitFsm fsm;
    UnitInfo info;
  }
}