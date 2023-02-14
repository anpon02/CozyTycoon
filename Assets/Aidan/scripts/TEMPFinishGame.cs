using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TEMPFinishGame : MonoBehaviour
{
    public void END()
    {
        SceneManager.LoadScene(7, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(2);
        SceneManager.UnloadSceneAsync(3);
        SceneManager.UnloadSceneAsync(4);
    }
}
