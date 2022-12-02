using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilitySlot : MonoBehaviour
{
    [SerializeField] public Image icon;
    [SerializeField] private TextMeshProUGUI abilityKey;

    public void SetAbilitySlot(Sprite icon, KeyCode key)
    {
        this.icon.sprite = icon;
        this.abilityKey.SetText(key.ToString());
        this.icon.type = Image.Type.Filled;
        this.icon.fillMethod = Image.FillMethod.Radial360;
    }


}
