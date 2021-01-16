using UnityEngine;

public class UIsfx : MonoBehaviour
{
    //Variables||

        public AudioClip clickSFX;
    //_________||


    public void playClick()
    {
        AudioSource UIelement = Camera.main.transform.Find("UISFX").GetComponent<AudioSource>();
        UIelement.clip = clickSFX;
        UIelement.time = 0.15f;
        UIelement.Play();
    }
}
