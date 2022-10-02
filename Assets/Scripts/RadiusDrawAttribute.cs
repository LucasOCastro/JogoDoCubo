using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class RadiusDrawAttribute : PropertyAttribute
{
    public Color Color { get; }

    public RadiusDrawAttribute()
    {
        Color = Color.white;
    }
    public RadiusDrawAttribute(float r, float g, float b)
    {
        Color = new Color(r,g,b);
    }
    public RadiusDrawAttribute(float r, float g, float b, float a)
    {
        Color = new Color(r,g,b,a);
    }
}