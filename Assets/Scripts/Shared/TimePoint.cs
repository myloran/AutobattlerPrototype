using System;

namespace Model.NBattleSimulation {
  public struct TimePoint : IComparable<TimePoint> {
    public readonly float Point;
    
    public TimePoint(float point) => Point = point;

    public static implicit operator TimePoint(float value) => new TimePoint(value);
    public static implicit operator float(TimePoint value) => value.Point;

    public int CompareTo(TimePoint other) => Point.CompareTo(other.Point);
  }
}