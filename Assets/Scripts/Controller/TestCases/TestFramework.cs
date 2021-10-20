using Model.NBattleSimulation;
using View.Presenters;

namespace Controller.TestCases {
  public class TestFramework {
    public bool IsOn;

    public TestFramework(PlayerContext playerContext, PlayerPresenterContext playerPresenterContext) {
      battleTest = new SilenceTest(playerContext, playerPresenterContext);
      battleTest = new WithinRadiusTest(playerContext, playerPresenterContext);
    }

    public void Reset() => battleTest.Reset();
    public void PrepareState() => battleTest.PrepareState();

    readonly IBattleTest battleTest; 
  }
}