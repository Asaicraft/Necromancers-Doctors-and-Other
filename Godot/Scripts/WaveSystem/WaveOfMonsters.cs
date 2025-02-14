using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[GlobalClass]
public partial class WaveOfMonsters: Resource
{
    
    public List<SmallWave> SmallWaves
    {
        get; set;
    }
}
