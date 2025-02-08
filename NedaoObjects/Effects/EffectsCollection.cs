using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NedaoObjects.Effects;

/// <summary>
/// Represents a collection of effects associated with a specific <see cref="NedaoObject"/>.
/// This class is designed to be extended in derived classes.
/// </summary>
public class EffectsCollection(NedaoObject associatedObject) : ICollection<IEffect>
{
    /// <summary>
    /// The list of effects contained in the collection.
    /// </summary>
    protected readonly List<IEffect> Effects = [];

    /// <summary>
    /// The object to which the effects are applied.
    /// </summary>
    protected readonly NedaoObject AssociatedObject = associatedObject;

    /// <summary>
    /// Updates all effects in the collection.
    /// Can be overridden in a derived class to provide custom update logic.
    /// </summary>
    /// <param name="delta">The time step for the update.</param>
    public void Update(double delta)
    {
        UpdateCore(delta);
    }

    /// <summary>
    /// Updates each effect in the collection using the associated object.
    /// Designed to be overridden in derived classes.
    /// </summary>
    /// <param name="delta">The time step for the update.</param>
    protected virtual void UpdateCore(double delta)
    {
        foreach (var effect in Effects)
        {
            effect.UpdateEffect(AssociatedObject, delta);
        }
    }

    /// <summary>
    /// Adds an effect to the collection and applies it to the associated object.
    /// Designed to be overridden in derived classes.
    /// </summary>
    /// <param name="effect">The effect to add.</param>
    protected virtual void AddCore(IEffect effect)
    {
        Effects.Add(effect);
        effect.ApplyEffect(AssociatedObject);

        effect.OnEffectEnd += EffectEndHandler;
    }

    /// <summary>
    /// Clears all effects from the collection and removes them from the associated object.
    /// Calls <see cref="RemoveCore(IEffect)"/> for each effect to ensure proper cleanup.
    /// Designed to be overridden in derived classes.
    /// </summary>
    protected virtual void ClearCore()
    {
        var effectsCopy = Effects.ToArray();

        foreach (var effect in effectsCopy)
        {
            RemoveCore(effect);
        }

        Effects.Clear();
    }

    /// <summary>
    /// Removes an effect from the collection and detaches it from the associated object.
    /// Designed to be overridden in derived classes.
    /// </summary>
    /// <param name="effect">The effect to remove.</param>
    /// <returns>True if the effect was successfully removed; otherwise, false.</returns>
    protected virtual bool RemoveCore(IEffect effect)
    {
        var result = Effects.Remove(effect);

        if (result)
        {
            effect.OnEffectEnd -= EffectEndHandler;
            effect.RemoveEffect(AssociatedObject);
        }

        return result;
    }

    /// <summary>
    /// Handles the <see cref="IEffect.OnEffectEnd"/> event.
    /// Automatically removes the effect from the collection when it ends.
    /// Can be overridden in a derived class to customize effect cleanup behavior.
    /// </summary>
    /// <param name="e">The effect that has ended.</param>
    protected virtual void EffectEndHandler(IEffect e) => RemoveCore(e);

    #region ICollection<IEffect> implementation

    /// <inheritdoc />
    public int Count => Effects.Count;

    /// <inheritdoc />
    public bool IsReadOnly => false;

    /// <inheritdoc />
    public void Add(IEffect effect)
    {
        AddCore(effect);
    }

    /// <inheritdoc />
    public void Clear()
    {
        ClearCore();
    }

    /// <inheritdoc />
    public bool Contains(IEffect item)
    {
        return Effects.Contains(item);
    }

    /// <inheritdoc />
    public void CopyTo(IEffect[] array, int arrayIndex)
    {
        Effects.CopyTo(array, arrayIndex);
    }

    /// <inheritdoc />
    public IEnumerator<IEffect> GetEnumerator()
    {
        return Effects.GetEnumerator();
    }

    /// <inheritdoc />
    public bool Remove(IEffect item)
    {
        return RemoveCore(item);
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return Effects.GetEnumerator();
    }

    #endregion
}