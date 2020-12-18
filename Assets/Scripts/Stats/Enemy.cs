using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterStats))]
public class Enemy : Interactable
{

    PlayerManager playerManager;
    CharacterStats myStats;

    void Start()
    {
        playerManager = PlayerManager.instance;
        myStats = GetComponent<CharacterStats>();
        if (MainMenu.diff == 1) {
            this.GetComponent<CharacterStats>().maxHealth = this.GetComponent<CharacterStats>().maxHealth;
        }
        else {
            this.GetComponent<CharacterStats>().maxHealth = this.GetComponent<CharacterStats>().maxHealth + 20;
            this.GetComponent<CharacterStats>().currentHealth = this.GetComponent<CharacterStats>().maxHealth;
        }

    }

    public override void Interact()
    {
        CharacterCombat playerCombat = playerManager.player.GetComponent<CharacterCombat>();
        if(playerCombat != null)
        {
            //base.Interact();
            this.hasInteracted = false;
            playerCombat.Attack(myStats);
        }
    }

}
