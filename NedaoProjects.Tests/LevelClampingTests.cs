using NedaoObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NedaoProjects.Tests;
public class LevelClampingTests
{
    [Fact]
    public void LevelIsClampedWithinValidRange()
    {
        var hero = new NedaoObject { Level = -100 };

        Assert.Equal(1, hero.Level);

        hero.Level = 1000;

        Assert.Equal(NedaoObject.MaxLevel, hero.Level);
    }
}
