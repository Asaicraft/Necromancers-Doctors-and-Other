using Godot;
using Godot.Collections;
using NedaoObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[GlobalClass]
[Tool]
public sealed partial class StatsProxy: BaseAttributes
{
    [Export]
    public float Health
    {
        get; set;
    }

    /// <summary>
    /// Applies the stored stats to the specified <see cref="NedaoObject"/>.
    /// </summary>
    /// <param name="nedaoObject">The target object whose stats will be updated.</param>
    public void ApplyTo(NedaoObject nedaoObject)
    {
        nedaoObject.MaxHealth.BaseValue = MaxHealth;
        nedaoObject.Health = Health;
        nedaoObject.Damage.BaseValue = Damage;
        nedaoObject.Armor.BaseValue = Armor;
        nedaoObject.Speed.BaseValue = Speed;
        nedaoObject.AttackSpeed.BaseValue = AttackSpeed;
        nedaoObject.AttackRange.BaseValue = AttackRange;
        nedaoObject.BaseAttackTime.BaseValue = BaseAttackTime;
        nedaoObject.HpRegen.BaseValue = HpRegen;
    }
}
