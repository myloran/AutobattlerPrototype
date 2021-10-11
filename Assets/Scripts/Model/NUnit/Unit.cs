using System.Collections.Generic;
using System.Text;
using Model.NAbility;
using Model.NAI;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Model.NUnit.Components;
using Newtonsoft.Json;
using Shared.Addons.Examples.FixMath;
using Shared.Primitives;

namespace Model.NUnit {
  public class Unit : IUnit {
    public Unit(HealthComponent health, AttackComponent attack, MovementComponent movement, TargetComponent target,
        AiComponent ai, StatsComponent stats, AbilityComponent ability, SilenceComponent silence) {
      this.health = health;
      this.attack = attack;
      this.movement = movement;
      this.target = target;
      this.ai = ai;
      this.stats = stats;
      this.ability = ability;
      this.silence = silence;
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
    public bool IsRanged => attack.IsRanged;
    public bool CanStartAttack(F32 currentTime) => attack.CanStartAttack(currentTime);
    public bool IsWithinAttackRange(IMovement target) => attack.IsWithinAttackRange(target);
    public void StartAttack(F32 currentTime) => attack.StartAttack(currentTime);
    public void EndAttack() => attack.EndAttack();

    [JsonIgnore] public IEnumerable<IUnit> ArrivingTargets {
      get => target.ArrivingTargets;
      set => target.ArrivingTargets = value;
    }

    public IEnumerable<Coord> ArrivingTargetCoords => target.ArrivingTargetCoords; //to test determinism
    [JsonIgnore] public IUnit Target => target.Target;
    public Coord TargetCoord => target.TargetCoord; //to test determinism
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

    public Ability Ability => ability.Ability;
    [JsonIgnore] public IUnit AbilityTarget => ability.AbilityTarget;
    public Coord AbilityTargetCoord => ability.AbilityTargetCoord; //to test determinism
    public F32 Mana {
      get => ability.Mana;
      set => ability.Mana = value;
    }
    public F32 CastHitTime => ability.CastHitTime;
    public F32 TimeToFinishCast => ability.TimeToFinishCast;
    public bool HasManaAccumulated => ability.HasManaAccumulated;
    public void AccumulateMana() => ability.AccumulateMana();
    public bool IsWithinAbilityRange(AiContext context) => ability.IsWithinAbilityRange(context);
    public bool CanStartCasting(F32 time) => ability.CanStartCasting(time);
    public void StartCasting(F32 time) => ability.StartCasting(time);
    public void EndCasting() => ability.EndCasting();
    public void CastAbility(AiContext context) => ability.CastAbility(context);
    public void SetAbility(Ability ability) => this.ability.Ability = ability;
    public bool IsSilenced(F32 currentTime) => silence.IsSilenced(currentTime);
    public F32 SilenceEndTime => silence.SilenceEndTime;
    public void ApplySilence(F32 duration) => silence.ApplySilence(duration);

    #endregion
    
    public void Reset() {
      health.Reset();
      attack.Reset();
      movement.Reset();
      target.Reset();
      ai.Reset();
      stats.Reset();
      ability.Reset();
      silence.Reset();
    }

    public override string ToString() => new StringBuilder()
      .Append(health).Append("\n")
      .Append(attack).Append("\n")
      .Append(movement).Append("\n")
      .Append(ai).Append("\n")
      .Append(stats).Append("\n")
      .Append(ability).Append("\n")
      .Append(silence).Append("\n")
      .Append(target).Append("\n")
      .ToString();
    
    readonly HealthComponent health;
    readonly AttackComponent attack;
    readonly TargetComponent target;
    readonly MovementComponent movement;
    readonly AiComponent ai;
    readonly StatsComponent stats;
    readonly AbilityComponent ability;
    readonly SilenceComponent silence;
  }
}       