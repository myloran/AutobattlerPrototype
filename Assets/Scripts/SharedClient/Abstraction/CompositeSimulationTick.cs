namespace SharedClient.Abstraction {
  public class CompositeSimulationTick : ISimulationTick {
    public CompositeSimulationTick(params ISimulationTick[] simulationTicks) {
      this.simulationTicks = simulationTicks;
    }

    public void SimulationTick(float time) {
      foreach (var tick in simulationTicks) tick.SimulationTick(time);
    }

    readonly ISimulationTick[] simulationTicks;
  }
}