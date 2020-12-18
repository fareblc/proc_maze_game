using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : CharacterStats
{

    [SerializeField] private Text enemyHP;

    public PlayerHealthBar healthBar;

    private void Start()
    {
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        if (this.currentHealth > 0)
        {
            enemyHP.text = "Enemy HP: " + this.currentHealth.ToString();
            healthBar.SetHealth(currentHealth);
        }
        else
        {
            enemyHP.text = "Enemy Died";
            healthBar.SetHealth(0);
        }
    }

    public override void Die()
    {
        base.Die();
        gameObject.SetActive(false);
        this.currentHealth = 100;
        this.GetComponent<Enemy>().hasInteracted = false;
        this.GetComponent<Enemy>().isFocus = false;
    }

}
