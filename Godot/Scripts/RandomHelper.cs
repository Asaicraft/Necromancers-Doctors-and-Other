using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class RandomHelper
{
    public static double NextDouble(this Random random, double min, double max)
    {
        return random.NextDouble() * (max - min) + min;
    }
}
