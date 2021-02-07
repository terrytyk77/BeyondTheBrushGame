using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class sceneTeleport : MonoBehaviour
{
    static public string dungeonName;
    static public sceneTeleport instance;
    static private bool onTeleport = false;
    public GameObject background;
    public GameObject loadingScreen;
    public GameObject loadingBar;
    public GameObject loadingBarFill;

    private void Start()
    {
        if (onTeleport)
        {
            loadingScreen.SetActive(true);
            loadingBarFill.GetComponent<Image>().fillAmount = 1;
            StartCoroutine(fadeAway());
        }
        else
        {
            loadingBarFill.GetComponent<Image>().fillAmount = 0;
        }
        onTeleport = false;
    }


    private void Awake()
    {
        instance = this;
    }

    static public void start(int sceneNum)
    {

        string currentPlace = "";

        //Disable the aditional dungeon data
        if (DiscordPresence.PresenceManager.instance != null) DiscordPresence.PresenceManager.instance.presence.state = null;
        if (DiscordPresence.PresenceManager.instance != null) DiscordPresence.PresenceManager.instance.presence.smallImageKey = null;

        //Update discord presence correctly
        switch (sceneNum)
        {
            case 0:
                currentPlace = "Currently playing on: Main Menu";
                break;

            case 1:
                currentPlace = "Currently playing on: Village";
                break;

            case 2:

                currentPlace = "Currently playing on: " + dungeonName;
                if (DiscordPresence.PresenceManager.instance != null) DiscordPresence.PresenceManager.instance.presence.smallImageKey = "door";
                if (DiscordPresence.PresenceManager.instance != null) DiscordPresence.PresenceManager.instance.presence.state = "Room: [0, 0]";
                break;

            case 3:
                currentPlace = "Currently playing on: Tutorial";
                break;
        }

        //Update request
        if (DiscordPresence.PresenceManager.instance != null) DiscordPresence.PresenceManager.instance.presence.details = currentPlace;
        if (DiscordPresence.PresenceManager.instance != null) DiscordPresence.PresenceManager.UpdatePresence(null);

        //Turn the loading screen on
        instance.loadingScreen.SetActive(true);

        Time.timeScale = 0;
        onTeleport = true;
        instance.loadingBarFill.GetComponent<Image>().fillAmount = 0;
        instance.StartCoroutine("LoadAsync", sceneNum);
    }

    IEnumerator fadeAway()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        CanvasGroup backAlpha = background.GetComponent<CanvasGroup>();
        backAlpha.alpha = 1;

        while (backAlpha.alpha > 0)
        {
            backAlpha.alpha -= 0.05f;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        loadingScreen.SetActive(false);
        Time.timeScale = 1;
        yield return null;
    }

    IEnumerator LoadAsync(int sceneNum)
    {

        //Get the loading screen transparency
        CanvasGroup backAlpha = background.GetComponent<CanvasGroup>();
        backAlpha.alpha = 0;

        //Add some sort of transition
        while (backAlpha.alpha < 1)
        {
            backAlpha.alpha += 0.05f;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        //Start actually loading the new scene
        AsyncOperation opereration = SceneManager.LoadSceneAsync(sceneNum);

        //Track the new scene progress
        while (!opereration.isDone)
        {
            //Calculate current progress
            float CurrentProgress = Mathf.Clamp(opereration.progress / 0.9f, 1, 2);

            //Change the bar size
            loadingBarFill.GetComponent<Image>().fillAmount = CurrentProgress;

            //Restart the loop
            yield return null;
        }



    }

}
