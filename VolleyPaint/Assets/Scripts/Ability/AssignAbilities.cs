using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignAbilities : MonoBehaviour
{
    public Ability superJump;
    public Ability slingShot;
    public Ability shootDown;
    public Ability slide;

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
        ChangeAbilities(superJump, slide);
    }

    public void AssignSlingShot()
    {
        ChangeAbilities(slingShot, slide);
    }

    public void AssignShootDown()
    {
        ChangeAbilities(shootDown, slide);
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
