using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Login : MonoBehaviour
{
    public InputField userNameInput;
    public InputField passwordInput;
    public Button loginButton;

    // Start is called before the first frame update
    void Start()
    {
        LoginUser();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoginUser()
    {
        loginButton.onClick.AddListener(() =>
        {
            if (!string.IsNullOrEmpty(userNameInput.text) && !string.IsNullOrEmpty(passwordInput.text))
            {
                StartCoroutine(SendLoginData());
            }
        });
    }

    [System.Obsolete]
    private IEnumerator SendLoginData()
    {
        User user = new User
        {
            name = userNameInput.text,
            password = passwordInput.text
        };
        // Delete cookie before requesting a new login
        WebServices.CookieString = null;

        var request = WebServices.Post("login", JsonUtility.ToJson(user));
        yield return request.Send();

        if (request.isNetworkError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            WebServices.CookieString = request.GetResponseHeader("set-cookie");
            Debug.Log(WebServices.CookieString);
            Debug.Log(request.downloadHandler.text);
        }
    }

    private class User
    {
        public string name;
        public string password;
    }
}
