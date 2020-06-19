using Shared;

namespace Model.NUnit {
  public class CStats {
    public string Name;
    public int Level;
    public EPlayer Player;
    
    public CStats(string name, int level, EPlayer player) {
      Level = level;
      Player = player;
      Name = name;
    }

    public void Reset() => Level = 1;

    public override string ToString() => $"{nameof(Name)}: {Name}, {nameof(Level)}: {Level}, {nameof(Player)}: {Player}";
  }
}