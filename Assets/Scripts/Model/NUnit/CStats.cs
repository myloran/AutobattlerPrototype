using Shared;

namespace Model.NUnit {
  public class CStats {
    public int Level;
    public EPlayer Player;
    
    public CStats(int level, EPlayer player) {
      Level = level;
      Player = player;
    }

    public void Reset() => Level = 1;

    public override string ToString() => $"{nameof(Level)}: {Level}, {nameof(Player)}: {Player}";
  }
}