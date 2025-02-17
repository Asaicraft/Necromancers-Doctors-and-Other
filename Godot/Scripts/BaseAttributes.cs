﻿using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract partial class BaseAttributes : Resource
{
    [Export]
    public float MaxHealth
    {
        get; set;
    }

    [Export]
    public float Damage
    {
        get; set;
    }

    [Export]
    public float Armor
    {
        get; set;
    }

    [Export]
    public float Speed
    {
        get; set;
    }

    [Export]
    public float AttackSpeed
    {
        get; set;
    }

    [Export]
    public float AttackRange
    {
        get; set;
    }

    [Export]
    public float BaseAttackTime
    {
        get; set;
    }

    [Export]
    public float HpRegen
    {
        get; set;
    }
}
