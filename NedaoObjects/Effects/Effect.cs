using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NedaoObjects.Effects;

/// <summary>
/// Represents an abstract base class for all effects that can be applied to a <see cref="NedaoObject"/>.
/// Implements <see cref="IEffect"/> and is designed to be extended to define specific effect behaviors.
/// </summary>
public abstract class Effect : IEffect
{
    /// <inheritdoc />
    public event Action<Effect>? OnEffectEnd;

    /// <summary>
    /// Raises the <see cref="OnEffectEnd"/> event to signal that the effect has ended.
    /// Should be called by derived classes when the effect completes.
    /// </summary>
    protected void RaiseEffectEnd()
    {
        OnEffectEnd?.Invoke(this);
    }

    /// <inheritdoc />
    public abstract void ApplyEffect(NedaoObject target);

    /// <inheritdoc />
    public abstract void UpdateEffect(NedaoObject target, double delta);

    /// <inheritdoc />
    public abstract void RemoveEffect(NedaoObject target);
}
