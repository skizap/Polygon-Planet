﻿using UnityEngine.Events;

public interface IDeathHandler
{
    bool isDead { get; }
    void Die();
    void AddDeathEvent(UnityAction method);
    void RemoveDeathEvent(UnityAction method);
}