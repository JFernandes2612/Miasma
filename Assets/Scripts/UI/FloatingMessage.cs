using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingMessage : MonoBehaviour
{

    private bool textPulse = true;
    private string textMessage = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent nec eros vitae dui rutrum...";
    private float time = 0;

    private bool waiting = false;
    public TMPro.TMP_FontAsset[] fonts;

    private TextMeshProUGUI textObject;
    private Material textMaterialInstance;

    // Start is called before the first frame update
    void Start()
    {
        textObject =  GetComponent<TextMeshProUGUI>();
        textMaterialInstance = textObject.fontMaterial;
    }


    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        textObject.text = textMessage;
        if (textPulse){
            if (!waiting){
            StartCoroutine(SetColorAfterDelay());}
        }
    }


    private Color GenerateRandomColor(){
        
        

        return new Vector4(
            Random.Range(0f,1f),    //red
            Random.Range(0f,1f),    //green
            Random.Range(0f,1f),    //blue
            1f);
    }

    private IEnumerator SetColorAfterDelay()
    {
        waiting = true;
        yield return new WaitForSeconds( 1f + 0.5f * (float)System.Math.Sin(time)); 
        textObject.color = GenerateRandomColor();

        System.Random random = new System.Random();
        int randomFont = random.Next(0, fonts.Length);
        textObject.font = fonts[randomFont];

        textObject.gameObject.SetActive(false);
        textObject.fontSharedMaterial.SetFloat("_OutlineWidth", 0.05f);
        textObject.fontSharedMaterial.SetColor("_OutlineColor", Color.white);
        textObject.gameObject.SetActive(true);
        
        waiting = false;
    }

    public void activatePulse(){
        textPulse = true;
    }

    public void deactivatePulse(){
        textPulse = false;
    }

    public void changeText(string text){
        textMessage = text;
    }

    public void resetToDefault(){
        textObject.font = fonts[0];
        textObject.color = new Color(255,255,255);
    }

}
