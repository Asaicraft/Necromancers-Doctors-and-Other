using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NedaoObjects.Bonuses;

/// <summary>
/// Defines the type of operation applied to a property value.
/// </summary>
public enum ModifierOperation
{
    /// <summary>
    /// Adds a fixed value to the property.
    /// If you want to decrease the value, use negative values.
    /// </summary>
    Additive,

    /// <summary>
    /// Multiplies the property value by a factor.
    /// Useful for percentage-based bonuses or scaling effects.
    /// </summary>
    Multiplicative,
}