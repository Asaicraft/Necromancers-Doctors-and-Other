using Godot;
using NedaoObjects;
using NedaoObjects.Gains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[GlobalClass]
public sealed partial class GainProxy: BaseAttributes
{
    public void ApplyTo(NedaoObject nedaoObject)
    {
        if(nedaoObject.GainModifier is SimpleGainModifier gainModifier)
        {
            gainModifier.MaxHealth.BaseValue = MaxHealth;
            gainModifier.Damage.BaseValue = Damage;
            gainModifier.Armor.BaseValue = Armor;
            gainModifier.Speed.BaseValue = Speed;
            gainModifier.AttackSpeed.BaseValue = AttackSpeed;
            gainModifier.AttackRange.BaseValue = AttackRange;
            gainModifier.BaseAttackTime.BaseValue = BaseAttackTime;
            gainModifier.HpRegen.BaseValue = HpRegen;
        }
    }
}
