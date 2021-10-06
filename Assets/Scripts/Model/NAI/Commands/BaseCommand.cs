using Model.NUnit.Abstraction;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Shared.Primitives;

namespace Model.NAI.Commands {
  public abstract class BaseCommand : ICommand {
    protected BaseCommand(IUnit unit) {
      Unit = unit;
    }
    
    [JsonConverter(typeof(StringEnumConverter))] 
    public virtual ECommand Type { get; } = ECommand.Other;
    [JsonIgnore] public IUnit Unit { get; }
    public Coord Coord => Unit.Coord; //to test determinism
    
    public abstract void Execute();
  }
}