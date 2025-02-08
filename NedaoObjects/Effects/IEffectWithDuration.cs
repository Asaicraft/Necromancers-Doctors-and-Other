using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NedaoObjects.Effects;

/// <summary>
/// Extends <see cref="IEffect"/> by adding a duration property.
/// Represents an effect that lasts for a specific period.
/// </summary>
public interface IEffectWithDuration : IEffect
{

    /// <summary>
    /// Gets the remaining duration of the effect in seconds.
    /// </summary>
    public double Duration
    {
        get;
    }
}
