using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    bool started;
    [SerializeField] GameObject cam;
    [SerializeField] GameObject loadScreen, loadIcon, loadText;
    [SerializeField] int uiclickSound;
    [SerializeField] AudioSource menuMusic;
    List<AsyncOperation> loadingScenes = new List<AsyncOperation>();
    public void StartGame()
    {
       // AudioManager.instance.PlaySound(uiclickSound, gameObject);
        if (started) return;
        started = true;
        loadScreen.SetActive(true);
        StartCoroutine(FadeMusic(1.5f));
        StartCoroutine(LoadSequence());
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
    }

    IEnumerator FadeMusic(float duration)
    {
        float time = 0;
        float s = menuMusic.volume;
        while (time < duration)
        {
            menuMusic.volume = Mathf.Lerp(s, 0, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator LoadSequence()
    {
        FadeCoordinator animStatus = loadScreen.GetComponent<FadeCoordinator>();
        yield return new WaitUntil(() => animStatus.animComplete);
        loadIcon.SetActive(true);
        loadText.SetActive(true);
        cam.GetComponent<AudioListener>().enabled = false;
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        yield return new WaitUntil(() => SceneManager.GetSceneByBuildIndex(1).isLoaded);
        LoadGame();
        yield return new WaitUntil(() => finishedLoading()); // Wait until all scenes are loaded
        SceneManager.UnloadSceneAsync(0);
    }

    void LoadGame()
    {
        cam.GetComponent<AudioListener>().enabled = false;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
        loadingScenes.Add(SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive));
        loadingScenes.Add(SceneManager.LoadSceneAsync(4, LoadSceneMode.Additive));
        loadingScenes.Add(SceneManager.LoadSceneAsync(5, LoadSceneMode.Additive));
        loadingScenes.Add(SceneManager.LoadSceneAsync(6, LoadSceneMode.Additive));
        loadingScenes.Add(SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive));
    }
    
    bool finishedLoading()
    {
        for (int i = 0; i < loadingScenes.Count; i++)
        {
            if(!loadingScenes[i].isDone) return false;
        }
        return true;
    }
}
