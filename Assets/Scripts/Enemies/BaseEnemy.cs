using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseEnemy : BasePhysics,IHurtable
{
    public float hp;
    // Update is called once per frame
    void Awake()
    {
        characterRidgidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        UpdateStates();
    }
    protected virtual void UpdateStates()
    {
    }
    public void OnHit(int _dmg = 1)
    {
        hp -= _dmg;
        if (hp <= 0)
            Destroy(gameObject);
    }

}
