using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NedaoObjects.Bonuses;

/// <summary>
/// Represents a specific value modifier applied to a property.
/// </summary>
/// <typeparam name="T">The numeric type of the property, constrained to <see cref="IBinaryNumber{T}"/>.</typeparam>
public sealed class ValueModifier<T> : PropertyModifier<T> where T : struct, IBinaryNumber<T>
{
    /// <summary>
    /// The value applied by this modifier.
    /// </summary>
    /// <remarks>
    /// If you change this value before applying the bonus, 
    /// you should call <see cref="NedaoProperty{T}.InvalidState"/>.
    /// </remarks>
    public T Value
    {
        get;
        set;
    }

    /// <summary>
    /// The operation used to modify the property value.
    /// </summary>
    public ModifierOperation Operation
    {
        get;
        set;
    }

    /// <summary>
    /// Applies the modifier to the given property.
    /// </summary>
    /// <param name="property">The property being modified.</param>
    /// <returns>The new total modified value.</returns>
    public override T Modify(NedaoProperty<T> property)
    {
        return Operation switch
        {
            ModifierOperation.Additive => property.TotalBonus + Value,
            ModifierOperation.Multiplicative => property.TotalBonus * Value,
            _ => property.TotalBonus
        };
    }
}