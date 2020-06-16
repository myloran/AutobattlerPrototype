using Controller.UnitDrag;
using Shared;
using View.Presenters;

namespace Controller.NTile {
  public class TileHighlighter : IHandler<EndDragEvent>, IHandler<CoordChangedEvent> {
    public TileHighlighter(TilePresenter tilePresenter) => this.tilePresenter = tilePresenter;
    
    public void Handle(EndDragEvent e) => tilePresenter.TileAt(e.Last).Unhighlight();

    public void Handle(CoordChangedEvent e) {
      if (e.From != Coord.Invalid)
        tilePresenter.TileAt(e.From).Unhighlight();
        
      tilePresenter.TileAt(e.To).Highlight();
      
      //   //if swap previous, cancel it
      //   if (tile.Unit != null) {
      //     SwapUnits(tile, oldTile);
      //   }
      //   //swap units if tile with unit
    }

    readonly TilePresenter tilePresenter;
  }
}