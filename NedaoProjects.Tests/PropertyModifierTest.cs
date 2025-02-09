using NedaoObjects;
using NedaoObjects.Bonuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NedaoProjects.Tests;


public class PropertyModifierTest
{
    [Fact]
    public void AdditiveValueModifierTest()
    {
        // TotalBonus = 0 + 5 = 5
        // TotalValue = 10 + TotalBonus = 15

        var property = new NedaoProperty<int>()
        {
            BaseValue = 10
        };

        var modifier = new ValueModifier<int>
        {
            Value = 5,
            Operation = ModifierOperation.Additive
        };

        property.Add(modifier);

        var totalBonus = property.TotalBonus;
        var totalValue = property.TotalValue;

        Assert.Equal(15, totalValue);
        Assert.Equal(5, totalBonus);
    }

    [Fact]
    public void MultiplicativeAndAdditiveValueModifierTest()
    {
        // TotalBonus = 0 + 10 * 0.5 = 5
        // TotalValue = 10 + TotalBonus = 15

        var property = new NedaoProperty<float>()
        {
            BaseValue = 10
        };

        var additiveModifier = new ValueModifier<float>
        {
            Value = 10,
            Operation = ModifierOperation.Additive
        };

        var modifier = new ValueModifier<float>
        {
            Value = 0.5f,
            Operation = ModifierOperation.Multiplicative
        };

        property.Add(additiveModifier);
        property.Add(modifier);

        var totalBonus = property.TotalBonus;
        var totalValue = property.TotalValue;

        Assert.Equal(5, totalBonus);
        Assert.Equal(15, totalValue);
    }

    [Fact]
    public void ALotOfAdditiveValueModifierTest()
    {
        // TotalBonus = 0 + 5 + 5 + 5 + 5 + 5 = 25
        // TotalValue = 10 + TotalBonus = 35
        var property = new NedaoProperty<int>()
        {
            BaseValue = 10
        };

        var modifier = new ValueModifier<int>
        {
            Value = 5,
            Operation = ModifierOperation.Additive
        };

        property.Add(modifier);
        property.Add(modifier);
        property.Add(modifier);
        property.Add(modifier);
        property.Add(modifier);

        var totalBonus = property.TotalBonus;
        var totalValue = property.TotalValue;

        Assert.Equal(35, totalValue);
        Assert.Equal(25, totalBonus);
    }

    [Fact]
    public void OverrideModifierIgnoresPreviousBonuses()
    {
        var property = new NedaoProperty<float>()
        {
            BaseValue = 10
        };

        var cubingModifier = new OverrideCubingModifier();
        var additiveModifier = new ValueModifier<float>
        {
            Value = 71,
            Operation = ModifierOperation.Additive
        };

        property.Add(additiveModifier);
        property.Add(cubingModifier);

        var totalBonus = property.TotalBonus;
        var totalValue = property.TotalValue;

        // Expected values explanation:
        // 1. AdditiveModifier adds 71 to the TotalBonus (but this is ignored later).
        // 2. OverrideCubingModifier replaces TotalBonus with BaseValue^3 => 10^3 = 1000.
        // 3. TotalValue = BaseValue + TotalBonus => 10 + 1000 = 1010.

        Assert.Equal(1000, totalBonus);
        Assert.Equal(1010, totalValue);
    }

    /// <summary>
    /// A modifier that completely overrides the total bonus by cubing the base value.
    /// </summary>
    private class OverrideCubingModifier : PropertyModifier<float>
    {

        /// <summary>
        /// Overrides the total bonus with the cube of the base value.
        /// This modifier **ignores all previous bonuses** and directly sets the total bonus.
        /// </summary>
        /// <param name="property">The property being modified.</param>
        /// <returns>The cubed base value as the new total bonus.</returns>
        public override float Modify(NedaoProperty<float> property)
        {
            return MathF.Pow(property.BaseValue, 3);
        }
    }
}
