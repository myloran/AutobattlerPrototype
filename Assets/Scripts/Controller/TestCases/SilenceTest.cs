using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Primitives;
using View.NUnit;
using View.Presenters;
using static Shared.Addons.Examples.FixMath.F32;

namespace Controller.TestCases {
  public class SilenceTest : IBattleTest {
    public SilenceTest(PlayerContext playerContext, PlayerPresenterContext playerPresenterContext) {
      this.playerContext = playerContext;
      this.playerPresenterContext = playerPresenterContext;
    }

    public void Reset() {            
      playerContext.DestroyAll();
      playerPresenterContext.DestroyAll();
      
      unit1 = playerContext.InstantiateToBoard("Troglodyte", (3, 2), EPlayer.First);
      unitPresenter1 = playerPresenterContext.InstantiateToBoard("Troglodyte", (3, 2), EPlayer.First);
      
      unit2 = playerContext.InstantiateToBoard("Troglodyte", (3, 3), EPlayer.Second);
      unitPresenter2 = playerPresenterContext.InstantiateToBoard("Troglodyte", (3, 3), EPlayer.Second);
    }
    
    public void PrepareState() {
      unit1.Mana = ToF32(100);
      unitPresenter1.SetMana(100);
      
      unit2.Mana = ToF32(90);
      unitPresenter2.SetMana(90);
      
      //execute until cast ability
      //take unit at this point and it should not be silenced
      //take unit at 1 second from it and it should be silenced
      //take unit at 5 second from it and it should not be silenced
    }

    readonly PlayerContext playerContext;
    readonly PlayerPresenterContext playerPresenterContext;
    IUnit unit1, unit2;
    UnitView unitPresenter1, unitPresenter2;
  }
}