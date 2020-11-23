using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hourglass : MonoBehaviour
{

    private Text text;
    public int value = 0;

    private void OnEnable()
    {
        text = GetComponentInChildren<Text>();
        text.text = value.ToString();
    }

    public void SetValue(int value, bool anim = true)
    {
        this.value = value;
        if (anim)
            GetComponent<Animator>().SetTrigger("trigger");     
        else
            text.text = value.ToString();
    }

    public void Trigger()
    {
        text.text = value.ToString();
    }
}
