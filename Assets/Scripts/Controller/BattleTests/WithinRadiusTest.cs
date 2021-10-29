using Controller.NUnit;
using Shared.Primitives;
using View.NUnit;

namespace Controller.TestCases {
  public class WithinRadiusTest : IBattleTest {
    public WithinRadiusTest(PlayerSharedContext playerContext) {
      this.playerContext = playerContext;
    }

    public void Reset() {
      playerContext.DestroyAll();

      var unitName = "BattleDwarf";
      unit1 = playerContext.InstantiateToBoard(unitName, (3, 2), EPlayer.First);
      unit2 = playerContext.InstantiateToBoard(unitName, (3, 5), EPlayer.Second);
    }

    public void PrepareState() {
      unit1.SetMana(100);

      //execute ability
      //make sure ability applied damage
    }

    readonly PlayerSharedContext playerContext;
    UnitContext unit1, unit2;
    UnitView unitPresenter1, unitPresenter2;
  }
}
  
