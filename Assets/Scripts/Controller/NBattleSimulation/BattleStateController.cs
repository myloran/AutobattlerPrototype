using UniRx;
using View;
using View.UIs;

namespace Controller.NBattleSimulation {
  public class BattleStateController {
    public bool IsBattleStarted { get; private set; }

    public BattleStateController(BattleSimulationUI ui) {
      ui.OStart.onValueChanged.AsObservable()
        .Subscribe(SetBattleStarted).AddTo(ui.OStart);
    }

    void SetBattleStarted(bool isOn) => IsBattleStarted = isOn;
  }
}