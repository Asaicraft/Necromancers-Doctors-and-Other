using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// This class is intended for single-use only within <see cref="WaveAction.Execute(WaveContext)"/>.
/// Do not store instances of this class in fields or reuse them.
/// </summary>
public abstract class WaveContext
{
    public abstract int WavePoints
    {
        get; set;
    }

    public abstract WaveEndCondition WaveEndCondition
    {
        get; set;
    }

    /// <summary>
    /// When <see cref="WaveEndCondition"/> is <see cref="WaveEndCondition.Time"/>, 
    /// this is the time the wave will last.
    /// </summary>
    public abstract double WaveTime
    {
        get; set;
    }

    public abstract Enemy CreateMob(MobRequest mobRequest);
    public abstract Enemy CreateMob(MobRequest mobRequest, double delay);

    /// <summary>
    /// Manually ends the current wave, regardless of the active <see cref="WaveEndCondition"/>.
    /// This method should be called only when <see cref="WaveEndCondition.Custom"/> is used.
    /// </summary>
    public abstract void FinishWave();

    public abstract void Execute(WaveAction action);
}
