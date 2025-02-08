using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NedaoObjects;

/// <summary>
/// Represents different types of damage that can be dealt to a target.
/// This enum is designed to be used as flags.
/// </summary>
[Flags]
public enum DamageType : byte
{
    /// <summary>
    /// Represents physical damage.
    /// </summary>
    Physical = 1 << 1,

    /// <summary>
    /// Represents magical damage.
    /// </summary>
    Magical = 1 << 2,

    /// <summary>
    /// Represents pure (unmitigated) damage.
    /// </summary>
    Pure = 1 << 3,

    /// <summary>
    /// Unused values are not reserved, allowing custom damage types to be defined.
    /// Example: <code>public static readonly DamageType AnotherDamageType = (DamageType)(1 << 4);</code>
    /// </summary>
}