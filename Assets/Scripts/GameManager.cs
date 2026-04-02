using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int score;
    public UIManager uiManager;

    public void AddScore(int add)
    {
        score += add;
        uiManager.SetScore(score);
    }
}