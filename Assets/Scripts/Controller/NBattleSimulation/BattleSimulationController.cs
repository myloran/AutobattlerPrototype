using Model.NBattleSimulation;
using Shared.Shared.Client;
using UniRx;
using View;
using View.Exts;

namespace Controller.NBattleSimulation {
  public class BattleSimulationController {
    public bool IsBattleStarted;

    public BattleSimulationController(BattleSimulation simulation, BattleSimulationUI ui,
        ISimulationTick viewSimulation, AiContext context, Player[] players) {
      this.simulation = simulation;
      this.ui = ui;
      this.viewSimulation = viewSimulation;
      this.context = context;
      this.players = players;
      ui.OStartBattle.onValueChanged.AsObservable().Subscribe(StartBattle).AddTo(ui.OStartBattle);
      ui.BExecuteNextDecision.Sub(ExecuteNextDecision);
      ui.BExecuteAllDecisions.Sub(ExecuteAllDecisions);
    }

    void StartBattle(bool isOn) {
      IsBattleStarted = isOn;
      if (!isOn) return;
      
      simulation.PrepareBattle(players[0], players[1]);
      ui.BExecuteNextDecision.interactable = !simulation.IsBattleOver;
      ui.BExecuteAllDecisions.interactable = !simulation.IsBattleOver;
    }

    void ExecuteNextDecision() {
      simulation.ExecuteNextDecision();
      // viewSimulation.SimulationTick(context.CurrentTime);

      if (simulation.IsBattleOver) {
        ui.BExecuteNextDecision.Disable();
        ui.BExecuteAllDecisions.Disable();
      } 
    }

    void ExecuteAllDecisions() {
      while (!simulation.IsBattleOver) {
        ExecuteNextDecision();
      }
    }
    
    readonly BattleSimulation simulation;
    readonly BattleSimulationUI ui;
    readonly ISimulationTick viewSimulation;
    readonly AiContext context;
    readonly Player[] players;
  }
}