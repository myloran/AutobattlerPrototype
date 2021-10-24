namespace Controller.TestCases {
  public class TestFramework {
    public bool IsOn;

    //TODO: add dropdown to battle simulation ui to allow select specific test case
    public TestFramework(PlayerSharedContext playerContext) {
      battleTest = new SilenceTest(playerContext);
      battleTest = new WithinRadiusTest(playerContext);
      battleTest = new TauntTest(playerContext);
      battleTest = new StunTest(playerContext);
    }

    public void Reset() => battleTest.Reset();
    public void PrepareState() => battleTest.PrepareState();

    readonly IBattleTest battleTest; 
  }
}