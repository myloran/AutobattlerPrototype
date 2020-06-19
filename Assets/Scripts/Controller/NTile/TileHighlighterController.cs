using System;
using Controller.UnitDrag;
using Shared;
using View.Presenters;
using UniRx;

namespace Controller.NTile {
  public class TileHighlighterController : IDisposable {
    public TileHighlighterController(TilePresenter tilePresenter, 
        UnitDragController untDragController) {
      this.tilePresenter = tilePresenter;
      this.untDragController = untDragController;
    }

    public void Init() {
      untDragController.CoordChanged.Subscribe(ChangeHighlight).AddTo(disposable);
      untDragController.DragEnded.Subscribe(Unhighlight).AddTo(disposable);
    }

    void Unhighlight(DragEndedEvent e) => tilePresenter.TileAt(e.Last).Unhighlight();

    void ChangeHighlight(CoordChangedEvent e) {
      if (e.From != Coord.Invalid)
        tilePresenter.TileAt(e.From).Unhighlight();
        
      tilePresenter.TileAt(e.To).Highlight();
      
      //   //if swap previous, cancel it
      //   if (tile.Unit != null) {
      //     SwapUnits(tile, oldTile);
      //   }
      //   //swap units if tile with unit
    }
    
    public void Dispose() => disposable.Clear();

    readonly TilePresenter tilePresenter;
    readonly UnitDragController untDragController;
    readonly IObservable<CoordChangedEvent> coordChanged;
    readonly IObservable<DragEndedEvent> dragEnded;
    readonly CompositeDisposable disposable = new CompositeDisposable();
  }
}