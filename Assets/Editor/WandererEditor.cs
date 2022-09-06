using UnityEditor;
using UnityEngine;
using System.Reflection;

[CustomEditor(typeof(WanderBehavior), true)]
public class WandererEditor : Editor
{
    private const string MaxDistanceFieldName = "maxDistanceFromOrigin";
    
    //A posição inicial não é serializada, então devo acessar por Reflection
    private const string StartPositionFieldName = "_startPosition";

    private static FieldInfo StartPositionField =
        typeof(WanderBehavior).GetField(StartPositionFieldName, BindingFlags.Instance | BindingFlags.NonPublic);
    
    private void OnSceneGUI()
    {
        var wander = target as WanderBehavior;
        if (wander == null)
        {
            return;
        }
        
        DrawVisionRange(wander);
    }

    private Vector3 GetStartPosition(WanderBehavior wander)
    {
        if (!Application.isPlaying)
        {
            return wander.transform.position;
        }

        if (StartPositionField == null)
        {
            Debug.Log("Nao achei o campo de posição inicial no wanderer!", wander);
            return Vector3.zero;
        }

        return (Vector3) StartPositionField.GetValue(wander);
    } 
    
    private void DrawVisionRange(WanderBehavior wander)
    {
        var distanceProperty = serializedObject.FindProperty(MaxDistanceFieldName);
        if (distanceProperty == null)
        {
            Debug.Log("Nao achei a propriedade de distância máxima no wanderer!", wander);
            return;
        }

        
        float distance = distanceProperty.floatValue;
        Handles.color = Color.white;
        Handles.DrawWireDisc(GetStartPosition(wander), Vector3.up, distance);
        Handles.color = Color.white;
    }
}