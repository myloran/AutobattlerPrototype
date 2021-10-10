namespace Shared.Abstraction {
  public class CompositeReset : IReset {
    public CompositeReset(params IReset[] resets) {
      this.resets = resets;
    }
    
    public void Reset() {
      foreach (var tick in resets) tick.Reset();
    }

    readonly IReset[] resets;
  }
}