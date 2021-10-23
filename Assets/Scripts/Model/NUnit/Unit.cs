using System;
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
    public Unit(HealthComponent health, AttackComponent attack, MovementComponent movement, TargetingComponent targeting,
        AiComponent ai, StatsComponent stats, AbilityComponent ability, SilenceComponent silence, StunComponent stun) {
      this.health = health;
      this.attack = attack;
      this.movement = movement;
      this.targeting = targeting;
      this.ai = ai;
      this.stats = stats;
      this.ability = ability;
      this.silence = silence;
      this.stun = stun;
    }
    
    #region Components

    public F32 Health => health.Health;
    public bool IsAlive => health.IsAlive;
    public void TakeDamage(F32 damage) => health.TakeDamage(damage);
    public void SubToDeath(ITargeting targeting) => health.SubToDeath(targeting);
    public void UnsubFromDeath(ITargeting targeting) => health.UnsubFromDeath(targeting);
    
    public F32 Damage => attack.Damage;
    public F32 AttackAnimationHitTime => attack.AttackAnimationHitTime;
    public F32 TimeToFinishAttackAnimation => attack.TimeToFinishAttackAnimation;
    public F32 AttackSpeedTime => attack.AttackSpeedTime;
    public bool IsRanged => attack.IsRanged;
    public bool CanStartAttack(F32 currentTime) => attack.CanStartAttack(currentTime);
    public bool IsWithinAttackRange(IMovement target) => attack.IsWithinAttackRange(target);
    public F32 ProjectileTravelTimeTo(IMovement target) => attack.ProjectileTravelTimeTo(target);
    public void StartAttack(F32 currentTime) => attack.StartAttack(currentTime);
    public void EndAttack() => attack.EndAttack();

    [JsonIgnore] public IEnumerable<IUnit> ArrivingTargets {
      get => targeting.ArrivingTargets;
      set => targeting.ArrivingTargets = value;
    }

    public IEnumerable<Coord> ArrivingTargetCoords => targeting.ArrivingTargetCoords; //to test determinism
    [JsonIgnore] public IUnit Target => targeting.Target;
    public Coord TargetCoord => targeting.TargetCoord; //to test determinism
    public bool TargetExists => targeting.TargetExists;
    public F32 TauntEndTime => targeting.TauntEndTime;
    public bool IsTaunted(F32 currentTime) => targeting.IsTaunted(currentTime);
    public void Taunt(IUnit unit, F32 tauntEndTime) => targeting.Taunt(unit, tauntEndTime);
    public void ClearTarget() => targeting.ClearTarget();
    public void ChangeTargetTo(IUnit unit) => targeting.ChangeTargetTo(unit);

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
    public bool CanStartMove(F32 currentTime) => movement.CanStartMove(currentTime);
    public void StartMove(F32 endTime) => movement.StartMove(endTime);
    public void FinishMove() => movement.FinishMove();
    public F32 TimeToMove(bool isDiagonal = true) => movement.TimeToMove(isDiagonal);

    public IDecisionTreeNode CurrentDecision => ai.CurrentDecision;
    public F32 DecisionTime => ai.DecisionTime;
    public F32 TimeWhenDecisionWillBeExecuted => ai.TimeWhenDecisionWillBeExecuted;
    public Action<F32> OnDecisionExecutionTimeUpdated {
      get => ai.OnDecisionExecutionTimeUpdated;
      set => ai.OnDecisionExecutionTimeUpdated = value;
    }
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
    public F32 TargetingSqrRange => ability.TargetingSqrRange;
    public F32 AbilitySqrRadius => ability.AbilitySqrRadius;
    public bool HasManaAccumulated => ability.HasManaAccumulated;
    public void AccumulateMana() => ability.AccumulateMana();
    public bool IsWithinAbilityRange(AiContext context) => ability.IsWithinAbilityRange(context);
    public bool CanStartCasting(F32 time) => ability.CanStartCasting(time);
    public void StartCasting(F32 time) => ability.StartCasting(time);
    public void EndCasting() => ability.EndCasting();
    public void CastAbility(AiContext context) => ability.CastAbility(context);
    public void SetAbility(Ability ability) => this.ability.Ability = ability;
    
    public F32 SilenceEndTime => silence.SilenceEndTime;
    public bool IsSilenced(F32 currentTime) => silence.IsSilenced(currentTime);
    public void ApplySilence(F32 endTime) => silence.ApplySilence(endTime);
    
    public F32 StunEndTime => stun.StunEndTime;
    public void ApplyStun(F32 endTime) => stun.ApplyStun(endTime);

    #endregion
    
    public void Reset() {
      health.Reset();
      attack.Reset();
      movement.Reset();
      targeting.Reset();
      ai.Reset();
      stats.Reset();
      ability.Reset();
      silence.Reset();
      stun.Reset();
    }

    public override string ToString() => new StringBuilder()
      .Append(health).Append("\n")
      .Append(attack).Append("\n")
      .Append(movement).Append("\n")
      .Append(ai).Append("\n")
      .Append(stats).Append("\n")
      .Append(ability).Append("\n")
      .Append(silence).Append("\n")
      .Append(targeting).Append("\n")
      .ToString();
    
    readonly HealthComponent health;
    readonly AttackComponent attack;
    readonly TargetingComponent targeting;
    readonly MovementComponent movement;
    readonly AiComponent ai;
    readonly StatsComponent stats;
    readonly AbilityComponent ability;
    readonly SilenceComponent silence;
    readonly StunComponent stun;
  }
}       