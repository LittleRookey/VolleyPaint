using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UICharger : MonoBehaviour
{
    [SerializeField] private Image bar;
    [SerializeField] private TextMeshProUGUI chargeText;

    Vector3 one;
    private void Awake()
    {
        one = Vector3.one;

    }
    private void SetY(float yVal)
    {
        one.Set(one.x, yVal, one.z);
    }
    // shows the percentage of how far it is charged 
    // percentage from 0 - 1f;
    public void SetCharger(float percentage)
    {
        SetY(percentage); // sets the y value of vector3.one
        bar.transform.localScale = one; // changes local scale only on y axis
        chargeText.SetText((percentage * 100f).ToString("F0") + "%");
    }
}
