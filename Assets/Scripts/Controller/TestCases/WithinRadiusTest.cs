using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Primitives;
using View.NUnit;
using View.Presenters;
using static Shared.Addons.Examples.FixMath.F32;

namespace Controller.TestCases {
  public class WithinRadiusTest : IBattleTest {
    public WithinRadiusTest(PlayerContext playerContext, PlayerPresenterContext playerPresenterContext) {
      this.playerContext = playerContext;
      this.playerPresenterContext = playerPresenterContext;
    }

    public void Reset() {
      playerContext.DestroyAll();
      playerPresenterContext.DestroyAll();

      var unitName = "BattleDwarf";
      var firstPosition = (3, 2);
      var firstPlayer = EPlayer.First;
      unit1 = playerContext.InstantiateToBoard(unitName, firstPosition, firstPlayer);
      unitPresenter1 = playerPresenterContext.InstantiateToBoard(unitName, firstPosition, firstPlayer);

      var secondPosition = (3, 5);
      var secondPlayer = EPlayer.Second;
      unit2 = playerContext.InstantiateToBoard(unitName, secondPosition, secondPlayer);
      unitPresenter2 = playerPresenterContext.InstantiateToBoard(unitName, secondPosition, secondPlayer);
    }

    public void PrepareState() {
      unit1.Mana = ToF32(100);
      unitPresenter1.SetMana(100);

      unit2.Mana = ToF32(0);
      unitPresenter2.SetMana(0);

      //execute ability
      //make sure ability applied damage
    }

    readonly PlayerContext playerContext;
    readonly PlayerPresenterContext playerPresenterContext;
    IUnit unit1, unit2;
    UnitView unitPresenter1, unitPresenter2;
  }
}
  
