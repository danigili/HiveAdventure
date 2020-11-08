using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour
{
    public string key;

    private Language language;
    private Text textComponent;
    private TextMesh textMesh;

    void Start()
    {
        gameObject.TryGetComponent<Text>(out textComponent);
        gameObject.TryGetComponent<TextMesh>(out textMesh);
        SetText();
    }

    void Update()
    {
        if (language != Localization.GetCurrentLanguage())
            SetText();
    }

    private void SetText()
    {
        language = Localization.GetCurrentLanguage();
        if (textComponent != null)
            textComponent.text = Localization.Translate(key);
        if (textMesh != null)
            textMesh.text = Localization.Translate(key);
    }
}
