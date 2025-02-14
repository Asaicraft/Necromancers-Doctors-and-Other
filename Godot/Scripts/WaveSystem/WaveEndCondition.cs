using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum WaveEndCondition
{
    /// <summary>
    /// The wave finishes when the specified time has elapsed.
    /// </summary>
    Time,

    /// <summary>
    /// The wave finishes when all enemies in the wave are dead.
    /// </summary>
    AllDead,

    /// <summary>
    /// The wave finishes when <see cref="WaveContext.FinishWave"/> is called manually.
    /// </summary>
    Custom
}
