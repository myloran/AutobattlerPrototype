using System;
using Controller;
using Model;
using UnityEngine;

namespace Infrastructure {
  public class CompositionRoot : MonoBehaviour {
    public BattleSetupController BattleSetupController;

    void Start() {
      var player1 = new Player();
      var player2 = new Player();
      BattleSetupController.Init(player1, player2);
    }
  }
}
