using Model.NUnit.Abstraction;
using Shared;

namespace Model.NUnit {
  public class CStats : IStats {
    public string Name { get; }
    public EPlayer Player { get; }
    
    public CStats(string name, int level, EPlayer player) {
      this.level = level;
      Player = player;
      Name = name;
    }
    
    public bool IsAllyWith(EPlayer player) => Player == player;
    public void Reset() => level = 1;

    public override string ToString() => $"{nameof(Name)}: {Name}, {nameof(level)}: {level}, {nameof(Player)}: {Player}";
    
    int level;
  }
}