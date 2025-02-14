using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Enemy : NedaoProxy
{
	private Tombstone[] _tombstoneList = [];

	[Export]
	public NavigationAgent2D NavigationAgent2D
	{
		get; set;
	} = null!;

	[Export]
	public GameLevel GameLevel
	{
		get; set;
	}

	[Export]
	public Timer PathFindTimer
	{
		get; set;
	}

	[Export]
	public Timer AggroTimeout
	{
		get; set;
	}

	[Export]
	public Sprite2D Sprite
	{
		get; set;
	}

	public override void _Ready()
	{
		base._Ready();

		PathFindTimer.Timeout += UpdateTarget;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		var minDistance = Target.AttackRange * .7f;

		if(GlobalPosition.DistanceTo(NavigationAgent2D.TargetPosition) < minDistance)
		{
			return;
		}

		var nextPath = NavigationAgent2D.GetNextPathPosition();

		var direction = ToLocal(nextPath).Normalized();

		Velocity = direction * Target.Speed;

		Sprite.FlipH = direction.X < 0;

		MoveAndSlide();
	}

	private void UpdateTarget()
	{
		if (AggroTimeout.TimeLeft <= 0)
		{
			NavigationAgent2D.TargetPosition = FindNearestTombstone()?.GlobalPosition ?? GlobalPosition;
			return;
		}

		NavigationAgent2D.TargetPosition = FindNearestEnemy().GlobalPosition;
	}

	private Node2D FindNearestEnemy()
	{
		if(GameLevel.Gravedigger == null)
		{
			return this;
		}

		Node2D nearestEnemy = GameLevel.Gravedigger;
		var nearestDistance = GlobalPosition.DistanceTo(nearestEnemy.GlobalPosition);

		if (GameLevel.Tombstones.Length == 0)
		{
			return this;
		}

		for (var i = 0; i < GameLevel.Tombstones.Length; i++)
		{
			var enemy = GameLevel.Tombstones[i];
			var distance = GlobalPosition.DistanceTo(enemy.GlobalPosition);
			if (distance < nearestDistance)
			{
				nearestEnemy = enemy;
				nearestDistance = distance;
			}
		}

		return nearestEnemy;
	}


	private Tombstone? FindNearestTombstone()
	{
		if(GameLevel.Tombstones.Length == 0)
		{
			return null;
		}

		var nearestTombstone = GameLevel.Tombstones[0];
		var nearestDistance = GlobalPosition.DistanceTo(nearestTombstone.GlobalPosition);

		for (var i = 0; i < _tombstoneList.Length; i++)
		{
			var tombstone = _tombstoneList[i];

			var distance = GlobalPosition.DistanceTo(tombstone.GlobalPosition);

			if (distance < nearestDistance)
			{
				nearestTombstone = tombstone;
				nearestDistance = distance;
			}
		}

		return nearestTombstone;
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);

		if (disposing)
		{
			PathFindTimer.Timeout -= UpdateTarget;
		}
	}
}
