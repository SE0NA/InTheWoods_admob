using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchSliderText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mytext;
    [SerializeField] Slider myslider;

    public void SetText()
    {
        mytext.text = myslider.value.ToString();
    }
}
