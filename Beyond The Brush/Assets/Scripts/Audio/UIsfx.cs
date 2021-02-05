using UnityEngine;

public class UIsfx : MonoBehaviour
{
    //Variables||

        public AudioClip clickSFX;
        public AudioClip typingSFX;
        public AudioClip levelUPSFX;
        public AudioClip blingSFX;
        
        public AudioClip breakWoodChest;

        public AudioClip milestoneReached;

        //Character Spells
        public AudioClip slashSFX;
        public AudioClip xSlashSFX;
        public AudioClip shieldSFX;
        public AudioClip rock;
        public AudioClip box;
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
        AudioSource UIelement = Camera.main.transform.Find("SFX").GetComponent<AudioSource>();
        UIelement.clip = levelUPSFX;
        UIelement.time = 0f;
        UIelement.Play();
    }

    public void playMilestoneReached()
    {
        AudioSource UIelement = Camera.main.transform.Find("SFX").GetComponent<AudioSource>();
        UIelement.clip = milestoneReached;
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
        AudioSource spell = Camera.main.transform.Find("PlayerSFX").GetComponent<AudioSource>();
        spell.clip = slashSFX;
        spell.time = 0f;
        spell.Play();
    }

    public void xSlash()
    {
        AudioSource spell = Camera.main.transform.Find("PlayerSFX").GetComponent<AudioSource>();
        spell.clip = xSlashSFX;
        spell.time = 0f;
        spell.Play();
    }

    public void shield()
    {
        AudioSource spell = Camera.main.transform.Find("PlayerSFX").GetComponent<AudioSource>();
        spell.clip = shieldSFX;
        spell.time = 0f;
        spell.Play();
    }

    public void rockSpawn()
    {
        AudioSource spell = Camera.main.transform.Find("PlayerSFX").GetComponent<AudioSource>();
        spell.clip = rock;
        spell.time = 0f;
        spell.Play();
    }

    public void boxSpawn()
    {
        AudioSource spell = Camera.main.transform.Find("PlayerSFX").GetComponent<AudioSource>();
        spell.clip = box;
        spell.time = 0f;
        spell.Play();
    }

    public void breakChest(GameObject chestPosition)
    {
        AudioSource.PlayClipAtPoint(breakWoodChest, chestPosition.transform.position, PlayerData.sfxVolume);
    }

}
