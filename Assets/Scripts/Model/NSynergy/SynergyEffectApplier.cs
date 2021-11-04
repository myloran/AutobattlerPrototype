using System.Collections.Generic;
using System.Linq;
using Model.NAbility;
using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Primitives;

namespace Model.NSynergy {
  public class SynergyEffectApplier {
    public SynergyEffectApplier(Board board, AiContext context, Dictionary<string, SynergyInfo> synergyInfos,
        Dictionary<string, EffectInfo> effectInfos, EffectFactory effectFactory) {
      this.effectFactory = effectFactory;
      this.synergyInfos = synergyInfos;
      this.effectInfos = effectInfos;
      this.board = board;
      this.context = context;
    }

    public void Init() {
      foreach (var synergy in synergyInfos) {
        foreach (var level in synergy.Value.SynergyLevels) {
          var effectInfo = effectInfos[level.EffectName];
          synergyEffects[level.EffectName] = effectFactory.Create(effectInfo);
        }
      }
    }
    
    public void ApplyEffects() {
      ApplyEffect(EPlayer.First);
      ApplyEffect(EPlayer.Second);
    }
    
    void ApplyEffect(EPlayer player) {
      var firstPlayerUnits = board.GetPlayerUnits(player).ToList();
      var synergiesCount = CalculateSynergyCount(firstPlayerUnits);
      ApplySynergyEffect(firstPlayerUnits, synergiesCount);
    }
    
    Dictionary<string, int> CalculateSynergyCount(List<IUnit> firstPlayerUnits) {
      var synergiesCount = new Dictionary<string, int>();
      var unitsCounted = new HashSet<string>();

      foreach (var unit in firstPlayerUnits) {
        if (!unitsCounted.Add(unit.Name)) continue;
        
        foreach (var synergy in unit.Synergies) {
          if (!synergiesCount.ContainsKey(synergy)) synergiesCount[synergy] = 0;
          synergiesCount[synergy]++;
        }
      }

      return synergiesCount;
    }

    void ApplySynergyEffect(List<IUnit> firstPlayerUnits, Dictionary<string, int> synergiesCount) {
      foreach (var synergyCount in synergiesCount) {
        if (!synergyInfos.ContainsKey(synergyCount.Key)) {
          log.Error($"Synergy {synergyCount.Key} is missing!");
          continue;
        }
        
        var info = synergyInfos[synergyCount.Key];
        var levels = info.SynergyLevels.OrderByDescending(l => l.UnitCount);

        foreach (var level in levels) {
          if (synergyCount.Value >= level.UnitCount) {
            var effect = synergyEffects[level.EffectName];
            effect.Apply(context, firstPlayerUnits);
            break;
          }
        }
      }
    }

    
    readonly Dictionary<string, IEffect> synergyEffects = new Dictionary<string, IEffect>();
    readonly Dictionary<string, SynergyInfo> synergyInfos;
    readonly Dictionary<string, EffectInfo> effectInfos;
    readonly Board board;
    readonly AiContext context;
    readonly EffectFactory effectFactory;
    static readonly Shared.Addons.OkwyLogging.Logger log = Shared.Addons.OkwyLogging.MainLog.GetLogger(nameof(SynergyEffectApplier));
  }
}