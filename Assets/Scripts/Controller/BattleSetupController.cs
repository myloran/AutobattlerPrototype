using Model;
using UnityEngine;
using View;
using View.UI;

namespace Controller {
  public class BattleSetupController : MonoBehaviour {
    public BattleSetupUI BattleSetupUI;
    BenchView[] benches;
    Player[] players;

    public void Init(Player[] players, BenchView[] benches) {
      this.players = players;
      this.benches = benches;
      BattleSetupUI.BAdd.onClick.AddListener(AddUnit);
      BattleSetupUI.BRemove.onClick.AddListener(RemoveUnit);
    }

    void AddUnit() {
      var unit = new Unit();
      var id = BattleSetupUI.GetSelectedPlayerId;
      var isAdded = players[id].AddUnit(unit);
      if (isAdded) benches[id].AddUnit(BattleSetupUI.GetSelectedUnitName);
    }
    
    void RemoveUnit() {
      var id = BattleSetupUI.GetSelectedPlayerId;
      var isRemoved = players[id].RemoveUnit();
      if (isRemoved) benches[id].RemoveUnit();
    }

    void OnDestroy() {
      BattleSetupUI.BAdd.onClick.RemoveListener(AddUnit);
      BattleSetupUI.BRemove.onClick.RemoveListener(RemoveUnit);
    }
  }
}