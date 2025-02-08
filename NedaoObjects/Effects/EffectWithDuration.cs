using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NedaoObjects.Effects;

/// <summary>
/// Represents an effect with a limited duration.
/// Implements <see cref="IEffectWithDuration"/> and extends <see cref="Effect"/>.
/// </summary>
/// <param name="duration">The initial duration of the effect in seconds.</param>
public abstract class EffectWithDuration(double duration) : Effect, IEffectWithDuration
{
    /// <summary>
    /// The remaining duration of the effect in seconds.
    /// </summary>
    private double _duration = duration;

    /// <inheritdoc />
    public double Duration => _duration;

    /// <summary>
    /// Updates the effect, reducing its duration over time.
    /// When the duration reaches zero, the effect is removed and the <see cref="Effect.OnEffectEnd"/> event is raised.
    /// Calls <see cref="UpdateEffectCore"/> to allow further customization.
    /// </summary>
    /// <param name="target">The object affected by the effect.</param>
    /// <param name="delta">The time step for the update.</param>
    public sealed override void UpdateEffect(NedaoObject target, double delta)
    {
        _duration -= delta;

        if (_duration <= 0)
        {
            RemoveEffect(target);
            RaiseEffectEnd();

            return;
        }

        UpdateEffectCore(target, delta);
    }

    /// <summary>
    /// Provides a hook for derived classes to add custom update logic while the effect is active.
    /// Called from <see cref="UpdateEffect"/> before the effect ends.
    /// </summary>
    /// <param name="target">The object affected by the effect.</param>
    /// <param name="delta">The time step for the update.</param>
    protected virtual void UpdateEffectCore(NedaoObject target, double delta)
    {
    }
}
