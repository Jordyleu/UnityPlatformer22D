using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HeroHorizontalMovementsSettings
{
    public float acceleration = 20f;
    public float deceleration = 15f;
    public float turnBackFrictions = 25f;
    public float speedmax = 5f;

    public float DashSpeed = 40f;
    public float DashDuration = 0.1f;
}
