using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bite_ZombieData : ZombieData
{
    public float HoldTime => holdtime;

    [SerializeField]
    float holdtime = 2.0f;
}
