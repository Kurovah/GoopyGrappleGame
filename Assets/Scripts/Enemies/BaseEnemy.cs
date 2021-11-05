using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseEnemy : BasePhysics,IHurtable
{
    public float hp;

    public enum EnemyStates
    {
        normal,
        alerted,
        reacting,
        stunned
    }
    protected Action NormalStateAction, AlertStateAction, ReactStateAction, StunnedStateAction;
    EnemyStates currentState = EnemyStates.normal;
    // Update is called once per frame
    void Awake()
    {
        InitStateFunctions();
        characterRidgidBody = GetComponent<Rigidbody2D>();
    }

    protected virtual void InitStateFunctions() { }
    void Update()
    {
        switch (currentState)
        {
            case EnemyStates.normal:
                NormalStateAction();
                break;
            case EnemyStates.alerted:
                AlertStateAction();
                break;
            case EnemyStates.reacting:
                ReactStateAction();
                break;
            case EnemyStates.stunned:
                StunnedStateAction();
                break;
        }
    }
    #region state functions
    
    #endregion
    public void OnHit(int _dmg = 1)
    {
        hp -= _dmg;
        if (hp <= 0)
            Destroy(gameObject);
    }

}
