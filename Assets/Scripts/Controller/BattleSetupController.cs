using Model;
using UnityEngine;
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
      BattleSetupUI.BRemove.onClick.AddListener(RemoveUnit);
    }

    void AddUnit() {
      var unit = new Unit();
      var isAdded = player1.AddUnit(unit);
      if (isAdded) BenchView.AddUnit(BattleSetupUI.GetSelectedUnit);
    }
    
    void RemoveUnit() {
      var isRemoved = player1.RemoveUnit();
      if (isRemoved) BenchView.RemoveUnit();
    }

    void OnDestroy() {
      BattleSetupUI.BAdd.onClick.RemoveListener(AddUnit);
      BattleSetupUI.BRemove.onClick.RemoveListener(RemoveUnit);
    }
  }
}