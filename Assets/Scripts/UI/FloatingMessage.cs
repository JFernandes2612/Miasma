using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingMessage : MonoBehaviour
{

    private bool textPulse = true;
    private string textMessage = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent nec eros vitae dui rutrum...";

    private bool active = false;

    private bool waiting = false;
    public TMPro.TMP_FontAsset[] fonts;

    private TextMeshProUGUI textObject;
    private Material textMaterialInstance;

    private float alpha = 0.0f;

    private bool fadingIn = false;

    // Start is called before the first frame update
    void Start()
    {
        textObject = GetComponent<TextMeshProUGUI>();
        textMaterialInstance = textObject.fontMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (active) {
            textObject.text = textMessage;
            if (textPulse)
            {
                if (!waiting)
                {
                    StartCoroutine(SetColorAfterDelay());
                }
            }
            else
            {
                ResetToDefault();
            }

            SetAlphaColor();
        }
        else {
            textObject.text = "";
        }
    }

    private void SetAlphaColor() {
        textObject.color = new Vector4(textObject.color.r, textObject.color.g, textObject.color.b, alpha);
    }


    private Color GenerateRandomColor()
    {
        return new Vector4(
            Random.Range(0f, 1f),    //red
            Random.Range(0f, 1f),    //green
            Random.Range(0f, 1f),    //blue
            1f);
    }

    private IEnumerator SetColorAfterDelay()
    {
        waiting = true;
        textObject.color = GenerateRandomColor();
        yield return new WaitForSeconds(0.5f + Random.Range(-0.4f, 0.4f));

        System.Random random = new System.Random();
        int randomFont = random.Next(0, fonts.Length);
        textObject.font = fonts[randomFont];

        textObject.gameObject.SetActive(false);
        textObject.fontSharedMaterial.SetFloat("_OutlineWidth", 0.05f);
        textObject.fontSharedMaterial.SetColor("_OutlineColor", Color.white);
        textObject.gameObject.SetActive(true);

        waiting = false;
    }

    public void FadeIn() {
        StartCoroutine(FadeInC());
    }

    public void FadeOut() {
        StartCoroutine(FadeOutC());
    }

    private IEnumerator FadeInC()
    {
        fadingIn = true;
        while (alpha < 1) {
            alpha += 0.04f;
            yield return new WaitForSeconds(0.01f);
        }
        fadingIn = false;
    }

    private IEnumerator FadeOutC()
    {
        bool i = false;
        while (alpha > 0) {
            while (fadingIn) {
                yield return new WaitForSeconds(0.01f);
                i = true;
                continue;
            }
            alpha -= 0.04f;
            yield return new WaitForSeconds(0.01f);
        }
        if (!i)
            SetActive(false);
    }

    public void SetPulse(bool value)
    {
        textPulse = value;
    }

    public void SetActive(bool value)
    {
        active = value;
    }

    public void SetText(string text)
    {
        textMessage = text;
    }

    public void Reset() {
        ResetToDefault();
        fadingIn = false;
        active = false;
        alpha = 0.0f;
        textPulse = true;
    }

    public void ResetToDefault()
    {
        textObject.font = fonts[0];
        textObject.color = new Color(255, 255, 255);
    }

}
