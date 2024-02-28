using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public static Player current;

    [Header("Attack")]
    [SerializeField] private float attackRange;
    [SerializeField] private float attackRate;
    private float lastAttackTime;

    public void Awake()
    {
        current = this;
    }

}
