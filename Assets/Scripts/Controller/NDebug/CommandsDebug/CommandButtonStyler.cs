using System.Collections.Generic;
using Model.NUnit.Abstraction;
using UnityEngine;
using UnityEngine.UIElements;
using View.Presenters;

namespace Controller.NDebug.CommandsDebug {
  public class CommandButtonStyler {
    public CommandButtonStyler(BoardPresenter boardPresenter) {
      this.boardPresenter = boardPresenter;
    }

    public string SimplifyCommandName(string name) {
      var index = name.LastIndexOf('C');
      return index > 0 
        ? name.Substring(0, index) 
        : name;
    }
    
    void HighlightButton(Button button) {
      defaultButtonBackgroundColor = button.style.backgroundColor; 
      button.style.backgroundColor = new StyleColor(Color.green);
    }

    void UnhighlightButton(Button button) => button.style.backgroundColor = defaultButtonBackgroundColor;
                  
    public void Highlight(List<Button> buttons, IUnit unit) {
      var unitView = boardPresenter.TryGetUnit(unit.Coord);
      if (unitView == null) return;
      
      unitView.Highlight();
      foreach (var b in buttons) HighlightButton(b);
    }

    public void Unhighlight(List<Button> buttons, IUnit unit) {
      var unitView = boardPresenter.TryGetUnit(unit.Coord);
      if (unitView == null) return;
      
      unitView.Unhighlight();
      foreach (var b in buttons) UnhighlightButton(b);
    }
    
    readonly BoardPresenter boardPresenter;
    StyleColor defaultButtonBackgroundColor;
  }
}