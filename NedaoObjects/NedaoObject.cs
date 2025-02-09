using NedaoObjects.Effects;
using NedaoObjects.Gaints;
using System.Collections.Frozen;
using System.ComponentModel;
using System.Reflection.Emit;

namespace NedaoObjects;

public abstract partial class NedaoObject
{
    public const float AbsoluteMaxHelth = 2000;
    public const int MaxLevel = 25;
    public const float ArmorFormulaBase = 1;
    public const float ArmorFormulaFactor = 0.06f;

    public static readonly FrozenDictionary<int, int> ExpToLevels;

    static NedaoObject()
    {
        var expToLevels = new Dictionary<int, int>()
        {
            { 1, 0 },
            { 2, 6 },
            { 3, 16 },
            { 4, 29 },
            { 5, 44 },
            { 6, 61 },
            { 7, 80 },
            { 8, 100 },
            { 9, 122 },
            { 10, 147 },
            { 11, 175 },
            { 12, 205 },
            { 13, 237 },
            { 14, 272 },
            { 15, 310 },
            { 16, 350 },
            { 17, 392 },
            { 18, 437 },
            { 19, 485 },
            { 20, 535 },
            { 21, 590 },
            { 22, 650 },
            { 23, 715 },
            { 24, 785 },
            { 25, 860 }
        };

        ExpToLevels = expToLevels.ToFrozenDictionary();
    }

    public readonly EffectsCollection Effects;
    public readonly AttackBehavior AttackBehavior;
    public readonly GainModifier GainModifier;

    private bool _isDie = false;
    private int _exp = 0;
    private int _level = 1;

    private float _helth = 100;

    public NedaoObject()
    {
        Effects = CreateEffectsCollection();
        AttackBehavior = CreateAttackBehavior();
        GainModifier = CreateGainModifier();
    }

    public bool IsDie
    {
        get => _isDie;
        protected set
        {
            if(_isDie == value)
            {
                return;
            }

            _isDie = value;
            SetDie(_isDie);
        }
    }

    public int Exp
    {
        get => _exp;
        protected set
        {
            _exp = value;

            if (Level >= MaxLevel)
            {
                return;
            }

            if (ExpToLevels.TryGetValue(Level + 1, out var expToNextLevel) && _exp >= expToNextLevel)
            {
                Level++;
            }
        }
    }

    public int Level
    {
        get => _level;
        set
        {
            var prevLevel = _level;

            _level = value;

            if (_level > MaxLevel)
            {
                _level = MaxLevel;
            }

            var diffLevel = _level - prevLevel;

            if (diffLevel == 0)
            {
                return;
            }

            SetLevel(_level, prevLevel);
        }
    }

    public float Helth
    {
        get => _helth;
        set
        {
            _helth = value;
        }
    }

    public readonly NedaoProperty<float> MaxHealth = [];

    public readonly NedaoProperty<float> Damage = [];

    public readonly NedaoProperty<float> Armor = [];

    public readonly NedaoProperty<float> Speed = [];

    public readonly NedaoProperty<float> AttackSpeed = [];

    public readonly NedaoProperty<float> AttackRange = [];

    public readonly NedaoProperty<float> BaseAttackTime = [];

    public virtual void TakeExp(int exp)
    {
        Exp += exp;
    }

    public virtual void Die()
    {
        IsDie = true;

        Effects.Clear();
    }

    public virtual void TakeDamage(Damage damage)
    {
        Helth -= CalculateDamage(damage);
    }

    protected virtual void SetDie(bool isDie)
    {
    }

    protected virtual void SetLevel(int level, int prevLevel)
    {
        GainModifier.ApplyGain(this, level - prevLevel);
    }

    protected virtual EffectsCollection CreateEffectsCollection()
    {
        return new(this);
    }

    protected virtual AttackBehavior CreateAttackBehavior()
    {
        return new(this);
    }

    protected virtual GainModifier CreateGainModifier()
    {
        return new SimpleGainModifier();
    }

    protected virtual int CalculateDamage(Damage damage)
    {
        var damageFactor = 1 - CalculateDamageResist(damage);

        return (int)(damage.Value * damageFactor);
    }

    protected virtual double CalculateDamageResist(Damage damage)
    {
        if (damage.DamageType != DamageType.Physical)
        {
            return 0;
        }

        if (Armor == 0)
        {
            return 0;
        }

        return (ArmorFormulaFactor * Armor) / (ArmorFormulaBase + ArmorFormulaFactor * Math.Abs(Armor));
    }

    protected virtual float CalculateAttackSpeed()
    {
        return ((100+AttackSpeed) / (100*BaseAttackTime));
    }

    protected static void RescalePercentage(ref double current, double max, double newMax)
    {
        current = (current / max) * newMax;
    }
}
