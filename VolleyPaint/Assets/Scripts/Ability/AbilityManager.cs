using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    // first load all kinds of ability
    private Dictionary<string, Ability> abilityLibrary;

    private const string abilitiesPath = "Abilities";

    PlayerController[] players;

    void Start()
    {
        LoadAbilities();
        
    }

    public void UpdatePlayerList()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
        
        for(int i = 0; i < gos.Length; i++)
        {
            players[i] = gos[i].GetComponent<PlayerController>();
        }
    }
    // loads all the existing abilities to dictionary
    void LoadAbilities()
    {
        Ability[] tempAbility = Resources.LoadAll<Ability>(abilitiesPath);
        foreach (Ability ab in tempAbility)
        {
            abilityLibrary.Add(ab.name, ab);
        }
    }


}
