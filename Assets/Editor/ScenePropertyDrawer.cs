using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(SerializedSceneAttribute))]
public class ScenePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Debug.Log("awoo");
        if (property.propertyType != SerializedPropertyType.String)
        {
            Debug.LogError("Type do atributo SerializedScene deve ser string");
            return;
        }
        
        var oldSceneAsset = GetSceneAsset(property.stringValue);
        var sceneAsset = EditorGUI.ObjectField(position, label, oldSceneAsset, typeof(SceneAsset), false) as SceneAsset;
        if (oldSceneAsset == sceneAsset || sceneAsset == null)
        {
            return;
        }

        if (!EnsureSceneIsConfigured(sceneAsset))
        {
            return;
        }
        property.stringValue = sceneAsset.name;
    }

    private bool EnsureSceneIsConfigured(SceneAsset sceneAsset)
    {
        if (GetSceneAsset(sceneAsset.name) != null) //Já foi configurado
        {
            return true;
        }
        
        bool autoAdd = EditorUtility.DisplayDialog(
            "Cena não configurada",
            "Para ser usada no jogo, a cena precisa antes ser adicionada nas BuildSettings do projeto. Deseja adicionar essa cena automaticamente?",
            "Sim", "Não"
        );
        if (!autoAdd)
        {
            return false;
        }

        var newScenes = new EditorBuildSettingsScene[EditorBuildSettings.scenes.Length + 1];
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++) {
            newScenes[i] = EditorBuildSettings.scenes[i];
        }
        newScenes[^1] = new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(sceneAsset), true);

        EditorBuildSettings.scenes = newScenes;
        return true;
    }

    private static SceneAsset GetSceneAsset(string named)
    {
        if (string.IsNullOrEmpty(named))
        {
            return null;
        }

        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (scene.path.Contains(named))
            {
                return AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
            }
        }
        return null;
    }
}
