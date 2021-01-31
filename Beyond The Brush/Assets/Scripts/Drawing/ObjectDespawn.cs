using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDespawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DespawnObject", 8f);
    }

    private void DespawnObject()
    {
        Destroy(gameObject);
    }
}
