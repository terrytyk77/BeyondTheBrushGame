using UnityEngine;

public class UIsfx : MonoBehaviour
{
    //Variables||

        public AudioClip clickSFX;
        public AudioClip typingSFX;
        public AudioClip levelUPSFX;
        public AudioClip blingSFX;

        //Character Spells
        public AudioClip slashSFX;
        public AudioClip xSlashSFX;
    //_________||


    public void playClick()
    {
        AudioSource UIelement = Camera.main.transform.Find("UISFX").GetComponent<AudioSource>();
        UIelement.clip = clickSFX;
        UIelement.time = 0.15f;
        UIelement.Play();
    }

    public void playType()
    {
        AudioSource UIelement = Camera.main.transform.Find("UISFX").GetComponent<AudioSource>();
        UIelement.clip = typingSFX;
        UIelement.time = 0f;
        UIelement.Play();
    }

    public void levelUP(){
        AudioSource UIelement = Camera.main.transform.Find("UISFX").GetComponent<AudioSource>();
        UIelement.clip = levelUPSFX;
        UIelement.time = 0f;
        UIelement.Play();
    }

    public void bling(){
        AudioSource UIelement = Camera.main.transform.Find("UISFX").GetComponent<AudioSource>();
        UIelement.clip = blingSFX;
        UIelement.time = 0.15f;
        UIelement.Play();
    }

    public void slash()
    {
        AudioSource UIelement = Camera.main.transform.Find("UISFX").GetComponent<AudioSource>();
        UIelement.clip = slashSFX;
        UIelement.time = 0f;
        UIelement.Play();
    }

    public void xSlash()
    {
        AudioSource UIelement = Camera.main.transform.Find("UISFX").GetComponent<AudioSource>();
        UIelement.clip = xSlashSFX;
        UIelement.time = 0f;
        UIelement.Play();
    }


}
