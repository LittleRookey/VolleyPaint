using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum eAbilityType
{
    Dash
}

[CreateAssetMenu(menuName = "Litkey/Ability/base")]
public class Ability : ScriptableObject
{
    [Header("Debug")]
    public bool showLog;

    [Header("Main Ability Settings")]
    public Sprite icon;
    public new string name; // ability name
    public float coolDownTime; // cooldown time for ability
    public float activeTime; // time to start cooldown after ability is activated, normally runs until the player's on key up event
    [TextArea]
    public string description; // description of the ability
    public eAbilityType abilityType; // type of ability 
    public KeyCode key; // ability use key 
    public bool Instantaneous; // will allow to run OnAbilityRunning even when player on key up

    [SerializeField]
    protected bool cantMoveWhileUsingAbility; // allows player to move while using ability
    protected bool isUsingAbility; // when player is holding on a key(charge?), 

    public Ability Clone()
    {
        Ability ab = new Ability();
        ab.name = name;
        ab.coolDownTime = coolDownTime;
        ab.description = description;
        ab.abilityType = abilityType;
        ab.key = key;
        return ab;
    }

    /// <summary>
    /// callback event ran when ability starts
    /// </summary>
    /// /// <param name="parent">the gameObject Ability Holder is attached to</param>
    public virtual void OnAbilityStart(GameObject parent)
    {
        if (showLog)
        {
            Debug.Log(name + " Ability Started");
        }
        isUsingAbility = true;
        if (cantMoveWhileUsingAbility)
        {
            PlayerMovement pm = parent.GetComponent<PlayerMovement>();
            pm.canMove = false;
        }
    }

    /// <summary>
    /// callback event ran when ability ended
    /// </summary>
    /// /// <param name="parent">the gameObject Ability Holder is attached to</param>
    public virtual void OnAbilityEnd(GameObject parent)
    {
        if (showLog)
        {
            Debug.Log(name + " Ability Ended");
        }
        if (cantMoveWhileUsingAbility)
        {
            PlayerMovement pm = parent.GetComponent<PlayerMovement>();
            pm.canMove = true;
        }
    }

    /// <summary>
    /// callback event ran when player is holding a key
    /// </summary>
    /// /// <param name="parent">the gameObject Ability Holder is attached to</param>
    public virtual void OnAbilityRunning(GameObject parent)
    {
        if (showLog)
        {
            //Debug.Log(name + " Ability Running");
        }
    }

    protected bool IsOnCooldown()
    {
        return coolDownTime > 0f;
    }
}
