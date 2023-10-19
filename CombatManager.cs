using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatManager : Damageable
{


    public List<Damageable> damageables = new List<Damageable>();

    public float fireTime;
    float startFireTime;

    float fireTimer;

    public bool isFiring;

    public EventTrigger[] eventTriggers;

    [Range(0, 1)]
    public float hitRate = 1f;

    public bool canFire;


    public virtual void Start()
    {
        // eventTriggers[0].OnTriggerEnterAgent += AddMeToList;
        // eventTriggers[0].OnTriggerExitAgent += RemoveMeFromList;
        // eventTriggers[0].OnTriggerStayAgent += CheckIfTargetIsDead;

        startFireTime = fireTime;

        SetCanFire(true);

        GameManager.Instance.AddMeToCombatManagers(this);
    }

    public virtual void Update()
    {
        if (GameManager.Instance._gameState != GameManager.GameState.Started)
            return;
        if (!canFire)
            return;
        FireChecker();
    }

    public void AddMeToList(Damageable damageable)
    {
        if (myId == damageable.myId)
            return;
        if (damageable.amINotTargetable)
            return;
        damageables.Add(damageable);
        fireTimer = fireTime;
    }
    public void RemoveMeFromList(Damageable damageable)
    {
        if (myId == damageable.myId)
            return;
        damageables.Remove(damageable);
    }

    private void FireChecker()
    {
        if (isFiring)
            return;
        if (damageables.Count <= 0)
            return;
        if (amIDead)
            return;

        fireTimer += Time.deltaTime;
        if (fireTimer >= fireTime)
        {
            fireTimer = 0;
            FireWeapon();
        }
    }


    public abstract void FireWeapon();



    public void CheckIfTargetIsDead(Damageable damageable)
    {
        if (myId == damageable.myId)
            return;
        if (!damageable.amIDead)
            return;
        if (!damageables.Contains(damageable))
            return;
        damageables.Remove(damageable);
    }

    public void UpdateFireTime(int level)
    {
        float minFireTime = 0.1f;
        float newFireTime = startFireTime - (0.1f * level);
        fireTime = newFireTime < minFireTime ? minFireTime : newFireTime;
    }
    public void IncreaseFireTime(float value)
    {
        float minFireTime = 0.1f;
        float maxFireTime = 0.3f;
        float newFireTime = fireTime;
        newFireTime += value;
        fireTime = newFireTime < minFireTime ? minFireTime : newFireTime;
        // PlayerController player = (PlayerController)this;
        // if (player)
        // {
            // player.anims[1].speed = ((maxFireTime + 0.1f) - fireTime) * 10;
        // }
    }

    public void SetCanFire(bool sts)
    {
        canFire = sts;
    }

}
