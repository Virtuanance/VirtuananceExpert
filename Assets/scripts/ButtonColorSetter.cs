using UnityEngine;
using UnityEngine.UI;

public class ButtonColorSetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button b = this.gameObject.GetComponent<Button>();
        if (b != null)
        {
            b.transition = Selectable.Transition.ColorTint;
            ColorBlock x = b.colors;
            x.colorMultiplier = 1;
            x.fadeDuration = 0.1f;
            x.normalColor = new Color(0.75f, 0.75f, 0.75f, 0.96f);
            x.highlightedColor = new Color(0.7725f, 0.7725f, 0.7725f, 1);
            x.pressedColor = Color.white;
            x.disabledColor = Color.gray;
            x.selectedColor = new Color(0.75f, 0.75f, 0.75f, 0.96f);
            RectTransform rt = b.gameObject.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 250);
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 75);
            }

            b.colors = x;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
