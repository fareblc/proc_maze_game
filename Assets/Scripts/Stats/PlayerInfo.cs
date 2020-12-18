using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : CharacterStats
{
    [SerializeField] Text playerHP;


    public PlayerHealthBar healthBar;

    private void Start()
    {
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        if (this.currentHealth > 0)
        {
            playerHP.text = "Player HP: " + this.currentHealth.ToString();
            healthBar.SetHealth(currentHealth);
        }
        else
        {
            playerHP.text = "Player Died";
            healthBar.SetHealth(0);
        }
    }

    public override void Die()
    {
        base.Die();
        PlayerManager.instance.KillPlayer();
    }

}
