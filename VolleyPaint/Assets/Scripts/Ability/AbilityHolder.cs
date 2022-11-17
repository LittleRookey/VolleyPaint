using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public UnityAction<GameObject> OnAbilityStart; // event that runs on Ability Start
    public UnityAction<GameObject> OnAbilityRunning; // event that runs while abiilty is active, runs once if it is immediate ability
    public UnityAction<GameObject> OnAbilityEnd; // event that runs on Ability Start

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
        if (!ability) return;

        // takes care of cooldown of the ability
        switch (state)
        {
            case AbilityState.ready:
                if (Input.GetKeyDown(ability.key) && !isActive)
                {
                    OnAbilityStart?.Invoke(gameObject);
                    state = AbilityState.active;
                    activeTime = ability.activeTime;
                    isActive = true;

                }
                break;
            case AbilityState.active:
                if (activeTime > 0)
                {
                    activeTime -= Time.deltaTime;

                    OnAbilityRunning?.Invoke(gameObject);
                    if (Input.GetKeyUp(ability.key) && !ability.Instantaneous) // ends active time when key is unpressed and ability is not instantanous 
                    {
                        TurnSkillOff();
                    }
                }
                else
                {
                    TurnSkillOff();
                }
                break;
            case AbilityState.cooldown:
                if (cooldownTime > 0)
                {
                    cooldownTime -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.ready;
                }
                break;
        }
    }
    // Disactivate the skill and run cooldown and set active time to 0
    private void TurnSkillOff()
    {
        activeTime = 0f;
        isActive = false;
        state = AbilityState.cooldown;
        cooldownTime = ability.coolDownTime;
        OnAbilityEnd?.Invoke(gameObject);
    }
}
