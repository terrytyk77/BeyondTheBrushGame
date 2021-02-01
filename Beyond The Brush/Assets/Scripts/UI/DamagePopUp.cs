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
        disappearTimer = 1f;
    }


    // Update is called once per frame
    void Update()
    {
        float moveYspeed = 2.5f;
        transform.position += new Vector3(0, moveYspeed, 0) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 4f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
        }
        if(textColor.a < 0)
        {
            Destroy(gameObject);
        }
    }
}
