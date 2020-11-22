using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hourglass : MonoBehaviour
{

    private Text text;
    public int value = 0;

    private void Start()
    {
        text = GetComponentInChildren<Text>();
    }

    public void SetValue(int value)
    {
        this.value = value;
        GetComponent<Animator>().SetTrigger("trigger");
    }

    public void Trigger()
    {
        text.text = value.ToString();
    }
}
