using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NedaoObjects.Gaints;

/// <summary>
/// Represents a base class for handling attribute gains when a hero levels up.
/// Designed to be extended for different gain mechanics.
/// </summary>
public abstract class GainModifier
{
    /// <summary>
    /// Applies attribute gains to the specified <see cref="NedaoObject"/>.
    /// </summary>
    /// <param name="nedaoObject">The object that receives attribute gains.</param>
    /// <param name="levelDifference">The number of levels gained.</param>
    public abstract void ApplyGain(NedaoObject nedaoObject, int levelDifference);
}