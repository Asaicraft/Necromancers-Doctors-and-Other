using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class GameLevel: Node2D
{

	[Export]
	public Tombstone[] Tombstones
	{
		get; set;
	}

	[Export]
	public Gravedigger Gravedigger
	{
		get; set;
	}

	public override void _Ready()
	{
		base._Ready();

		Tombstones ??= [];
	}
}
