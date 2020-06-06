using TMPro;

namespace View.UI {
  public class ModelUI : AutoReferencer<ModelUI> {
    public TMP_Text Text;

    public void UpdateText(string text) => Text.text = text;
  }
}