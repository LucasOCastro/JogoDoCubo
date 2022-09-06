using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AreaEnemyAttacker))]
public class AreaEnemyAttackerEditor : Editor
{
    private const string DistanceFieldName = "distance";
    private const string AngleFieldName = "angle";

    private void OnSceneGUI()
    {
        var attacker = target as AreaEnemyAttacker;
        if (attacker == null)
        {
            return;
        }
        
        DrawAttackArc(attacker);
    }

    private void DrawAttackArc(EnemyAttacker attacker)
    {
        var distanceProperty = serializedObject.FindProperty(DistanceFieldName);
        var angleProperty = serializedObject.FindProperty(AngleFieldName);
        if (distanceProperty == null || angleProperty == null)
        {
            Debug.Log("Nao achei a propriedade de distância de ataque / ângulo de ataque no inimigo!", attacker);
            return;
        }

        float distance = distanceProperty.floatValue;
        float angle = angleProperty.floatValue;

        var transform = attacker.transform;
        Vector3 position = transform.position;
        Vector3 forward = transform.forward;
        Vector3 startVec = ArcDirection(-angle, forward);
        Vector3 endVec = ArcDirection(angle, forward);
        
        Handles.color = Color.white;
        Handles.DrawLine(position, position + startVec * distance);
        Handles.DrawLine(position, position + endVec * distance);
        Handles.DrawWireArc(position, Vector3.up, startVec, angle, distance);
        Handles.color = Color.white;
        
    }

    private static Vector3 ArcDirection(float angle, Vector3 forward)
    {
        float forwardAngle = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
        float totalAngle = forwardAngle + angle*.5f;
        float rad = totalAngle * Mathf.Deg2Rad;
        float x = Mathf.Sin(rad);
        float z = Mathf.Cos(rad);
        return new Vector3(x, 0, z);
    }
}

