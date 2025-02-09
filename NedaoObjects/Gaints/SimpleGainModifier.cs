using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NedaoObjects.Gaints;

/// <summary>
/// A simple gain modifier that increases attributes linearly per level.
/// </summary>
public sealed class SimpleGainModifier : GainModifier
{
    public readonly NedaoProperty<float> MaxHealth = [];
    public readonly NedaoProperty<float> Damage = [];
    public readonly NedaoProperty<float> Armor = [];
    public readonly NedaoProperty<float> Speed = [];
    public readonly NedaoProperty<float> AttackSpeed = [];
    public readonly NedaoProperty<float> AttackRange = [];
    public readonly NedaoProperty<float> BaseAttackTime = [];

    /// <inheritdoc />
    public override void ApplyGain(NedaoObject nedaoObject, int levelDifference)
    {
        nedaoObject.MaxHealth.BaseValue += MaxHealth * levelDifference;
        nedaoObject.Damage.BaseValue += Damage * levelDifference;
        nedaoObject.Armor.BaseValue += Armor * levelDifference;
        nedaoObject.Speed.BaseValue += Speed * levelDifference;
        nedaoObject.AttackSpeed.BaseValue += AttackSpeed * levelDifference;
        nedaoObject.AttackRange.BaseValue += AttackRange * levelDifference;
        nedaoObject.BaseAttackTime.BaseValue += BaseAttackTime * levelDifference;
    }
}