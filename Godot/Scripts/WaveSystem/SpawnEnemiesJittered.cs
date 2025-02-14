using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[GlobalClass]
public partial class SpawnEnemiesJittered : WaveAction
{
    [Export]
    public double MinDelay
    {
        get; set;
    } = .6d;

    [Export]
    public double MaxDelay
    {
        get; set;
    } = 2.3d;

    /// <summary>
    /// if <code>0</code>, the <see cref="WaveContext"/> points will be used
    /// </summary>
    [Export]
    public int Points
    {
        get; set;
    }

    /// <summary>
    /// if not <code>0</code>,, the <see cref="WaveContext"/> points will be incremented by this value
    /// </summary>
    [Export]
    public int IncrementPoints
    {
        get; set;
    }

    [Export]
    public Array<MobRequest> Mobs
    {
        get; set;
    }

    public override void Execute(WaveContext context)
    {
        Points = Points <= 0 ? context.WavePoints : Points;

        if (IncrementPoints > 0)
        {
            context.WavePoints += IncrementPoints;
        }

        if(Mobs is null || Mobs.Count == 0)
        {
            GD.PrintErr("No mobs to spawn");
            return;
        }

        var mobs = SpendPoints();
        var delay = 0d;

        for (var i = 0; i < mobs.Length; i++)
        {
            var mob = mobs[i];

            delay += Random.Shared.NextDouble(MinDelay, MaxDelay);

            context.CreateMob(mob, delay);
        }
    }

    private MobRequest[] SpendPoints()
    {
        var points = Points; 
        var mobs = new List<MobRequest>();

        while (points > 0)
        {
            var validMobs = Mobs.Where(m => m.Cost <= points).ToList();
            if (validMobs.Count == 0)
            {
                break;
            }

            var mob = SelectRandomRequest(validMobs);
            points -= mob.Cost;
            mobs.Add(mob);
        }

        return [.. mobs];
    }

    private static MobRequest SelectRandomRequest(List<MobRequest> pool)
    {
        var totalProbability = 0d;
        foreach (var mob in pool)
        {
            totalProbability += mob.Probability;
        }

        var random = Random.Shared.NextDouble() * totalProbability;

        foreach (var mob in pool)
        {
            random -= mob.Probability;
            if (random <= 0)
            {
                return mob;
            }
        }

        return pool[^1];
    }
}
