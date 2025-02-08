using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NedaoObjects.Bonuses;

/// <summary>
/// Represents a base class for property modifiers.
/// Designed to be extended to define custom modification logic.
/// </summary>
/// <typeparam name="T">The numeric type of the property, constrained to <see cref="IBinaryNumber{T}"/>.</typeparam>
public abstract class PropertyModifier<T> where T : struct, IBinaryNumber<T>
{
    /// <summary>
    /// Modifies the given property.
    /// Must be implemented in a derived class.
    /// </summary>
    /// <param name="property">The property being modified.</param>
    /// <returns>The modified value.</returns>
    public abstract T Modify(NedaoProperty<T> property);
}