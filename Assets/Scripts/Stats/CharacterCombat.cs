using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour
{
    public float attackSpeed = 1f;
    private float attackCooldown = 0f;
    const float combatCooldown = 5f;
    float lastAttackTime;

    public float attackDelay = .6f;

    public bool InCombat { get; private set; }
    public event System.Action OnAttack;


    CharacterStats myStats;
    CharacterStats opponentStats;

    void Start()
    {
        myStats = GetComponent<CharacterStats>();
    }

    void Update()
    {
        attackCooldown -= Time.deltaTime;   

        if(Time.time - lastAttackTime > combatCooldown)
        {
            InCombat = false;
        }
    }

    public void Attack(CharacterStats targetStats)
    {
        if (attackCooldown <= 0f)
        {
            if (Random.value < 1.0f)
            {
                //opponentStats = targetStats;
                //if (OnAttack != null)
                //    OnAttack();
                StartCoroutine(DoDamage(targetStats, attackDelay));
                //targetStats.TakeDamage(myStats.damage.GetValue());
                attackCooldown = 1f / attackSpeed;
            }
            else
                Debug.Log("Attack Missed");

            InCombat = true;
            lastAttackTime = Time.time;
        }
    }

    IEnumerator DoDamage(CharacterStats stats, float delay)
    {
        yield return new WaitForSeconds(delay);

        stats.TakeDamage(myStats.damage.GetValue());
        if (stats.currentHealth <= 0)
        {
            InCombat = false;
        }
    }

    //public void AttackHit_AnimationEvent()
    //{
    //    opponentStats.TakeDamage(myStats.damage.GetValue());
    //    if (opponentStats.currentHealth <= 0)
    //    {
    //        InCombat = false;
    //    }
    //}
}
