using Godot;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class StatusBar : Node2D
{
    [Export]
    public NedaoProxy Target
    {
        get; set;
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

    public override void _Process(double delta)
    {
        if (Target == null)
        {
            return;
        }
        UpdateHpDelimeters();
    }

    private void UpdateHpDelimeters()
    {
        if (HpDelimeters == null)
        {
            return;
        }

        ClearHpDelimeters();

        var barWidth = HpDelimeters.OffsetRight;
        var maxHp = Target.Target.MaxHealth.TotalValue;

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
}
