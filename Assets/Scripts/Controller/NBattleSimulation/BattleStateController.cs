using UniRx;
using View;
using View.UIs;

namespace Controller.NBattleSimulation {
  public class BattleStateController {
    public bool IsBattleStarted { get; private set; }

    public BattleStateController(BattleSimulationUI ui) {
      ui.OStartBattle.onValueChanged.AsObservable()
        .Subscribe(SetBattleStarted).AddTo(ui.OStartBattle);
    }

    void SetBattleStarted(bool isOn) => IsBattleStarted = isOn;
  }
}