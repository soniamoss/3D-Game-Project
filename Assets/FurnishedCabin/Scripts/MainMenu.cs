using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Demo"); // Your game scene
    }

    public void GoToTitlePage()
    {
        SceneManager.LoadScene("TitlePage"); // Your title page scene
        Debug.Log("Going back to Title Page");
    }
}
