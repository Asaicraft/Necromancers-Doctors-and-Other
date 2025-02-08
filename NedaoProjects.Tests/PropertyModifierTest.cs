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
}
