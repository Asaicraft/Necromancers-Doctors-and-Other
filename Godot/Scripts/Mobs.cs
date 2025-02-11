using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Flags]
public enum Mobs : int
{
    /// <summary>
    /// Represents a player entity.
    /// </summary>
    Player = 1 << 0,

    /// <summary>
    /// Represents all enemy mobs.
    /// Used as a mask to identify enemies.
    /// </summary>
    Enemies = 1 << 1,

    /// <summary>
    /// Represents a necromancer enemy.
    /// </summary>
    Necromancer = 1 << 2,
    EnemyNecromancer = Enemies | Necromancer,

    /// <summary>
    /// Represents a doctor enemy.
    /// </summary>
    Doctor = 1 << 3,
    EnemyDoctor = Enemies | Doctor,

    /// <summary>
    /// Represents a skeleton enemy.
    /// </summary>
    Skeleton = 1 << 4,
    EnemySkeleton = Enemies | Skeleton,

    /// <summary>
    /// Represents all allied mobs.
    /// Used as a mask to identify allies.
    /// </summary>
    Allies = 1 << 10,

    /// <summary>
    /// Represents a tombstone ally.
    /// </summary>
    Tombstone = 1 << 11,
    AlliedTombstone = Allies | Tombstone,

    /// <summary>
    /// Represents a bum thinker ally.
    /// </summary>
    BumThinker = 1 << 12,
    AlliedBumThinker = Allies | BumThinker,
}
