using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignAbilities : MonoBehaviour
{
    public Ability superJump;
    public Ability slingShot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignSuperJump()
    {
        ChangeAbilities(superJump);
    }

    public void AssignSlingShot()
    {
        ChangeAbilities(slingShot);
    }


    public void ChangeAbilities(params Ability[] abilities)
    {
        // removes existing ability holders first so player doesn't have more than one set of abilities at a time
        foreach (AbilityHolder abilityHolder in GetComponent<UIManager>().userPlayer.GetComponents<AbilityHolder>()) 
        {
            Destroy(abilityHolder);
        }

        // adds ability holder to player and assigns ability
        foreach (Ability ab in abilities)
        {
            GetComponent<UIManager>().userPlayer.AddComponent<AbilityHolder>().ability = ab;
        }
    }

}
