using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NedaoObjects;
public abstract class Bonus<T> where T : struct, IBinaryNumber<T>
{
    public abstract T Affect(NedaoProperty<T> property);
}
