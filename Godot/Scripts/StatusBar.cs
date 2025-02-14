using Godot;
using NedaoObjects;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class StatusBar : Node2D
{
	private NedaoProxy? _target;

	[Export]
	public NedaoProxy? Target
	{
		get => _target;
		set
		{
			ClearSubscription(_target);
			_target = value;
			Subscribe(_target);
		}
	}

	[Export]
	public ProgressBar? HpBar
	{
		get; set;
	}

	[Export]
	public Label? HpLabel
	{
		get; set;
	}

	[Export]
	public ProgressBar? AttackCooldownBar
	{
		get; set;
	}

	[Export]
	public int HpPerHpDelimiter
	{
		get; set;
	}

	[Export]
	public HBoxContainer? HpDelimeters
	{
		get; set;
	}

	[Export]
	public PackedScene? HpDelimeter
	{
		get; set;
	}

	[Export]
	public TextureProgressBar? ExpProgressBar
	{
		get; set;
	}

	[Export]
	public Label? LevelLabel
	{
		get; set;
	}

	public float AttackCooldown
	{
		get; set;
	}

	public float AttackCooldownMax
	{
		get; set;
	}

	public float HpMax
	{
		get; set;
	}

	public float HpCurrent
	{
		get; set;
	}

	public int Level
	{
		get; set;
	}

	public float Exp
	{
		get; set;
	}

	public override void _Ready()
	{
		if (Target == null)
		{
			GD.Print("Target is null");
			return;
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if(Target == null)
		{
			return;
		}

		AttackCooldown = (float)Target.Target.AttackBehavior.Cooldown;
		HpCurrent = Target.Target.Health;
		Exp = Target.Target.Exp;
		Level = Target.Target.Level;

		if (HpBar != null)
		{
			HpBar.Value = (HpCurrent / HpMax) * 100;
		}
		
		if(HpLabel != null)
		{
			HpLabel.Text = $"{(int)HpCurrent}";
		}

		if(AttackCooldownBar != null)
		{
			AttackCooldownBar.Value = (1 - (AttackCooldown / AttackCooldownMax)) * 100;
		}

		if(LevelLabel != null)
		{
			LevelLabel.Text = Level.ToString();
		}

		if (ExpProgressBar != null)
		{
			if(Level == NedaoObject.MaxLevel)
			{
				ExpProgressBar.Value = 100;
				return;
			}

			var maxExp = NedaoObject.ExpToLevels[Target.Target.Level + 1];

			ExpProgressBar.Value = (Exp / maxExp) * 100;
		}
	}

	public override void _ExitTree()
	{
		ClearSubscription(_target);
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);

		if (disposing)
		{
			ClearSubscription(_target);
		}
	}

	private void UpdateHpDelimeters()
	{
		if (HpDelimeters == null || HpDelimeter == null)
		{
			return;
		}

		ClearHpDelimeters();

		var barWidth = HpDelimeters.OffsetRight;

		var hpDelimeterCount = (int)(HpMax / HpPerHpDelimiter);
		var delimeterMargin = barWidth / hpDelimeterCount;

		HpDelimeters.OffsetLeft = delimeterMargin;
		HpDelimeters.AddThemeConstantOverride("separation", (int)delimeterMargin);

		GD.Print("BarWidth: " + barWidth);
		GD.Print("MaxHp: " + HpMax);
		GD.Print("HpDelimeterCount: " + hpDelimeterCount);
		GD.Print("DelimeterMargin: " + delimeterMargin);

		// Skip the first delimeter
		for (var i = 1; i < hpDelimeterCount; i++)
		{
			var hpDelimeter = HpDelimeter.Instantiate();
			HpDelimeters.AddChild(hpDelimeter);
		}
	}

	private void ClearHpDelimeters()
	{
		if (HpDelimeters == null)
		{
			return;
		}

		var children = HpDelimeters.GetChildren();

		if(children.Count == 0)
		{
			return;
		}

		var tempArray = ArrayPool<Node>.Shared.Rent(children.Count);
		children.CopyTo(tempArray, 0);

		try
		{
			for (var i = 0; i < children.Count; i++)
			{
				HpDelimeters.RemoveChild(tempArray[i]);
				tempArray[i].QueueFree();
			}
		}
		finally
		{
			ArrayPool<Node>.Shared.Return(tempArray);
		}
	}

	private void OnMaxHealthChanged(float maxHealth)
	{
		if (Target == null)
		{
			return;
		}

		HpMax = maxHealth;
		UpdateHpDelimeters();
	}

	private void OnAttackSpeedChanged(float attackSpeed)
	{
		if(Target == null)
		{
			return;
		}

		AttackCooldownMax = Target.Target.CalculateAttackSpeed();
	}

	private void ClearSubscription(NedaoProxy? nedaoProxy)
	{
		if(nedaoProxy == null)
		{
			return;
		}

		nedaoProxy.Target.MaxHealth.OnTotalValueChanged -= OnMaxHealthChanged;
		nedaoProxy.Target.AttackSpeed.OnTotalValueChanged -= OnAttackSpeedChanged;
		nedaoProxy.Target.BaseAttackTime.OnTotalValueChanged -= OnAttackSpeedChanged;
	}

	private void Subscribe(NedaoProxy? nedaoProxy)
	{
		if (nedaoProxy == null)
		{
			return;
		}

		nedaoProxy.Target.MaxHealth.OnTotalValueChanged += OnMaxHealthChanged;
		nedaoProxy.Target.AttackSpeed.OnTotalValueChanged += OnAttackSpeedChanged;
		nedaoProxy.Target.BaseAttackTime.OnTotalValueChanged += OnAttackSpeedChanged;
	}
}
