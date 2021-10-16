using System.Collections.Generic;
using System.Linq;
using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Exts;

namespace Model.NAbility {
  public class SingleTargetsSelector : IAdditionalTargetsSelector {
    public IEnumerable<IUnit> Select(IUnit target, AiContext context) => 
      target != null ? target.AsEnumerable() : Enumerable.Empty<IUnit>();
  }
}