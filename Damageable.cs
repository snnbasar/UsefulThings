using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damageable : Levelable
{
    public int myId;
    public float _health;
    public float health
    {
        get => _health;
        set
        {
            _health = value;
            if (_health <= 0 && !amIDead)
            {
                KillMe();
                amIDead = true;
            }
        }
    }

    public bool amIDead;
    public bool amINotTargetable;

    [Button]
    public abstract void KillMe();
    public virtual void GiveMeDamage(float damage)
    {
        health -= damage;
    }

}
