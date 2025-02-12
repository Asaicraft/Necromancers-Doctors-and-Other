using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Enemy: NedaoProxy
{
	private Tombstone[] _tombstoneList = [];

	[Export]
	public NavigationAgent2D NavigationAgent2D
	{
		get; set;
	} = null!;

	[Export]
	public Gravedigger Gravedigger
	{
		get; set;
	} = null!;

	[Export]
	public Tombstone[] Tombstones
	{
		get => _tombstoneList;
		set => _tombstoneList = value ?? [];
	}

	[Export]
	public Timer Timer
	{
		get; set;
	}

	public override void _Ready()
	{
		base._Ready();

		Timer.Timeout += UpdateTarget;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		var nextPath = NavigationAgent2D.GetNextPathPosition();

		var direction = ToLocal(nextPath).Normalized();

		Velocity = direction * Target.Speed;

		MoveAndSlide();
	}

	private void UpdateTarget()
	{
		NavigationAgent2D.TargetPosition = Gravedigger?.GlobalPosition ?? GlobalPosition;
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);

		if (disposing)
		{
			Timer.Timeout -= UpdateTarget;
		}
	}
}
