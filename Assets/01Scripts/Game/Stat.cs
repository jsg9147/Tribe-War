using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier
{
    /// <summary>
    /// The constant value to identify a permanent modifier.
    /// </summary>
    private const int PERMANENT = 0;

    /// <summary>
    /// The value of this modifier.
    /// </summary>
    public int value;

    /// <summary>
    /// The duration in turn of this modifier.
    /// </summary>
    public int duration;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="value">The value of the modifier.</param>
    /// <param name="duration">The duration of the modifier.</param>
    public Modifier(int value, int duration = PERMANENT)
    {
        this.value = value;
        this.duration = duration;
    }

    /// <summary>
    /// Returns true if this modifier is permanent and false otherwise.
    /// </summary>
    /// <returns>True if this modifier is permanent; false otherwise.</returns>
    public bool IsPermanent()
    {
        return duration == PERMANENT;
    }
}

public class Stat
{
    public int statID;
    public string name;

    private int _baseValue;
    public int originalValue;

    public int minValue;
    public int maxValue;

    public List<Modifier> modifiers = new List<Modifier>();

    public Action<int, int> onValueChanged;

    public int baseValue
    {
        get
        {
            return _baseValue;
        }
        set
        {
            int oldValue = _baseValue;
            _baseValue = value;
            if (onValueChanged != null && oldValue != _baseValue)
            {
                onValueChanged(oldValue, value);
            }
        }
    }

    public int effectiveValue
    {
        get
        {
            // Start with the base value.
            var value = baseValue;
            // Apply all the modifiers.
            foreach (var modifier in modifiers)
            {

                value += modifier.value;
            }

            // Clamp to [minValue, maxValue] if needed.
            if (value < minValue)
            {
                value = minValue;
            }
            else if (value > maxValue)
            {
                value = maxValue;
            }

            // Return the effective value.
            return value;
        }
    }

    /// <summary>
    /// Adds a modifier to this stat.
    /// </summary>
    /// <param name="modifier">The modifier to add.</param>
    public void AddModifier(Modifier modifier)
    {
        int oldValue = effectiveValue;
        modifiers.Add(modifier);
        if (onValueChanged != null)
        {
            onValueChanged(oldValue, effectiveValue);
        }
    }

    public void AllRemoveModifier()
    {
        int oldValue = effectiveValue;
        modifiers.Clear();

        if (onValueChanged != null)
        {
            onValueChanged(oldValue, originalValue);
        }
    }

    /// <summary>
    /// This method is automatically called when the turn ends.
    /// </summary>
    public void OnEndTurn()
    {
        int oldValue = effectiveValue;
        List<Modifier> modifiersToRemove = new List<Modifier>(modifiers.Count);

        var temporaryModifiers = modifiers.FindAll(x => !x.IsPermanent());
        foreach (var modifier in temporaryModifiers)
        {
            modifier.duration -= 1;
            if (modifier.duration <= 0)
            {
                modifiersToRemove.Add(modifier);
            }
        }
        foreach (var modifier in modifiersToRemove)
        {
            modifiers.Remove(modifier);
        }
        if (modifiersToRemove.Count > 0 && onValueChanged != null)
        {
            onValueChanged(oldValue, effectiveValue);
        }
    }
}
