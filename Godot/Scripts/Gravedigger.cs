using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class Gravedigger: NedaoProxy
{

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		var input = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		Velocity = input * Stats.Speed;
		MoveAndSlide();
	}

}
