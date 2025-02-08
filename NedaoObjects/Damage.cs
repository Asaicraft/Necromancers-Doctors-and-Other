using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NedaoObjects;

/// <summary>
/// Represents a damage instance inflicted by a source on a target.
/// This struct is immutable.
/// </summary>
/// <param name="source">The object that inflicted the damage.</param>
/// <param name="value">The amount of damage dealt.</param>
/// <param name="damageType">The type of damage dealt.</param>
public readonly struct Damage(NedaoObject source, float value, DamageType damageType)
{
    /// <summary>
    /// The object that inflicted the damage.
    /// </summary>
    public readonly NedaoObject Source = source;

    /// <summary>
    /// The amount of damage dealt.
    /// </summary>
    public readonly float Value = value;

    /// <summary>
    /// The type of damage dealt.
    /// </summary>
    public readonly DamageType DamageType = damageType;
}
