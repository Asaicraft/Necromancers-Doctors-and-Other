using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class SmallWave: GodotObject
{
    [Export]
    public int Cost
    {
        get; set;
    }

    [Export]
    public double SmallWaveDuration
    {
        get; set;
    }

    public List<MobRequest> MobRequests
    {
        get; set;
    }

}
