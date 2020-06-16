using TMPro;

namespace View.UIs {
  public class ModelUI : AutoReferencer<ModelUI> {
    public TMP_Text Text;

    public void UpdateText(string text) => Text.text = text;
  }
}