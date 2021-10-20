using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Primitives;
using View.NUnit;
using View.Presenters;
using static Shared.Addons.Examples.FixMath.F32;

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