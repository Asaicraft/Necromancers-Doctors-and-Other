using Godot;
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
    public int HpPerHpDelimiter
    {
        get; set;
    }

    [Export]
    public HBoxContainer HpDelimeters
    {
        get; set;
    }

    [Export]
    public PackedScene HpDelimeter
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
        if (HpDelimeters == null)
        {
            return;
        }

        ClearHpDelimeters();

        var barWidth = HpDelimeters.OffsetRight;
        var maxHp = Target!.Target.MaxHealth.TotalValue;

        var hpDelimeterCount = (int)(maxHp / HpPerHpDelimiter);
        var delimeterMargin = barWidth / hpDelimeterCount;

        HpDelimeters.OffsetLeft = delimeterMargin;
        HpDelimeters.AddThemeConstantOverride("separation", (int)delimeterMargin);

        GD.Print("BarWidth: " + barWidth);
        GD.Print("MaxHp: " + maxHp);
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
        UpdateHpDelimeters();
    }

    private void ClearSubscription(NedaoProxy? nedaoProxy)
    {
        if(nedaoProxy == null)
        {
            return;
        }

        nedaoProxy.Target.MaxHealth.OnTotalValueChanged -= OnMaxHealthChanged;
    }

    private void Subscribe(NedaoProxy? nedaoProxy)
    {
        if (nedaoProxy == null)
        {
            return;
        }
        nedaoProxy.Target.MaxHealth.OnTotalValueChanged += OnMaxHealthChanged;
    }
}
