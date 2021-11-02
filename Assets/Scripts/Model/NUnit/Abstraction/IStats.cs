using System.Collections.Generic;
using Shared.Primitives;

namespace Model.NUnit.Abstraction {
  public interface IStats {
    string Name { get; }
    EPlayer Player { get; }
    List<string> Synergies { get; }
    
    bool IsAllyWith(EPlayer player);
  }
}