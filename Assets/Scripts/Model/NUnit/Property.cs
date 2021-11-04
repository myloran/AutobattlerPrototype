using Shared.Addons.Examples.FixMath;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NUnit {
  public class Property {
    public Property() { }
    
    public Property(F32 value) {
      this.value = value;
      unclampedValue = value;
      resetValue = value;
    }
    
    public Property Modify(F32 amount) {
      unclampedValue += amount;
      value = unclampedValue;
      value = Clamp(value, Zero, maxChance);
      return this;
    }
    
    public void Reset() {
      unclampedValue = resetValue;
      value = resetValue;
    }
    
    public static implicit operator Property(F32 value) => new Property(value);
    public static implicit operator F32 (Property property) => property.value;
    public override string ToString() => $"{nameof(value)}: {value}";

    static readonly F32 maxChance = ToF32(100f);
    F32 value;
    F32 unclampedValue;
    readonly F32 resetValue;
  }
}