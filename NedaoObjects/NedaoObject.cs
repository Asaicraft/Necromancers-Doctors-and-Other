using NedaoObjects.Effects;
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

    private bool _isDie = false;
    private int _exp = 0;
    private int _level = 1;

    private float _helth = 100;
    private float _maxHelth = 100;
    private float _damage = 10;
    private float _armor = 0;
    private float _speed = 100;
    private float _attackSpeed = 100;
    private float _attackRange = 150;
    private float _baseAttackTime = 1.7f;

    private float _currentHelth;
    private float _currentMaxHelth;
    private float _currentDamage;
    private float _currentArmor;
    private float _currentSpeed;
    private float _currentAttackSpeed;
    private float _currentAttackRange;
    private float _currentBaseAttackTime;

    public NedaoObject()
    {
        Effects = CreateEffectsCollection();
        AttackBehavior = CreateAttackBehavior();
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
            
            SetHelth(_helth);
        }
    }

    public float MaxHelth
    {
        get => _maxHelth;
        set
        {
            _maxHelth = value;

            if(_maxHelth < 0)
            {
                _maxHelth = 0;
            }

            if (_maxHelth > AbsoluteMaxHelth)
            {
                _maxHelth = AbsoluteMaxHelth;
            }

            SetMaxHelth(_maxHelth);
        }
    }

    public float Damage
    {
        get => _damage;
        set
        {
            _damage = value;

            SetDamage(_damage);
        }
    }

    public float Armor
    {
        get => _armor;
        set
        {
            _armor = value;

            SetArmor(_armor);
        }
    }

    public float Speed
    {
        get => _speed;
        set
        {
            _speed = value;

            if(_speed < 0)
            {
                _speed = 0;
            }

            SetSpeed(_speed);
        }
    }

    public float AttackSpeed
    {
        get => _attackSpeed;
        set
        {
            _attackSpeed = value;

            if(_attackSpeed < 0)
            {
                _attackSpeed = 0;
            }

            SetAttackSpeed(_attackSpeed);
        }
    }

    public float AttackRange
    {
        get => _attackRange;
        set
        {
            _attackRange = value;

            if (_attackRange < 0)
            {
                _attackRange = 0;
            }

            SetAttackRange(_attackRange);
        }
    }

    public float BaseAttackTime
    {
        get => _baseAttackTime;
        set
        {
            _baseAttackTime = value;

            if (_baseAttackTime < 0)
            {
                _baseAttackTime = 0;
            }

        }
    }

    public float CurrentHelth
    {
        get => _currentHelth;
        set
        {
            _currentHelth = value;

            SetCurrentHelth(_currentHelth);
        }
    }

    public float CurrentMaxHelth
    {
        get => _currentMaxHelth;
        set
        {
            _currentMaxHelth = value;

            SetCurrentMaxHelth(_currentMaxHelth);
        }
    }

    public float CurrentDamage
    {
        get => _currentDamage;
        set
        {
            _currentDamage = value;

            SetCurrentDamage(_currentDamage);
        }
    }

    public float CurrentArmor
    {
        get => _currentArmor;
        set
        {
            _currentArmor = value;

            SetCurrentArmor(_currentArmor);
        }
    }

    public float CurrentSpeed
    {
        get => _currentSpeed;
        set
        {
            _currentSpeed = value;

            SetCurrentSpeed(_currentSpeed);
        }
    }

    public float CurrentAttackSpeed
    {
        get => _currentAttackSpeed;
        set
        {
            _currentAttackSpeed = value;

            SetCurrentAttackSpeed(_currentAttackSpeed);
        }
    }

    public float CurrentAttackRange
    {
        get => _currentAttackRange;
        set
        {
            _currentAttackRange = value;
            if (_currentAttackRange < 0)
            {
                _currentAttackRange = 0;
            }

            SetCurrentAttackRange(_currentAttackRange);
        }
    }

    public float CurrentBaseAttackTime
    {
        get => _currentBaseAttackTime;
        set
        {
            _currentBaseAttackTime = value;
            
            if (_currentBaseAttackTime < 0)
            {
                _currentBaseAttackTime = 0;
            }

            SetCurrentBaseAttackTime(_currentBaseAttackTime);
        }
    }

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
    }

    protected virtual void SetHelth(float helth)
    {
        CurrentHelth = helth;
    }

    protected virtual void SetMaxHelth(float maxHelth)
    {
        CurrentMaxHelth = maxHelth;
    }

    protected virtual void SetDamage(float damage)
    {
        CurrentDamage = damage;
    }

    protected virtual void SetArmor(float armor)
    {
        CurrentArmor = armor;
    }

    protected virtual void SetSpeed(float speed)
    {
        CurrentSpeed = speed;
    }

    protected virtual void SetAttackSpeed(float attackSpeed)
    {
        CurrentAttackSpeed = attackSpeed;
    }

    protected virtual void SetAttackRange(float attackRange)
    {
        CurrentAttackRange = attackRange;
    }

    protected virtual void SetBaseAttackTime(float baseAttackTime)
    {
        CurrentBaseAttackTime = baseAttackTime;
    }

    protected virtual void SetCurrentHelth(float currentHelth)
    {
    }

    protected virtual void SetCurrentMaxHelth(float currentMaxHelth)
    {
    }

    protected virtual void SetCurrentDamage(float currentDamage)
    {
    }

    protected virtual void SetCurrentArmor(float currentArmor)
    {
    }

    protected virtual void SetCurrentSpeed(float currentSpeed)
    {
        
    }

    protected virtual void SetCurrentAttackSpeed(float currentAttackSpeed)
    {
    }

    protected virtual void SetCurrentAttackRange(float currentAttackRange)
    {
    }

    protected virtual void SetCurrentBaseAttackTime(float currentBaseAttackTime)
    {
    }

    protected virtual EffectsCollection CreateEffectsCollection()
    {
        return new(this);
    }

    protected virtual AttackBehavior CreateAttackBehavior()
    {
        return new(this);
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
        return ((100+CurrentAttackSpeed) / 100);
    }

    protected static void RescalePercentage(ref double current, double max, double newMax)
    {
        current = (current / max) * newMax;
    }
}
