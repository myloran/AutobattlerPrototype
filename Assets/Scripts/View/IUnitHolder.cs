namespace View {
  public interface IUnitHolder {
    void Place(UnitView unit, TileView tile);
    EUnitHolder Type { get; }
  }
}