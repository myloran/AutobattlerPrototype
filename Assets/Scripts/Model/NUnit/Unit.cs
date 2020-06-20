using System.Collections.Generic;
using System.Text;
using FixMath;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared;

namespace Model.NUnit {
  //TODO: Use IUnit interface instead of Unit
  public class Unit : IUnit {
    public Unit(CHealth health, CAttack attack, CMovement movement, CTarget target, CAi ai, 
        CStats stats) {
      this.health = health;
      this.attack = attack;
      this.movement = movement;
      this.target = target;
      this.ai = ai;
      this.stats = stats;
    }
    
    public void Reset() {
      health.Reset();
      attack.Reset();
      movement.Reset();
      target.Reset();
      ai.Reset();
      stats.Reset();
    }

    public override string ToString() => new StringBuilder()
      .Append(health).Append("\n")
      .Append(attack).Append("\n")
      .Append(movement).Append("\n")
      .Append(ai).Append("\n")
      .Append(stats).Append("\n")
      .Append(target).Append("\n")
      .ToString();

    #region Components

    public F32 Health => health.Health;
    public bool IsAlive => health.IsAlive;
    public void TakeDamage(F32 damage) => health.TakeDamage(damage);
    public void SubToDeath(ITarget target) => health.SubToDeath(target);
    public void UnsubFromDeath(ITarget target) => health.UnsubFromDeath(target);
    
    public F32 Damage => attack.Damage;
    public F32 AttackAnimationHitTime => attack.AttackAnimationHitTime;
    public F32 TimeToFinishAttackAnimation => attack.TimeToFinishAttackAnimation;
    public F32 AttackSpeedTime => attack.AttackSpeedTime;
    public bool CanStartAttack(F32 currentTime) => attack.CanStartAttack(currentTime);
    public bool IsWithinAttackRange(IMovement target) => attack.IsWithinAttackRange(target);
    public void StartAttack(F32 currentTime) => attack.StartAttack(currentTime);
    public void EndAttack() => attack.EndAttack();
    
    public IUnit Target => target.Target;
    public bool TargetExists => target.TargetExists;
    public void ClearTarget() => target.ClearTarget();
    public void ChangeTargetTo(IUnit unit) => target.ChangeTargetTo(unit);
    public (bool, IUnit) FindNearestTarget(IEnumerable<IUnit> units) => target.FindNearestTarget(units);

    public Coord StartingCoord {
      get => movement.StartingCoord;
      set => movement.StartingCoord = value;
    }
    public Coord TakenCoord {
      get => movement.TakenCoord;
      set => movement.TakenCoord = value;
    }
    public Coord Coord {
      get => movement.Coord;
      set => movement.Coord = value;
    }
    public F32 TimeToMove(bool isDiagonal = true) => movement.TimeToMove(isDiagonal);

    public IDecisionTreeNode CurrentDecision => ai.CurrentDecision;
    public F32 DecisionTime {
      get => ai.DecisionTime;
      set => ai.DecisionTime = value;
    }
    public F32 TimeWhenDecisionWillBeExecuted {
      get => ai.TimeWhenDecisionWillBeExecuted;
      set => ai.TimeWhenDecisionWillBeExecuted = value;
    }
    public bool IsWaiting {
      get => ai.IsWaiting;
      set => ai.IsWaiting = value;
    }
    public void MakeDecision(AiContext context) => ai.MakeDecision(context);
    public void SetDecisionTree(IDecisionTreeNode decisionTree) => ai.SetDecisionTree(decisionTree);
    
    public string Name => stats.Name;
    public EPlayer Player => stats.Player;
    public bool IsAllyWith(EPlayer player) => stats.IsAllyWith(player);

    #endregion
    
    readonly CHealth health;
    readonly CAttack attack;
    readonly CTarget target;
    readonly CMovement movement;
    readonly CAi ai;
    readonly CStats stats;
  }
}