using NedaoObjects;
using NedaoObjects.Gains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NedaoProjects.Tests;
public class GainModiferTests
{
    [Theory]
    [InlineData(10, 5, 2, 1, 0.5f, 1, 1.5f, 5, 10)]  // Standard case
    [InlineData(20, 10, 4, 2, 1f, 2, 3f, 0, 15)]  // Starting level 0
    [InlineData(15, 7.5f, 3, 1.5f, 0.75f, 1.5f, 2.5f, 10, 20)] // Starting level 10
    [InlineData(5, 2.5f, 1, 0.5f, 0.25f, 0.5f, 1f, 20, 10)] // Starting level 20, test for MaxLevel restriction
    [InlineData(12, 6, 2.5f, 1.2f, 0.6f, 1.2f, 2f, 23, 5)] // Leveling up to MaxLevel
    public void SimpleGainModifierTest(
        float maxHealthGain, float damageGain, float armorGain,
        float speedGain, float attackSpeedGain, float attackRangeGain,
        float baseAttackTimeGain, int startLevel, int levelDifference)
    {
        var hero = new NedaoObject { Level = startLevel };

        var gainModifier = (SimpleGainModifier)hero.GainModifier;

        gainModifier.MaxHealth.BaseValue = maxHealthGain;
        gainModifier.Damage.BaseValue = damageGain;
        gainModifier.Armor.BaseValue = armorGain;
        gainModifier.Speed.BaseValue = speedGain;
        gainModifier.AttackSpeed.BaseValue = attackSpeedGain;
        gainModifier.AttackRange.BaseValue = attackRangeGain;
        gainModifier.BaseAttackTime.BaseValue = baseAttackTimeGain;

        var expectedFinalLevel = Math.Min(startLevel + levelDifference, NedaoObject.MaxLevel);
        var actualLevelIncrease = expectedFinalLevel - startLevel;

        hero.Level += levelDifference;

        Assert.Equal(maxHealthGain * actualLevelIncrease, hero.MaxHealth);
        Assert.Equal(damageGain * actualLevelIncrease, hero.Damage);
        Assert.Equal(armorGain * actualLevelIncrease, hero.Armor);
        Assert.Equal(speedGain * actualLevelIncrease, hero.Speed);
        Assert.Equal(attackSpeedGain * actualLevelIncrease, hero.AttackSpeed);
        Assert.Equal(attackRangeGain * actualLevelIncrease, hero.AttackRange);
        Assert.Equal(baseAttackTimeGain * actualLevelIncrease, hero.BaseAttackTime);
    }
}
