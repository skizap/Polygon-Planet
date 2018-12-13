﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

/*
 * CLASS HealthComplex2D : MonoBehaviour
 * -------------------------------------
 * Defines a health object capable of recieving damage inputs through an
 * aggregation of multiple damageable objects.  The script strictly
 * enforces a "one hazard, one hit" rule that prevents a single hazard
 * from dealing its damage to multiple damageable parts
 * -------------------------------------
 */ 

public class HealthComplex2D : MonoBehaviour
{
    protected int health;   // Current health
    [SerializeField]
    private int maxHealth;
    private UnityAction deathEvent; // Multi-cast delegate to functions that call when health is depleted
    private List<IDamageable2D> damageables = new List<IDamageable2D>();    
    private List<ProjectileInfo> scheduledDamage = new List<ProjectileInfo>();   // List of damage info scheduled to be taken by the health complex when the next frame is resolved

    private void Start()
    {
        health = maxHealth;
    }
    // Check each frame to see if any damage has been scheduled,
    // and if so, take the damage scheduled
    private void Update()
    {
        if(scheduledDamage.Count == 1)
        {
            TakeDamage(scheduledDamage[0].strength);
            MuteCollider(scheduledDamage[0].hitBox);
            scheduledDamage.RemoveAt(0);
        }
        else if(scheduledDamage.Count > 1)
        {
            ResolveDamage();
        }
    }
    // Deplete health. Invoke death event if health is depleted
    protected void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && deathEvent != null)
        {
            deathEvent();
        }
    }

    // Add given damage info to the schedule
    public void ScheduleDamage(ProjectileInfo info)
    {
        scheduledDamage.Add(info);
    }
    // Cleans the schedule of repeated offenses from the same hazard,
    // then takes the damage specified
    private void ResolveDamage ()
    {
        int index = 0;

        // Clean the schedule by making sure only one instance of any given collider
        // exists only once in the list
        while (index < scheduledDamage.Count)
        {
            CleanSchedule(scheduledDamage[index].hitBox);
            ++index;
        }

        // Once cleaned, take all the damage scheduled
        foreach (ProjectileInfo damage in scheduledDamage)
        {
            TakeDamage(damage.strength);
            MuteCollider(damage.hitBox);
        }

        // Clear all damage scheduled
        scheduledDamage.Clear();
    }
    // Given a collider, the method searches the list of scheduled damage and removes
    // all but the most damaging info belonging to that collider
    private void CleanSchedule (Collider2D search)
    {
        // Predicate returns true if local collider matches the one on the current damage info
        Predicate<ProjectileInfo> MatchColliders = x => x.hitBox == search;
        // Find all scheduled damage instances that have a reference to the same collider passed in as a parameter
        List<ProjectileInfo> matches = scheduledDamage.FindAll(MatchColliders);

        // Enter selection if multiple damages have the same collider
        // (i.e. if multiple damages were scheduled by the same hazardous object on multiple damageable parts)
        if (matches.Count > 1)
        {
            // Sort the list from greatest to least damage, and remove the greatest damage
            matches.Sort();
            matches.RemoveAt(0);

            // Remove all but the highest damage info from scheduled damage
            foreach(ProjectileInfo info in matches)
            {
                scheduledDamage.Remove(info);
            }
        }
    }

    // Prevent the given collider from colliding with any damageable object associated with this script
    private void MuteCollider (Collider2D muted)
    {
        foreach (IDamageable2D part in damageables)
        {
            Physics2D.IgnoreCollision(part.hitBox, muted);
        }
    }

    // Add given method to multi-cast delegate
    public void AddDeathEvent (UnityAction action)
    {
        deathEvent += action;
    }
    // Add given damageable to list
    public void RegisterDamageable (IDamageable2D register)
    {
        damageables.Add(register);
    }
}
