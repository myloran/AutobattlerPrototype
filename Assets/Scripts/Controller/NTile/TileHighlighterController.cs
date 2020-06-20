using System;
using Controller.UnitDrag;
using Shared;
using Shared.Poco;
using View.Presenters;
using UniRx;
using View.NTile;

namespace Controller.NTile {
  public class TileHighlighterController : IDisposable {
    public TileHighlighterController(TileSpawner tileSpawner, 
        UnitDragController untDragController) {
      this.tileSpawner = tileSpawner;
      this.untDragController = untDragController;
    }

    public void Init() {
      untDragController.CoordChanged.Subscribe(ChangeHighlight).AddTo(disposable);
      untDragController.DragEnded.Subscribe(Unhighlight).AddTo(disposable);
    }

    void Unhighlight(DragEndedEvent e) => tileSpawner.TileAt(e.Last).Unhighlight();

    void ChangeHighlight(CoordChangedEvent e) {
      if (e.From != Coord.Invalid)
        tileSpawner.TileAt(e.From).Unhighlight();
        
      tileSpawner.TileAt(e.To).Highlight();
      
      //   //if swap previous, cancel it
      //   if (tile.Unit != null) {
      //     SwapUnits(tile, oldTile);
      //   }
      //   //swap units if tile with unit
    }
    
    public void Dispose() => disposable.Clear();

    readonly TileSpawner tileSpawner;
    readonly UnitDragController untDragController;
    readonly IObservable<CoordChangedEvent> coordChanged;
    readonly IObservable<DragEndedEvent> dragEnded;
    readonly CompositeDisposable disposable = new CompositeDisposable();
  }
}