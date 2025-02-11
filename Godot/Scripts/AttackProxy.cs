using Godot;
using NedaoObjects;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

public partial class AttackProxy : Area2D
{
    private readonly HashSet<NedaoProxy> _nedaoProxies = [];

    private CollisionShape2D? _collisionShape;
    private CircleShape2D? _shape;

    [Export]
    public NedaoProxy? Nedao
    {
        get; set;
    }

    [Export]
    public CollisionShape2D? CollisionShape
    {
        get => _collisionShape;
        set
        {
            if (_collisionShape is not null)
            {
                _collisionShape.Shape = null;
            }

            _collisionShape = value;

            if (_collisionShape is not null)
            {
                _collisionShape.Shape = CircleShape;
            }
        }
    }

    public CircleShape2D CircleShape
    {
        get => _shape ??= new();
    }

    public override void _Ready()
    {
        base._Ready();
        Monitoring = true;

        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Nedao is { } nedao)
        {
            CircleShape.Radius = nedao.Target.AttackRange;

            if(nedao.CanAttack)
            {
                TryAttack(nedao);
            }
        }
    }

    protected virtual void TryAttack(NedaoProxy subject)
    {
        var temp = ArrayPool<NedaoProxy>.Shared.Rent(_nedaoProxies.Count);
        _nedaoProxies.CopyTo(temp);

        try
        {
            for (var i = 0; i < _nedaoProxies.Count; i++)
            {
                var enemy = temp[i];

#if RELEASE
                subject.TryAttack(enemy);
#elif DEBUG
                if (subject.TryAttack(enemy))
                {
                    GD.Print($"{subject.Name} attack {enemy.Name}; Enemy health: {enemy.Target.Health}")
                }
#endif

            }
        }
        finally
        {
            ArrayPool<NedaoProxy>.Shared.Return(temp);
        }
        
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is NedaoProxy nedao)
        {
            if(nedao == Nedao)
            {
                // Ignore self
                return;
            }

            if(IsEnemy(nedao))
            {
                _nedaoProxies.Add(nedao);

                nedao.TreeExited += () => _nedaoProxies.Remove(nedao);

                GD.Print("Attack");
            }
        }
    }

    private void OnBodyExited(Node2D body)
    {
        if (body is NedaoProxy nedao)
        {
            if (nedao == Nedao)
            {
                // Ignore self
                return;
            }

            _nedaoProxies.Remove(nedao);
        }
    }

    protected virtual bool IsEnemy(NedaoProxy subject)
    {
        return Nedao is not null && IsEnemy(Nedao, subject);
    }

    public static bool IsEnemy(NedaoProxy attacker, NedaoProxy defender)
    {
        return (attacker.Enemies & defender.Team) != 0;
    }

}
