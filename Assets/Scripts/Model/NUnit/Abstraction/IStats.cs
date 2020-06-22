using Shared;
using Shared.Primitives;

namespace Model.NUnit.Abstraction {
  public interface IStats {
    string Name { get; }
    EPlayer Player { get; }
    bool IsAllyWith(EPlayer player);
  }
}