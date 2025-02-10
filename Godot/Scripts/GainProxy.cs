using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[GlobalClass]
public partial class GainProxy: Resource
{
    [Export]
    public int MaxHealth
    {
        get; set;
    }
}
