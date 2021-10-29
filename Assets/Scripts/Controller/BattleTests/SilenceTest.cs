using Controller.NUnit;
using Shared.Primitives;
using View.NUnit;

namespace Controller.TestCases {
  public class SilenceTest : IBattleTest {
    public SilenceTest(PlayerSharedContext playerContext) {
      this.playerContext = playerContext;
    }

    public void Reset() {            
      playerContext.DestroyAll();
      
      unit1 = playerContext.InstantiateToBoard("Troglodyte", (3, 2), EPlayer.First);
      unit2 = playerContext.InstantiateToBoard("Troglodyte", (3, 3), EPlayer.Second);
    }
    
    public void PrepareState() {
      unit1.SetMana(100);
      unit2.SetMana(90);
      
      //execute until cast ability
      //take unit at this point and it should not be silenced
      //take unit at 1 second from it and it should be silenced
      //take unit at 5 second from it and it should not be silenced
    }

    readonly PlayerSharedContext playerContext;
    UnitContext unit1, unit2;
    UnitView unitPresenter1, unitPresenter2;
  }
}