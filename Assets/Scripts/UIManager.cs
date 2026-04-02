using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI scoreText;
    public Image hitScreen;
    public GameObject gameOverScreen;

    private void Awake()
    {
        hitScreen.enabled = false;
        gameOverScreen.SetActive(false);
    }

    public void SetSlider(int value)
    {
        slider.value = value;
    }

    public void SetScore(int value)
    {
        scoreText.text = $"Score: {value}";
    }

    public void HitScreenOnOff()
    {
        StartCoroutine(CoHitScreenOnOff());
    }

    public IEnumerator CoHitScreenOnOff()
    {
        hitScreen.enabled = true;
        yield return new WaitForSeconds(0.04f);
        hitScreen.enabled = false;
    }

    public void GameOverScreenOn()
    {
        gameOverScreen.SetActive(true);
    }
}
