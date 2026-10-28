using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject menu;
    public GameObject levelMenu;

    public void StartGame()
    {
        menu.SetActive(false);
        levelMenu.SetActive(true);
    }
    public void Back()
    {
        menu.SetActive(true);
        levelMenu.SetActive(false);
    }
    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
