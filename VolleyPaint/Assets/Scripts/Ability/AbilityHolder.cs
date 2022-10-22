using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;

public class AbilityHolder : MonoBehaviour
{
    public Ability ability;
    float cooldownTime;
    float activeTime;
    bool isActive = false;
    enum AbilityState
    {
        ready, // when ability gets ready, no cooldown
        active, // when ability is playing
        cooldown // when ability is in used, waiting to be active
    }
    AbilityState state = AbilityState.ready;

    public UnityAction OnAbilityStart; // event that runs on Ability Start
    public UnityAction OnAbilityRunning; // event that runs while abiilty is active, runs once if it is immediate ability
    public UnityAction OnAbilityEnd; // event that runs on Ability Start
    //public KeyCode key;

    public void OnEnable()
    {
        OnAbilityStart += ability.OnAbilityStart;
        OnAbilityRunning += ability.OnAbilityRunning;
        OnAbilityEnd += ability.OnAbilityEnd;
    }

    private void OnDisable()
    {
        OnAbilityStart -= ability.OnAbilityStart;
        OnAbilityRunning -= ability.OnAbilityRunning;
        OnAbilityEnd -= ability.OnAbilityEnd;
    }

    void Update()
    {
        // takes care of cooldown of the ability
        switch(state)
        {
            case AbilityState.ready:
                if (Input.GetKeyDown(ability.key) && !isActive)
                {
                    OnAbilityStart?.Invoke();
                    ability.UseAbility(gameObject);
                    state = AbilityState.active;
                    activeTime = ability.activeTime;
                    isActive = true;

                } 
                break;
            case AbilityState.active:
                if (activeTime > 0)
                {
                    activeTime -= Time.deltaTime;
                    OnAbilityRunning?.Invoke();
                    if (Input.GetKeyUp(ability.key)) // when key is unpressed while active
                    {
                        Debug.Log("Key up");
                        activeTime = 0f;
                        isActive = false;
                        ability.BeginCooldown(gameObject);
                        state = AbilityState.cooldown;
                        cooldownTime = ability.coolDownTime;
                        OnAbilityEnd?.Invoke();
                    }
                }
                else
                {
                    Debug.Log("ActiveTime <= 0");
                    activeTime = 0f;
                    isActive = false;
                    ability.BeginCooldown(gameObject);
                    state = AbilityState.cooldown;
                    cooldownTime = ability.coolDownTime;
                    OnAbilityEnd?.Invoke();
                }
                break;
            case AbilityState.cooldown:
                if (cooldownTime > 0)
                {
                    cooldownTime -= Time.deltaTime;
                } else
                {
                    state = AbilityState.ready;
                }
                break;
        }
    }
}
