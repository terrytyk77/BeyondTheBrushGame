using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetworkClient : SocketIOComponent
{
    // Start is called before the first frame update
    public override void Start()
    {
        // Call initial Start() function of socket io
        base.Start();
        // Override -- code executed after base start is loaded
        setupEvents();
    }

    // Update is called once per frame
    public override void Update()
    {
        // Call initial Update() function of socket io
        base.Update();
        // Override -- code executed after base update is loaded
    
    }

    private void setupEvents()
    {
        On("open", (Event) =>
        {
            Debug.Log("Connection made to the server!");
        });
    }
}