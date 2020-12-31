using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorTrigger : MonoBehaviour
{
    public GameObject doorToOpen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Drawable")
        {
            switch (collision.gameObject.name)
            {
                case "Stone(Clone)":
                    {
                        if(doorToOpen != null)
                        {
                            doorToOpen.GetComponent<LockedDoor>().DestroyDoor();
                        }
                        break;
                    }
                case "Crate(Clone)":
                    {
                        break;
                    }
            }
        }
    }
}
