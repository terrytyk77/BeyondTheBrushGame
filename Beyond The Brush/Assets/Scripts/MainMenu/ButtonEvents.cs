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
        //Get the loading screen
        GameObject loadingScreen = GameObject.FindGameObjectWithTag("LoadingScreen");

        if (loadingScreen != null)
            //Start Loading the Scene
            StartCoroutine(LoadAsync(loadingScreen));
        else
            Debug.Log("Error! Scene not found");
       
        
    }

    public void CancelOffline()
    {
        //This closes the window
        gameObject.SetActive(false);
    }

    IEnumerator LoadAsync(GameObject loadingScreen)
    {

        //Getting the loading bar
        GameObject background = loadingScreen.GetComponent<Transform>().GetChild(0).gameObject;
        GameObject loadingBar = loadingScreen.GetComponent<Transform>().GetChild(0).GetChild(0).gameObject;

        //Add some sort of transition


        //Turn the loading screen on
        background.SetActive(true);

        //Just wait
        yield return new WaitForSeconds(2);

        //Start actually loading the new scene
        AsyncOperation opereration = SceneManager.LoadSceneAsync(MainGame);

        //Track the new scene progress
        while (!opereration.isDone)
        {
            //Calculate current progress
            float CurrentProgress = Mathf.Clamp(opereration.progress / 0.9f, 1, 2);

            //Get the bar fill object
            GameObject barFill = loadingScreen.GetComponent<Transform>().GetChild(0).GetChild(0).GetChild(0).gameObject;

            //Change the bar size
            barFill.GetComponent<RectTransform>().localScale = new Vector3(CurrentProgress, 1, 1);

            //Restart the loop
            yield return null;
        }

    }

}

