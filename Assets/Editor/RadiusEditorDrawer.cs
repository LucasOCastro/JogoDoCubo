using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonoBehaviour), true)]
public class RadiusEditorDrawer : Editor
{
    private IEnumerable<Type> TypeAndParentTypes(Type type)
    {
        while (type != null && type != typeof(UnityEngine.Object))
        {
            yield return type;
            type = type.BaseType;
        }
    }

    private void DrawFloat(float radius, Color color)
    {
        Vector3 pos = (target as MonoBehaviour).transform.position;
        Handles.color = color;
        Handles.DrawWireDisc(pos, Vector3.up, radius);
        Handles.color = Color.white;
    }

    private void DrawRange(FloatRange range, Color color)
    {
        DrawFloat(range.Min, color);
        DrawFloat(range.Max, color);
    }


    private void OnSceneGUI()
    {
        Type objType = target.GetType();
        foreach (var type in TypeAndParentTypes(objType))
        {
            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var attributes = field.GetCustomAttributes(typeof(RadiusDrawAttribute), true);
                if (attributes.Length == 0) continue;

                var attribute = attributes[0] as RadiusDrawAttribute;
                Type fieldType = field.FieldType;
                if (fieldType == typeof(float) || fieldType == typeof(int) || fieldType == typeof(double) || fieldType == typeof(decimal))
                {
                    DrawFloat((float)field.GetValue(target), attribute.Color);
                }
                else if (fieldType == typeof(FloatRange))
                {
                    DrawRange((FloatRange) field.GetValue(target), attribute.Color);
                }
            }
        }
    }
}
