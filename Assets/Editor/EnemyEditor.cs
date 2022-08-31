using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enemy), true)]
public class EnemyEditor : Editor
{
    private const string ActivationRangeFieldName = "activationRange";
        
    private void OnSceneGUI()
    {
        var enemy = target as Enemy;
        if (enemy == null)
        {
            return;
        }
        
        DrawVisionRange(enemy);
    }

    private void DrawVisionRange(Enemy enemy)
    {
        var rangeProperty = serializedObject.FindProperty(ActivationRangeFieldName);
        if (rangeProperty == null)
        {
            Debug.Log("Nao achei a propriedade de range de ativação no inimigo!");
            return;
        }
        
        float range = rangeProperty.floatValue;
        Handles.color = enemy.Alerted ? Color.red : Color.yellow;
        Handles.DrawWireDisc(enemy.transform.position, Vector3.up, range);
        Handles.color = Color.white;
    }
}