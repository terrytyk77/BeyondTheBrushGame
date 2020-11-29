using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetCurrent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int y = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
