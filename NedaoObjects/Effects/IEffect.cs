namespace NedaoObjects.Effects;

/// <summary>
/// Defines the contract for effects that can be applied to a <see cref="NedaoObject"/>.
/// </summary>
public interface IEffect
{
    /// <summary>
    /// Occurs when the effect ends.
    /// </summary>
    public event Action<Effect>? OnEffectEnd;

    /// <summary>
    /// Applies the effect to the specified target object.
    /// </summary>
    /// <param name="target">The object to which the effect is applied.</param>
    public void ApplyEffect(NedaoObject target);

    /// <summary>
    /// Updates the effect over time.
    /// </summary>
    /// <param name="target">The object affected by the effect.</param>
    /// <param name="delta">The time step for the update.</param>
    public void UpdateEffect(NedaoObject target, double delta);

    /// <summary>
    /// Removes the effect from the specified target object.
    /// </summary>
    /// <param name="target">The object from which the effect is removed.</param>
    public void RemoveEffect(NedaoObject target);
}