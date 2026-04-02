using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int score;
    public UIManager uiManager;
    public Player player;

    private void Update()
    {
        if (player.isDead)
        {
            StartCoroutine(CoGameOver());
        }
    }

    public void AddScore(int add)
    {
        score += add;
        uiManager.SetScore(score);
    }

    public IEnumerator CoGameOver()
    {
        yield return new WaitForSeconds(3);
        uiManager.GameOverScreenOn();
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}