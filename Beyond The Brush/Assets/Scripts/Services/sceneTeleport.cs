using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class sceneTeleport : MonoBehaviour
{
    static public string dungeonName;
    static public sceneTeleport instance;
    public GameObject background;
    public GameObject loadingScreen;
    public GameObject loadingBar;

    private void Awake()
    {
        instance = this;
    }

    static public void start(int sceneNum)
    {
        instance.StartCoroutine("LoadAsync", sceneNum);
    }

    IEnumerator LoadAsync(int sceneNum)
    {
        Debug.Log(loadingScreen);

        //Get the loading screen transparency
        CanvasGroup backAlpha = background.GetComponent<CanvasGroup>();
        backAlpha.alpha = 0;

        //Turn the loading screen on
        loadingScreen.SetActive(true);

        //Add some sort of transition
        while (backAlpha.alpha < 1)
        {
            backAlpha.alpha += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }



        loadingBar.SetActive(true);

        //Just wait
        yield return new WaitForSeconds(.5f);


        //Start actually loading the new scene
        AsyncOperation opereration = SceneManager.LoadSceneAsync(sceneNum);

        //Track the new scene progress
        while (!opereration.isDone)
        {
            //Calculate current progress
            float CurrentProgress = Mathf.Clamp(opereration.progress / 0.9f, 1, 2);

            //Get the bar fill object
            GameObject barFill = loadingBar.GetComponent<Transform>().GetChild(0).gameObject;

            //Change the bar size
            barFill.GetComponent<Image>().fillAmount = CurrentProgress;

            //Restart the loop
            yield return null;
        }

    }

}
