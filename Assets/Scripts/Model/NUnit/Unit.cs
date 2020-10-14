using System.Collections.Generic;
using System.Text;
using Model.NAbility;
using Model.NAI;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Model.NUnit.Components;
using Shared.Addons.Examples.FixMath;
using Shared.Primitives;

namespace Model.NUnit {
  public class Unit : IUnit {
    public Unit(HealthComponent health, AttackComponent attack, MovementComponent movement, TargetComponent target,
        AiComponent ai, StatsComponent stats, AbilityComponent ability) {
      this.health = health;
      this.attack = attack;
      this.movement = movement;
      this.target = target;
      this.ai = ai;
      this.stats = stats;
      this.ability = ability;
    }
    
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

    public IEnumerable<IUnit> ArrivingTargets {
      get => target.ArrivingTargets;
      set => target.ArrivingTargets = value;
    }
    public IUnit Target => target.Target;
    public bool TargetExists => target.TargetExists;
    public void ClearTarget() => target.ClearTarget();
    public void ChangeTargetTo(IUnit unit) => target.ChangeTargetTo(unit);
    public IUnit FindNearestTarget(IEnumerable<IUnit> units) => target.FindNearestTarget(units);

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
    public MoveInfo NextMove {
      get => movement.NextMove;
      set => movement.NextMove = value;
    }
    public F32 TimeToMove(bool isDiagonal = true) => movement.TimeToMove(isDiagonal);

    public IDecisionTreeNode CurrentDecision => ai.CurrentDecision;
    public F32 DecisionTime => ai.DecisionTime;
    public F32 TimeWhenDecisionWillBeExecuted => ai.TimeWhenDecisionWillBeExecuted;
    public void MakeDecision(AiContext context) => ai.MakeDecision(context);
    public void SetDecisionTime(F32 currentTime, F32 time) => ai.SetDecisionTime(currentTime, time);
    public void SetDecisionTree(IDecisionTreeNode decisionTree) => ai.SetDecisionTree(decisionTree);
    
    public string Name => stats.Name;
    public EPlayer Player => stats.Player;
    public bool IsAllyWith(EPlayer player) => stats.IsAllyWith(player);
    
    public bool HasManaAccumulated => ability.HasManaAccumulated;
    public void AccumulateMana() => ability.AccumulateMana();
    public bool IsWithinAbilityRange(IMovement movement) => ability.IsWithinAbilityRange(movement);
    public void StartCastingAbility(F32 currentTime) => ability.StartCastingAbility(currentTime);
    public void EndCastingAbility() => ability.EndCastingAbility();

    #endregion
    
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
    
    readonly HealthComponent health;
    readonly AttackComponent attack;
    readonly TargetComponent target;
    readonly MovementComponent movement;
    readonly AiComponent ai;
    readonly StatsComponent stats;
    readonly AbilityComponent ability;
  }
}       