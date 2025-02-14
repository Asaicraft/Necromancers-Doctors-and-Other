using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[GlobalClass]
public abstract partial class WaveAction: Resource
{
    public abstract void Execute(WaveContext context);
}
