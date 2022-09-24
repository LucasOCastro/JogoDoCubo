using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enemy), true)]
public class EnemyEditor : Editor
{
    private const string ActivationRangeFieldName = "activationRange";
    private const string ViewHeightOffsetFieldName = "viewHeightOffset";
        
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
        var viewOffsetProperty = serializedObject.FindProperty(ViewHeightOffsetFieldName); 
        if (rangeProperty == null || viewOffsetProperty == null)
        {
            Debug.Log("Nao achei a propriedade de range de ativação ou view offset no inimigo!", enemy);
            return;
        }
        
        float range = rangeProperty.floatValue;
        float heightOffset = viewOffsetProperty.floatValue;
        Vector3 castStart = enemy.transform.position + Vector3.up * heightOffset;
        Handles.color = enemy.Alerted ? Color.red : Color.yellow;
        Handles.DrawWireDisc(enemy.transform.position, Vector3.up, range);
        Handles.DrawLine(castStart, castStart + enemy.transform.forward * range);
        Handles.color = Color.white;
    }
}