using System.Linq;
using Controller;
using Model;
using UnityEngine;
using View;
using View.UI;

namespace Infrastructure {
  public class CompositionRoot : MonoBehaviour {
    public BattleSetupController BattleSetupController;
    public BattleSetupUI BattleSetupUI;
    public BenchView BenchView;
    public UnitViewFactory UnitViewFactory;

    void Start() {
      var unitDataLoader = new UnitDataLoader();
      var units = unitDataLoader.Load();
      UnitViewFactory.Init(units);
      BattleSetupUI.Init(units.Keys.ToList());
      BenchView.Init(UnitViewFactory);
      
      var player1 = new Player();
      var player2 = new Player();
      BattleSetupController.Init(player1, player2);
    }
  }
}
