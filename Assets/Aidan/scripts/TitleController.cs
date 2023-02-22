using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    bool started;
    [SerializeField] GameObject cam;

    public void StartGame()
    {
        if (started) return;
        started = true;

        cam.GetComponent<AudioListener>().enabled = false;
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    public void ShowCredits()
    {
        SceneManager.LoadScene(8, LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Camera.main != null) cam.SetActive(false);

        if (!started || !SceneManager.GetSceneByBuildIndex(1).isLoaded) return;
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));

        
        SceneManager.LoadScene(3, LoadSceneMode.Additive);
        SceneManager.LoadScene(4, LoadSceneMode.Additive);
        SceneManager.LoadScene(5, LoadSceneMode.Additive);
        SceneManager.LoadScene(6, LoadSceneMode.Additive);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(0);
    }
}
