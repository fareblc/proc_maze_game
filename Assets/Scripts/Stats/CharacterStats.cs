using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth { get; set; }

    public Stat damage;
    public Stat armor;


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    TakeDamage(10);
        //}
    }

    public void TakeDamage (int damage)
    {
        damage -= armor.GetValue();
        damage += Random.Range(-3, 2);
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public virtual void Die()
    {
        //Debug.Log(transform.name + " died.");
    }
}
