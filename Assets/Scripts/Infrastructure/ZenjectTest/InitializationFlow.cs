using Shared.Primitives;
using UnityEngine;
using View.NUnit;
using Zenject;

namespace Infrastructure.ZenjectTest {
  public class InitializationFlow : MonoBehaviour, IInitializable {
    [Inject]
    void Construct(InfoLoader<UnitInfo> unitInfoLoader, UnitViewFactory unitViewFactory) {
      this.unitInfoLoader = unitInfoLoader;
      log.Info("2");
      this.unitViewFactory = unitViewFactory;
    }
    
    public void Initialize() {
      log.Info("3");
      unitInfoLoader.Load("Units");
      unitViewFactory.Create("Justicar", (0, 0), EPlayer.First);
    }
    
    UnitViewFactory unitViewFactory;
    InfoLoader<UnitInfo> unitInfoLoader;
    static readonly Shared.Addons.OkwyLogging.Logger log = Shared.Addons.OkwyLogging.MainLog.GetLogger(nameof(InitializationFlow));
  }
}