using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NedaoObjects;

/// <summary>
/// Represents the attack behavior of a <see cref="NedaoObject"/>.
/// This class is designed to be extended for custom attack logic.
/// </summary>
/// <param name="associatedObject">The associatedObject of the attack behavior.</param>
public class AttackBehavior(NedaoObject associatedObject)
{
    /// <summary>
    /// The object that owns this attack behavior.
    /// </summary>
    protected readonly NedaoObject Owner = associatedObject;

    /// <summary>
    /// Gets or sets the cooldown time before the next attack can be performed.
    /// </summary>
    public virtual double Cooldown { get; set; }

    /// <summary>
    /// Updates the attack behavior, reducing the cooldown over time.
    /// Can be overridden in a derived class to modify update logic.
    /// </summary>
    /// <param name="delta">The time step for the update.</param>
    public virtual void Update(double delta)
    {
        if (Cooldown > 0)
        {
            Cooldown -= delta;
        }
    }

    /// <summary>
    /// Performs an attack on the specified target.
    /// If the attack is on cooldown, it will not be executed.
    /// Can be overridden in a derived class to customize attack behavior.
    /// </summary>
    /// <param name="target">The target of the attack.</param>
    public virtual void Attack(NedaoObject target)
    {
        if (Cooldown > 0)
        {
            return;
        }



        var damage = CreateDamage(target);
        target.TakeDamage(damage);
    }

    /// <summary>
    /// Creates a <see cref="Damage"/> instance for the attack.
    /// By default, it inflicts physical damage based on the associatedObject's current damage value.
    /// Can be overridden in a derived class to modify damage calculation.
    /// </summary>
    /// <param name="target">The target receiving the damage.</param>
    /// <returns>A new <see cref="Damage"/> instance.</returns>
    public virtual Damage CreateDamage(NedaoObject target)
    {
        return new Damage(Owner, Owner.Damage, DamageType.Physical);
    }
}
