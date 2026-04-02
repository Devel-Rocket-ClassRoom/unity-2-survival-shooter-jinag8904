using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI scoreText;

    public void SetSlider(int value)
    {
        slider.value = value;
    }

    public void SetScore(int value)
    {
        scoreText.text = $"Score: {value}";
    }
}
