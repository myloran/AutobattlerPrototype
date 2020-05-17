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
    public BenchView BenchView1, BenchView2;
    public UnitViewFactory UnitViewFactory;

    void Start() {
      var unitDataLoader = new UnitDataLoader();
      var units = unitDataLoader.Load();
      UnitViewFactory.Init(units);
      BattleSetupUI.Init(units.Keys.ToList());
      BenchView1.Init(UnitViewFactory);
      BenchView2.Init(UnitViewFactory);

      var players = new[] {new Player(), new Player()};
      var benches = new[] {BenchView1, BenchView2};
      BattleSetupController.Init(players, benches);
    }
  }
}
