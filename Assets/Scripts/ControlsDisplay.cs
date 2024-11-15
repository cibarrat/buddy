using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsDisplay : MonoBehaviour
{
    public GameObject[] background;
    private int index = 0;

    public GameObject Controls;

    public void ToggleControl()
    {
        bool isActive = Controls.activeSelf;
        if (!isActive)
        {
            index = 0;
            UpdateDisplay();
        }
        Controls.SetActive(!isActive);
    }

    public void CloseControl()
    {
        Controls.SetActive(false);
    }

    void Start()
    {
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        for (int i = 0; i < background.Length; i++)
        {
            background[i].SetActive(i == index);
        }
    }

    public void Next()
    {
        index = (index + 1) % background.Length;
        UpdateDisplay();
    }

    public void Previous()
    {
        index = (index - 1 + background.Length) % background.Length;
        UpdateDisplay();
    }
}
