using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("Stats")]
    public int curHp;
    public int maxHp;

    protected Character target;

    public event UnityAction onTakeDamage; //invokes event

    public void TakeDamage(int damageToTake)
    {
        curHp -= damageToTake;
        onTakeDamage?.Invoke();
        if (curHp <= 0)
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void SetTarget(Character t)
    {
        target = t;
    }
}
