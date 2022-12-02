using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AbilityUIAssigner : MonoBehaviour
{
    [SerializeField] private GameObject horizontalLayout; // for abilitySlots
    [SerializeField] private AbilitySlot abilitySlot; // abilitySlot prefab

    public Image SetupSlots(Ability ability)
    {
        var slot = Instantiate(abilitySlot, horizontalLayout.transform);
        slot.SetAbilitySlot(ability.icon, ability.key);
        return slot.icon;
    }

    public void ClearSlots()
    {
        for (int i= 0; i < horizontalLayout.transform.childCount; i++)
        {
            Destroy(horizontalLayout.transform.GetChild(i).gameObject);
        }
    }
}
