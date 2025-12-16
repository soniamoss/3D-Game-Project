using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;   // the PauseMenu panel
    public GameObject pauseButton;   // the PauseButton (optional to hide when paused)

    private bool isPaused = false;

    void Update()
    {
        // Optional: also allow ESC key to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);

        if (pauseButton != null)
            pauseButton.SetActive(false);

        Time.timeScale = 0f; // freeze the game
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);

        if (pauseButton != null)
            pauseButton.SetActive(true);

        Time.timeScale = 1f; // unfreeze the game
        isPaused = false;
    }

    public void QuitToTitle()
    {
        // Make sure time runs normally again in the menu
        Time.timeScale = 1f;
        isPaused = false;

        SceneManager.LoadScene("MainMenu");
    }
}
