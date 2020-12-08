using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    public GameObject player;
    public Transform environmentObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        foreach (Transform envObject in environmentObjects)
        {
            if (envObject.GetComponent<BoxCollider2D>())
            {
                if (envObject.transform.position.y + envObject.GetComponent<BoxCollider2D>().size.y/2 < player.transform.position.y)
                {
                    envObject.GetComponent<SortingGroup>().sortingLayerName = "ObjectsHoveringPlayer";
                }
                else
                {
                    envObject.GetComponent<SortingGroup>().sortingLayerName = "ObjectsUnderPlayer";
                }
            }
            else
            {
                if (envObject.transform.position.y + envObject.GetComponent<CircleCollider2D>().radius / 2 < player.transform.position.y)
                {
                    envObject.GetComponent<SortingGroup>().sortingLayerName = "ObjectsHoveringPlayer";
                }
                else
                {
                    envObject.GetComponent<SortingGroup>().sortingLayerName = "ObjectsUnderPlayer";
                }
            }

        }
    }
}
