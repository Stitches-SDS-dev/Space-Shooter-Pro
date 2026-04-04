using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;

    private void Update()
    {
        if (_isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.M)) MainMenu();
            if (Input.GetKeyDown(KeyCode.R)) RestartGame();
        }
    }

    public void SetGameOver()
    {
        _isGameOver = true;
    }

    private void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
}
