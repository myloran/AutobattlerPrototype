using System;
using Controller.UnitDrag;
using Shared;
using Shared.Primitives;
using View.Presenters;
using UniRx;
using View.NTile;

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
    }
    
    public void Dispose() => disposable.Clear();

    readonly TilePresenter tilePresenter;
    readonly UnitDragController untDragController;
    readonly CompositeDisposable disposable = new CompositeDisposable();
  }
}