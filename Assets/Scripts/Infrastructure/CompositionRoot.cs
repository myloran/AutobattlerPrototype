using System.Linq;
using Controller;
using Model;
using UnityEngine;
using View;
using View.UI;

namespace Infrastructure {
  public class CompositionRoot : MonoBehaviour {
    public BattleSetupUI BattleSetupUI;
    public BoardView BoardView;
    public BenchView BenchView1, BenchView2;
    public UnitView UnitView;
    public TileView TileView;

    void Start() {
      var unitDataLoader = new UnitDataLoader();
      var units = unitDataLoader.Load();
      var players = new[] {new Player(), new Player()};
      var closestTileFinder = new ClosestTileFinder(BoardView, BenchView1, BenchView2);
      var unitViewFactory = new UnitViewFactory(units, UnitView);
      var unitViewFactoryDecorator = new UnitViewFactoryDecorator(closestTileFinder, BattleSetupUI, players, unitViewFactory);
      BattleSetupUI.Init(units.Keys.ToList());
      var tileViewFactory = new TileViewFactory(TileView);
      BoardView.Init(tileViewFactory);
      BenchView1.Init(unitViewFactoryDecorator, tileViewFactory, EPlayer.First);
      BenchView2.Init(unitViewFactoryDecorator, tileViewFactory, EPlayer.Second);

      var benches = new[] {BenchView1, BenchView2};
      var battleSetupController = new BattleSetupController(players, benches, units, BattleSetupUI);
    }
  }
}
