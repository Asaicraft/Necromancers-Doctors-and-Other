using Godot;
using Godot.Collections;
using NedaoObjects;
using NedaoObjects.Gains;
using System;

public partial class NedaoProxy : CharacterBody2D
{
	/// <summary>
	/// Prefers to use the target property instead of this field.
	/// </summary>
	private NedaoObject? _target;

	[Export]
	public StatsProxy Stats
	{
		get; set;
	} = new();

	[ExportGroup("Level")]
	[Export]
	public int Level
	{
		get; set;
	} = 1;

	[Export]
	public int Exp
	{
		get; set;
	}

	/// <summary>
	/// If true, the level is set before gaining experience.
	/// Be aware that the level may increase as a result.
	/// </summary>
	[Export]
	public bool SetLevelBeforeGain
	{
		get; set;
	} = true;

	[Export]
	public GainProxy Gain
	{
		get; set;
	} = new();

	[ExportGroup("Teams And Enemies")]
	[Export]
	public Mobs Team
	{
		get; set;
	}

	[Export]
	public Mobs Enemies
	{
		get; set;
	}


	[ExportGroup("Anothers")]
	[Export]
	public bool CanAttack
	{
		get; set;
	}


	public NedaoObject Target
	{
		get => _target ??= CreateTarget();
	}


	public override void _Ready()
	{
		Stats.ApplyTo(Target);

		if (SetLevelBeforeGain)
		{
			Target.Level = Level;

			// Warning: The level may increase, which may not be intended.
			Target.TakeExp(Exp);
		}

		Gain.ApplyTo(Target);

		if (!SetLevelBeforeGain)
		{
			Target.Level = Level;

			// Warning: The level may increase, which may not be intended.
			Target.TakeExp(Exp);
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		Target.Update(delta);
	}

	public virtual bool TryAttack(NedaoProxy nedaoProxy)
	{
		return Target.TryAttackNedao(nedaoProxy.Target);
	}

	/// <summary>
	/// Creates and initializes a new <see cref="NedaoObject"/> instance.
	/// This method is called lazily when accessing the <see cref="Target"/> property for the first time.
	/// Designed to be overridden in derived classes to customize object creation.
	/// </summary>
	/// <returns>A new instance of <see cref="NedaoObject"/>.</returns>
	protected virtual NedaoObject CreateTarget()
	{
		return new();
	}

	private static int ValidateLevel(int value)
	{
		if (value < 1)
		{
			return 1;
		}

		return value > NedaoObject.MaxLevel ? NedaoObject.MaxLevel : value;
	}
}
