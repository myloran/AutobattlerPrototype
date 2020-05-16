using System;
using Model;
using UnityEngine;
using UnityEngine.Serialization;
using View;
using View.UI;

namespace Controller {
  public class BattleSetupController : MonoBehaviour {
    public BattleSetupUI BattleSetupUI;
    public BenchView BenchView;
    Player player1, player2;

    public void Init(Player player1, Player player2) {
      this.player1 = player1;
      this.player2 = player2;
      BattleSetupUI.BAdd.onClick.AddListener(AddUnit);
    }

    void AddUnit() {
      var unit = new Unit();
      var isAdded = player1.AddUnit(unit);
      if (isAdded) BenchView.AddUnit();
    }

    void OnDestroy() {
      BattleSetupUI.BAdd.onClick.RemoveListener(AddUnit);
    }
  }
}