using System.Collections.Generic;
using Model.NUnit.Abstraction;
using Shared.Primitives;

namespace Model.NUnit.Components {
  public class StatsComponent : IStats {
    public string Name { get; }
    public EPlayer Player { get; }
    public List<string> Synergies { get; }
    
    public StatsComponent(string name, int level, EPlayer player, List<string> infoSynergyNames) {
      this.level = level;
      Synergies = infoSynergyNames;
      Player = player;
      Name = name;
    }
    
    public bool IsAllyWith(EPlayer player) => Player == player;
    public void Reset() => level = 1;

    public override string ToString() => $"{nameof(Name)}: {Name}, {nameof(level)}: {level}, {nameof(Player)}: {Player}";
    
    int level;
  }
}