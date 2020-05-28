using Shared;

namespace Model.NUnit {
  public class CStats {
    public int Level;
    public EPlayer Player;
    
    public CStats(int level, EPlayer player) {
      Level = level;
      Player = player;
    }
  }
}