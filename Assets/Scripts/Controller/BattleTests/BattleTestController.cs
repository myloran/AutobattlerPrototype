using System.Collections.Generic;
using System.Linq;
using Controller.Exts;
using UniRx;
using View.UIs;

namespace Controller.TestCases {
  public class BattleTestController {
    public BattleTestController(BattleSimulationUI ui, List<IBattleTest> battleTests) {
      this.ui = ui;
      this.battleTests = battleTests;
    }
    
    public void SubToUI() {
      ui.SetBattleTests(battleTests.Select(t => t.GetType().Name));
      ui.OTestMode.OnValueChangedAsObservable().Subscribe(SetTestMode).AddTo(ui.OPause);
      ui.DBattleTest.OnValueChangedAsObservable().Subscribe(SetBattleTest).AddTo(ui.DBattleTest);
    }
    
    void SetTestMode(bool isTestMode) => isOn = isTestMode;
    void SetBattleTest(int index) => SelectBattleTest(battleTests[index]);
    void SelectBattleTest(IBattleTest battleTest) => selectedBattleTest = battleTest;
    
    public void Reset() {
      if (isOn) selectedBattleTest.Reset();
    }

    public void PrepareState() {
      if (isOn) selectedBattleTest.PrepareState();
    }

    readonly List<IBattleTest> battleTests;
    readonly BattleSimulationUI ui;
    IBattleTest selectedBattleTest;
    bool isOn;
  }
}