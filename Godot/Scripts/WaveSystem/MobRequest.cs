using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Tool]
[GlobalClass]
public partial class MobRequest : Resource
{
    [Export]
    public StatsProxy Stats
    {
        get; set;
    }

    [Export]
    public GainProxy Gain
    {
        get; set;
    }

    [Export]
    public PackedScene? Enemy
    {
        get; set;
    }

    [Export]
    public int Cost
    {
        get; set;
    }
}
