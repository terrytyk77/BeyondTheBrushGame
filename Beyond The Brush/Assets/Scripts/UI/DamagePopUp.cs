using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{
    public static DamagePopUp Create(GameObject DamagePopUpPrefab, Vector3 position, int damageAmount)
    {
        GameObject damagePopUpObject = Instantiate(DamagePopUpPrefab, position, Quaternion.identity);
        DamagePopUp damagePopUp = damagePopUpObject.transform.GetComponent<DamagePopUp>();
        damagePopUp.Setup(damageAmount);
        return damagePopUp;
    }

    private TextMeshPro textMesh;
    private float disappearTimerMax = 0.5f;
    private float disappearTimer;
    private Color textColor;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    // Start is called before the first frame update
    public void Setup(int damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
        textColor = textMesh.color;
        disappearTimer = disappearTimerMax;
    }


    // Update is called once per frame
    void Update()
    {
        float moveYspeed = 4f;
        transform.position += new Vector3(0, moveYspeed, 0) * Time.deltaTime;

        //Animation
        if(disappearTimer > disappearTimerMax / 2)
        {
            transform.localScale += Vector3.one/2 * Time.deltaTime;
        }
        else
        {
            transform.localScale -= Vector3.one/2 * Time.deltaTime;
        }


        //Turn Transparent and Destroy Text
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 5f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
        }
        if(textColor.a < 0)
        {
            Destroy(gameObject);
        }
    }
}
