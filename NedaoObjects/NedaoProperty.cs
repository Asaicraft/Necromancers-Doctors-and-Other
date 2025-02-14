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

    /// <summary>
    /// The base value of the property.
    /// </summary>
    protected T _baseValue;

    /// <summary>
    /// The total bonus value applied to the base value.
    /// </summary>
    protected T _totalBonus;

    /// <summary>
    /// The final computed value of the property after applying all bonuses.
    /// </summary>
    protected T _totalValue;

    /// <summary>
    /// Occurs when <see cref="InvalidState"/> is called.
    /// This event signals that the total value and bonuses are being recalculated.
    /// </summary>
    public event Action<T>? OnInvalidateStateCalled;

    /// <summary>
    /// Occurs before the base value of the property changes.
    /// The event provides the new value before the update.
    /// </summary>
    public event Action<T>? OnBaseValueChanging;

    /// <summary>
    /// Occurs after the base value of the property has changed.
    /// The event provides the updated value.
    /// </summary>
    public event Action<T>? OnBaseValueChanged;

    /// <summary>
    /// Occurs before the total bonus value changes.
    /// The event provides the new total bonus before the update.
    /// </summary>
    public event Action<T>? OnTotalBonusChanging;

    /// <summary>
    /// Occurs after the total bonus value has changed.
    /// The event provides the updated total bonus.
    /// </summary>
    public event Action<T>? OnTotalBonusChanged;

    /// <summary>
    /// Occurs before the total value of the property changes.
    /// The event provides the new total value before the update.
    /// </summary>
    public event Action<T>? OnTotalValueChanging;

    /// <summary>
    /// Occurs after the total value of the property has changed.
    /// The event provides the updated total value.
    /// </summary>
    public event Action<T>? OnTotalValueChanged;

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
        get => _totalBonus;
        protected set => SetTotalBonus(value);
    }

    /// <summary>
    /// Gets the final value of the property after applying bonuses.
    /// </summary>
    public T TotalValue
    {
        get => _totalValue;
        protected set => SetTotalValue(value);
    }

    /// <summary>
    /// Sets the base value of the property.
    /// Designed to be overridden in derived classes.
    /// </summary>
    /// <param name="value">The new base value.</param>
    protected virtual void SetBaseValue(T value)
    {
        OnBaseValueChanging?.Invoke(value);
        _baseValue = value;
        OnBaseValueChanged?.Invoke(value);
        InvalidStateCore();
    }

    /// <summary>
    /// Sets the total bonus value and raises the corresponding events.
    /// </summary>
    /// <param name="value">The new total bonus value.</param>
    protected virtual void SetTotalBonus(T value)
    {
        OnTotalBonusChanging?.Invoke(value);
        _totalBonus = value;
        OnTotalBonusChanged?.Invoke(value);
    }

    /// <summary>
    /// Sets the total computed value and raises the corresponding events.
    /// </summary>
    /// <param name="value">The new total value.</param>
    protected virtual void SetTotalValue(T value)
    {
        OnTotalValueChanging?.Invoke(value);
        _totalValue = value;
        OnTotalValueChanged?.Invoke(value);
    }

    /// <summary>
    /// Recalculates the total bonus and updates the total value.
    /// Should be called when bonuses change.
    /// </summary>
    public void InvalidState()
    {
        OnInvalidateStateCalled?.Invoke(TotalValue);
        InvalidStateCore();
    }

    /// <summary>
    /// Performs the core logic of recalculating the total bonus and total value.
    /// Called internally when the property state needs to be updated.
    /// </summary>
    protected virtual void InvalidStateCore()
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
            TotalBonus = bonus.Modify(this);
        }

        return TotalBonus;
    }

    #region ICollection<PropertyModifier<T>> implementation

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
        InvalidStateCore();
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
        InvalidStateCore();
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
            InvalidStateCore();
        }

        return result;
    }

    /// <summary>
    /// Implicit conversion operator to retrieve the total value of the property.
    /// </summary>
    /// <param name="property">The property instance.</param>
    public static implicit operator T(NedaoProperty<T> property)
    {
        return property.TotalValue;
    }
}

