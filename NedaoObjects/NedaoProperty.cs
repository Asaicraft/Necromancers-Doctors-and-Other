using NedaoObjects.Bonuses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NedaoObjects;

/// <summary>
/// Represents a property with a base value and bonuses.
/// Implements <see cref="ICollection{T}"/> and is designed to be extended in derived classes.
/// </summary>
/// <typeparam name="T">The numeric type of the property, constrained to <see cref="IBinaryNumber{T}"/>.</typeparam>
public class NedaoProperty<T> : ICollection<PropertyModifier<T>> where T : struct, IBinaryNumber<T>
{
    /// <summary>
    /// The list of bonuses applied to this property.
    /// </summary>
    protected readonly List<PropertyModifier<T>> Bonuses = [];

    private T _baseValue;

    /// <summary>
    /// Gets or sets the base value of the property.
    /// Setting a new value updates the total value.
    /// </summary>
    public T BaseValue
    {
        get => _baseValue;
        set => SetBaseValue(value);
    }

    /// <summary>
    /// Gets the total bonus applied to the base value.
    /// </summary>
    public T TotalBonus
    {
        get;
        protected set;
    }

    /// <summary>
    /// Gets the final value of the property after applying bonuses.
    /// </summary>
    public T TotalValue
    {
        get;
        protected set;
    }

    /// <summary>
    /// Sets the base value of the property.
    /// Designed to be overridden in derived classes.
    /// </summary>
    /// <param name="value">The new base value.</param>
    protected virtual void SetBaseValue(T value)
    {
        _baseValue = value;
    }

    /// <summary>
    /// Recalculates the total bonus and updates the total value.
    /// Should be called when bonuses change.
    /// </summary>
    public virtual void InvalidState()
    {
        CountBonuses();
        TotalValue = BaseValue + TotalBonus;
    }

    /// <summary>
    /// Recalculates the total bonus by applying all bonuses in the collection.
    /// Designed to be overridden in derived classes.
    /// </summary>
    /// <returns>The total bonus after applying all effects.</returns>
    protected virtual T CountBonuses()
    {
        TotalBonus = default;

        var bonuses = Bonuses.ToArray();

        for (var i = 0; i < bonuses.Length; i++)
        {
            var bonus = bonuses[i];
            TotalBonus = bonus.Affect(this);
        }

        return TotalBonus;
    }

    #region ICollection<Bonus<T>> implementation

    /// <inheritdoc />
    public int Count => Bonuses.Count;

    /// <inheritdoc />
    public bool IsReadOnly => false;

    /// <inheritdoc />
    public void Add(PropertyModifier<T> bonus)
    {
        AddCore(bonus);
    }

    /// <inheritdoc />
    public void Clear()
    {
        ClearCore();
    }

    /// <inheritdoc />
    public bool Contains(PropertyModifier<T> item)
    {
        return Bonuses.Contains(item);
    }

    /// <inheritdoc />
    public void CopyTo(PropertyModifier<T>[] array, int arrayIndex)
    {
        Bonuses.CopyTo(array, arrayIndex);
    }

    /// <inheritdoc />
    public bool Remove(PropertyModifier<T> item)
    {
        return RemoveCore(item);
    }

    /// <inheritdoc />
    public IEnumerator<PropertyModifier<T>> GetEnumerator()
    {
        return Bonuses.GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return Bonuses.GetEnumerator();
    }

    #endregion

    /// <summary>
    /// Adds a bonus to the collection.
    /// Designed to be overridden in derived classes.
    /// </summary>
    /// <param name="bonus">The bonus to add.</param>
    protected virtual void AddCore(PropertyModifier<T> bonus)
    {
        Bonuses.Add(bonus);
        InvalidState();
    }

    /// <summary>
    /// Clears all bonuses from the collection.
    /// Calls <see cref="RemoveCore(PropertyModifier{T})"/> for each bonus to ensure proper cleanup.
    /// Designed to be overridden in derived classes.
    /// </summary>
    protected virtual void ClearCore()
    {
        var bonusesCopy = Bonuses.ToArray();

        foreach (var bonus in bonusesCopy)
        {
            RemoveCore(bonus);
        }

        Bonuses.Clear();
        InvalidState();
    }

    /// <summary>
    /// Removes a bonus from the collection.
    /// Designed to be overridden in derived classes.
    /// </summary>
    /// <param name="bonus">The bonus to remove.</param>
    /// <returns>True if the bonus was successfully removed; otherwise, false.</returns>
    protected virtual bool RemoveCore(PropertyModifier<T> bonus)
    {
        var result = Bonuses.Remove(bonus);

        if (result)
        {
            InvalidState();
        }

        return result;
    }
}
