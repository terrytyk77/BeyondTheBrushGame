using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvents : MonoBehaviour
{

    private int MainGame = 0;

    public void OpenNotification()
    {
            //This is gonna show up the confirmation box
            gameObject.SetActive(!gameObject.active);
    }

    public void StartOffline()
    {
        //Load the scene
        StartCoroutine(LoadAsync());
    }

    public void CancelOffline()
    {
        //This closes the window
        gameObject.SetActive(false);
    }

    IEnumerator LoadAsync()
    {
        AsyncOperation opereration = SceneManager.LoadSceneAsync(MainGame);

        while (!opereration.isDone)
        {
            Debug.Log(opereration.progress);

            yield return null;
        }

    }

}

