using Controller.Exts;
using Model.NBattleSimulation;
using Shared.Abstraction;
using Shared.Shared.Client;
using UniRx;
using View.NUnit;
using View.Presenters;
using View.UIs;
using View.Views;

namespace Controller.NDebug {
  public class BattleSimulationDebugController {
    public BattleSimulationDebugController(BattleSimulation simulation, BattleSimulationUI ui,
        ISimulationTick viewSimulation, AiContext context, Player[] players,
        BaseBoard<UnitView, PlayerPresenter> boardPresenter, PlayerPresenter[] playerPresenters,
        RealtimeBattleSimulationController realtimeBattleSimulationController) {
      this.simulation = simulation;
      this.ui = ui;
      this.viewSimulation = viewSimulation;
      this.context = context;
      this.players = players;
      this.boardPresenter = boardPresenter;
      this.playerPresenters = playerPresenters;
      this.realtimeBattleSimulationController = realtimeBattleSimulationController;

      ui.OStartBattle.onValueChanged.AsObservable().Where(b => b)
        .Subscribe(_ => StartBattle()).AddTo(ui.OStartBattle);
      ui.BExecuteNextDecision.Sub(ExecuteNextDecision);
      ui.BExecuteAllDecisions.Sub(ExecuteAllDecisions);
      ui.BExecuteInRealtime.Sub(PlayerBattleInRealtime);
    }
    
    void StartBattle() {
      simulation.PrepareBattle(players[0], players[1]);
      boardPresenter.Reset(playerPresenters[0], playerPresenters[1]);
      ui.SetEnabled(!simulation.IsBattleOver);
    }

    void ExecuteNextDecision() {
      simulation.ExecuteNextCommand();
      viewSimulation.SimulationTick(context.CurrentTime.Float);

      if (!simulation.IsBattleOver) return;
      
      ui.Disable();
    }

    void ExecuteAllDecisions() {
      while (!simulation.IsBattleOver) {
        ExecuteNextDecision();
      }
    }
    
    void PlayerBattleInRealtime() => realtimeBattleSimulationController.StartBattle();

    readonly BattleSimulation simulation;
    readonly BattleSimulationUI ui;
    readonly ISimulationTick viewSimulation;
    readonly AiContext context;
    readonly Player[] players;
    readonly BaseBoard<UnitView, PlayerPresenter> boardPresenter;
    readonly PlayerPresenter[] playerPresenters;
    readonly RealtimeBattleSimulationController realtimeBattleSimulationController;
  }
}