using System.Text;
using Shared;
using Shared.Abstraction;

namespace Model.NUnit {
  public class Unit : IUnit {
    public UnitInfo Info;
    public CHealth Health;
    public CAttack Attack;
    public CMovement Movement;
    public CTarget Target;
    public CAi Ai;
    public CStats Stats;
    
    public EPlayer Player { get; set; }
    public bool IsAllyWith(EPlayer player) => Player == player;

    public void Reset() {
      Health.Reset();
      Attack.Reset();
      Movement.Reset();
      Target.Reset();
      Ai.Reset();
      Stats.Reset();
    }

    public override string ToString() => new StringBuilder()
      .Append(Health).Append("\n")
      .Append(Attack).Append("\n")
      .Append(Movement).Append("\n")
      .Append(Ai).Append("\n")
      .Append(Stats).Append("\n")
      .Append(Target).Append("\n")
      .ToString();
  }
}