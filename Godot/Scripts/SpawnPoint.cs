using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class SpawnPoint : Node2D
{
    private int _waveIndex = 0;
    private readonly List<DumpTask> _tasks = [];
    private SpawnPointWaveContext? _context;

    [Export]
    public SpawnInfo SpawnInfo
    {
        get; set;
    }

    public override void _Ready()
    {
        base._Ready();

        if (SpawnInfo is null)
        {
            GD.PrintErr("No SpawnInfo assigned");
            return;
        }


    }

    public override void _Process(double delta)
    {
        for (var i = 0; i < _tasks.Count; i++)
        {
            var task = _tasks[i];
            task.Delay -= delta;

            if (task.Delay <= 0)
            {
                task.Action();
                _tasks.RemoveAt(i);
                i--;
            }

        }
    }

    private void FinishWave()
    {
        SpawnInfo.Points = _context!.WavePoints;
        _context = null;

        NextWave();

        _waveIndex++;
    }

    protected virtual void NextWave()
    {
        if(SpawnInfo is null)
        {
            GD.PrintErr("No SpawnInfo assigned");
            return;
        }

        if(_waveIndex >= SpawnInfo.Actions.Count)
        {
            GD.Print("Wave finished");
            return;
        }

        if (_context is null)
        {
            CreateContext();
        }

        var delay = Random.Shared.NextDouble(SpawnInfo.MinDelay, SpawnInfo.MaxDelay);
        _tasks.Add(new DumpTask(ExecuteWave, delay));
    }

    private void ExecuteWave()
    {
        if(_context is null)
        {
            GD.PrintErr("No context assigned");
            return;
        }

        var waveAction = SpawnInfo.Actions[_waveIndex];

        _context.Execute(waveAction);
    }

    [MemberNotNull(nameof(_context))]
    protected virtual WaveContext CreateContext()
    {
        return _context = new SpawnPointWaveContext(this)
        {
            WavePoints = SpawnInfo.Points
        };
    }

    private sealed class DumpTask(Action action, double delay)
    {
        public readonly Action Action = action;
        public double Delay = delay;
    }

    private sealed class SpawnPointWaveContext(SpawnPoint spawnPoint) : WaveContext
    {
        public event Action<SpawnPointWaveContext>? Finished;
        private readonly SpawnPoint _spawnPoint = spawnPoint;

        private readonly List<Enemy> _enemies = [];
        
        private bool _isReadOnly;
        private int _wavePoints;
        private WaveEndCondition _waveEndCondition;
        private double _waveTime;

        public bool IsReadOnly
        {
            get => _isReadOnly;
            set
            {
                if (_isReadOnly == true)
                {
                    throw new InvalidOperationException("IsReadOnly is read-only");
                }

                _isReadOnly = value;
            }
        }

        public override int WavePoints
        {
            get => _wavePoints;
            set
            {
                if (IsReadOnly)
                {
                    throw new InvalidOperationException("WavePoints is read-only");
                }

                _wavePoints = value;
            }
        }

        public override WaveEndCondition WaveEndCondition
        {
            get => _waveEndCondition;
            set
            {
                if (IsReadOnly)
                {
                    throw new InvalidOperationException("WaveEndCondition is read-only");
                }

                _waveEndCondition = value;
            }
        }

        public override double WaveTime
        {
            get => _waveTime;
            set
            {
                if (IsReadOnly)
                {
                    throw new InvalidOperationException("WaveTime is read-only");
                }

                _waveTime = value;
            }
        }

        public override Enemy CreateMob(MobRequest mobRequest)
        {
            if(IsReadOnly)
            {
                throw new InvalidOperationException("Cannot create mobs after the wave has started");
            }

            return CreateMob(mobRequest, 0);
        }

        public override Enemy CreateMob(MobRequest mobRequest, double delay)
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException("Cannot create mobs after the wave has started");
            }

            var mob = mobRequest.Instantiate();
            var task = new DumpTask(() => _spawnPoint.AddChild(mob), delay);
            _spawnPoint._tasks.Add(task);

            _enemies.Add(mob);

            return mob;
        }

        public override void FinishWave()
        {
            if(WaveEndCondition != WaveEndCondition.Custom)
            {
                throw new InvalidOperationException("Cannot finish wave manually when WaveEndCondition is not Custom");
            }

            _spawnPoint.FinishWave();
        }

        public override void Execute(WaveAction action)
        {
            action.Execute(this);

            IsReadOnly = true;

            if (WaveEndCondition == WaveEndCondition.AllDead)
            {
                // TODO
            }

            if (WaveEndCondition == WaveEndCondition.Time)
            {
                _spawnPoint._tasks.Add(new DumpTask(FinishWave, WaveTime));
            }
        }
    }
}
