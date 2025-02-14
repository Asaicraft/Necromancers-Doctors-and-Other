using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[GlobalClass]
public partial class SpawnEnemies : WaveAction
{

    [Export]
    public Array<MobRequest> Mobs
    {
        get; set;
    }

    public override void Execute(WaveContext context)
    {
        if(Mobs is null || Mobs.Count == 0)
        {
            GD.PrintErr("No mobs to spawn");
            return;
        }

        var delay = 0d;

        for (var i = 0; i < Mobs.Count; i++)
        {
            var mob = Mobs[i];

            delay += Random.Shared.NextDouble(mob.MinDelay, mob.MaxDelay);

            context.CreateMob(mob, delay);
        }
    }
}
