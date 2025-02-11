using NedaoObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NedaoProjects.Tests;

public class AttackTest
{
    [Fact]
    public void TestAttack()
    {
        var attacker = new NedaoObject();
        attacker.MaxHealth.BaseValue = 100;
        attacker.Helth = 100;
        attacker.Damage.BaseValue = 10;

        attacker.AttackBehavior.Cooldown = -0;

        var target = new NedaoObject();
        target.MaxHealth.BaseValue = 100;
        target.Helth = 100;

        var isAttackSuccessful = attacker.TryAttackNedao(target);

        Assert.True(isAttackSuccessful);
        Assert.Equal(90, target.Helth);

        isAttackSuccessful = attacker.TryAttackNedao(target);

        Assert.False(isAttackSuccessful);
    }
}
