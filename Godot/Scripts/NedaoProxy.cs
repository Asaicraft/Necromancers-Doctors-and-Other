using Godot;
using Godot.Collections;
using NedaoObjects;
using NedaoObjects.Gains;
using System;

public partial class NedaoProxy : Node
{
    /// <summary>
    /// Prefers to use the target property instead of this field.
    /// </summary>
    private NedaoObject? _target;

    [ExportGroup("Stats")]
	[Export]
	public float MaxHealth
	{
		get; set;
	}

    [Export]
    public float Health
    {
		get; set;
    }

    [Export]
	public float Damage
	{
		get; set;
	}

	[Export]
	public float Armor
	{
		get; set;
	}

	[Export]
	public float Speed
	{
		get; set;
	}

	[Export]
	public float AttackSpeed
	{
		get; set;
	}

	[Export]
	public float AttackRange
	{
		get; set;
	}

	[Export]
	public float BaseAttackTime
	{
		get; set;
	}

	[ExportGroup("Level")]
	[Export]
    public int Level
	{
		get; set;
	}

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
    /// <summary>
    /// If true, the level is set before gaining experience.
    /// Be aware that the level may increase as a result.
    /// </summary>
    public bool SetLevelBeforeGain
	{
		get; set;
	}

	[Export]
	public GainProxy Gain
	{
		get; set;
	} = new();

	public NedaoObject Target
	{
		get => _target ??= CreateTarget();
    }


    public override void _Ready()
    {
        Target.Helth = Health;
        Target.MaxHealth.BaseValue = MaxHealth;
        Target.Damage.BaseValue = Damage;
        Target.Armor.BaseValue = Armor;
        Target.Speed.BaseValue = Speed;
        Target.AttackSpeed.BaseValue = AttackSpeed;
        Target.AttackRange.BaseValue = AttackRange;
        Target.BaseAttackTime.BaseValue = BaseAttackTime;

        if (SetLevelBeforeGain)
        {
            Target.Level = Level;

            // Warning: The level may increase, which may not be intended.
            Target.TakeExp(Exp);
        }

        if (Target.GainModifier is SimpleGainModifier gainModifier)
        {
            gainModifier.MaxHealth.BaseValue = MaxHealth;
            gainModifier.Damage.BaseValue = Damage;
            gainModifier.Armor.BaseValue = Armor;
            gainModifier.Speed.BaseValue = Speed;
            gainModifier.AttackSpeed.BaseValue = AttackSpeed;
            gainModifier.AttackRange.BaseValue = AttackRange;
            gainModifier.BaseAttackTime.BaseValue = BaseAttackTime;
        }

        if (!SetLevelBeforeGain)
        {
            Target.Level = Level;

            // Warning: The level may increase, which may not be intended.
            Target.TakeExp(Exp);
        }
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
