using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.Netcode;

public class AbilityHolder : MonoBehaviour
{
    public Ability ability; // Ability Player will use
    public bool isLocked = false; // locks the ability, but cooldown will run
    [SerializeField]
    private Image abilityIcon; // Ability Icon for player

    float cooldownTime; // cooldown time of ability

    float activeTime; // active time of ability, how long should it run the code and start cooldown?

    bool isActive = false; // is ability active?

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
        if (isActive) { TurnSkillOff(); print("skill off"); } // so active abilities don't resume once a new round starts
    }

    void Update()
    {
        if (!ability) return;

        // takes care of cooldown of the ability
        switch (state)
        {
            case AbilityState.ready:
                if (isLocked) return;
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
                    if (abilityIcon != null)
                        abilityIcon.fillAmount = 1f - cooldownTime / ability.coolDownTime;
                }
                else
                {
                    cooldownTime = 0f;
                    state = AbilityState.ready;
                }
                break;
        }
    }

    /// <summary>
    /// Returns the Ability holder of the given ability type 
    /// </summary>
    /// <param name="target">the target GameObject AbilityHolder is attached to</param>
    /// <param name="abilityType">the type of ability to be returned</param>
    /// <returns> Returns the Ability holder that carries the given ability type </returns>
    public static AbilityHolder GetAbilityHolderOfType(GameObject target, eAbilityType abilityType)
    {
        AbilityHolder[] abilityHolders = target.GetComponents<AbilityHolder>();
        foreach (AbilityHolder ab in abilityHolders)
        {
            if (ab.ability.abilityType == abilityType)
            {
                return ab;
            }
        }
        return null;
    }

    /// <summary>
    /// Disactivate the skill and run cooldown and set active time to 0
    /// </summary>
    private void TurnSkillOff()
    {
        activeTime = 0f;
        isActive = false;
        state = AbilityState.cooldown;
        cooldownTime = ability.coolDownTime;
        OnAbilityEnd?.Invoke(gameObject);
    }
}
