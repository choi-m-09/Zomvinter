using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ZombieData : ScriptableObject
{
    public int Id => _id;
    public string Name => _name;
    public int MaxHp => _maxhp;
    public int Ap => _ap;
    public float AttackRange => _attackrange;
    public float AttackSpeed => _attackspeed;
    public float MoveSpeed => _movespeed;

    [SerializeField] int _id;
    [SerializeField] string _name;
    [SerializeField] int _maxhp;
    [SerializeField] int _ap;
    [SerializeField] float _attackrange;
    [SerializeField] float _attackspeed;
    [SerializeField] float _movespeed;
}
