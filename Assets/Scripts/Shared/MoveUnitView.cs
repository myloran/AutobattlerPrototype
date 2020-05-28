namespace Shared {
  public class MoveUnitView {
    public readonly Coord From;
    public readonly Coord To;
    public readonly float Time;

    public MoveUnitView(Coord from, Coord to, float time) {
      From = from;
      To = to;
      Time = time;
    }
  }
}