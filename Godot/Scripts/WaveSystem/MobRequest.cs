using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[GlobalClass]
public partial class MobRequest : Resource
{
    [Export]
    public PackedScene Enemy 
    { 
        get; set; 
    }

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

    [ExportGroup("Random")]
    [Export]
    public int Cost
    {
        get; set;
    }

    [Export]
    public double Probability
    {
        get; set;
    } = 1;

    [Export]
    public double MinDelay
    {
        get; set;
    } = 0.6d;

    [Export]
    public double MaxDelay
    {
        get; set;
    } = 3.1d;

    public Enemy Instantiate()
    {
        var mob = (Enemy)Enemy.Instantiate();

        mob.Stats = Stats ?? mob.Stats;
        mob.Gain = Gain ?? mob.Gain;

        return mob;
    }
}
