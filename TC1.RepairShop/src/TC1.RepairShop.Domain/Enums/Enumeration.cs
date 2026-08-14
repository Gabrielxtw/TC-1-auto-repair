using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TC1.RepairShop.Domain.Enums;

public abstract class Enumeration<T> where T : Enumeration<T>
{
    public int Value { get; }
    public string Name { get; }

    protected Enumeration(int value, string name)
    {
        Value = value;
        Name = name;
    }

    public override string ToString() => Name;

    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration<T> other) return false;
        return GetType() == obj.GetType() && Value.Equals(other.Value);
    }

    public override int GetHashCode() => Value.GetHashCode();

    public static IEnumerable<T> GetAll()
    {
        var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
        return fields.Select(f => f.GetValue(null)).OfType<T>();
    }

    public static T FromValue(int value)
    {
        var match = GetAll().FirstOrDefault(item => item.Value == value);
        if (match is null)
            throw new InvalidOperationException($"'{value}' is not a valid value for {typeof(T)}");
        return match;
    }

    public static T FromName(string name)
    {
        var match = GetAll().FirstOrDefault(item => string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase));
        if (match is null)
            throw new InvalidOperationException($"'{name}' is not a valid name for {typeof(T)}");
        return match;
    }
}
