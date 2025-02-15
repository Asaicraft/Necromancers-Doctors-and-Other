using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[GlobalClass]
public partial class SpawnInfo: Resource
{
    [Export]
    public Array<WaveAction>? Actions
    {
        get; set;
    }

    [Export]
    public int Points
    {
        get; set;
    }

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
}
