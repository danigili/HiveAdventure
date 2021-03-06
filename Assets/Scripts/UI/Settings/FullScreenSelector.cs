﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenSelector : MonoBehaviour
{

    public GameObject slider;
    public GameObject[] options;

    private Vector3 sliderPos;
    private float sliderSpeed = 10f;

    void Start()
    {
        SetOption(Screen.fullScreen ? 1 : 0);
    }

    private void OnEnable()
    {
        slider.transform.position = sliderPos;
    }

    void Update()
    {
        slider.transform.position = Vector3.Lerp(slider.transform.position, sliderPos, Time.deltaTime * sliderSpeed);
    }

    public void SetOption(int index)
    {
        sliderPos = options[index].transform.position;
        for (int i = 0; i < options.Length; i++)
            options[i].GetComponentInChildren<Text>().color = (i == index) ? Color.white : Color.black;
        Screen.fullScreen = (index == 1);
    }
}
