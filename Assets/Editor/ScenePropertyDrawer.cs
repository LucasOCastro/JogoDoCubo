using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(SerializedSceneAttribute))]
public class ScenePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.String:
                DrawForString(position, property, label);
                break;
            case SerializedPropertyType.Integer:
                DrawForInt(position, property, label);
                break;
            default:
                Debug.LogError("Type do atributo SerializedScene é inválido.");
                break;
        }
    }

    private void DrawForString(Rect position, SerializedProperty property, GUIContent label)
    {
        var oldSceneAsset = GetSceneAssetNamed(property.stringValue, out _);
        var sceneAsset = EditorGUI.ObjectField(position, label, oldSceneAsset, typeof(SceneAsset), false) as SceneAsset;
        if (oldSceneAsset == sceneAsset) {
            return;
        }

        if (sceneAsset == null) {
            property.stringValue = "";
            return;
        }

        if (EnsureSceneIsConfigured(sceneAsset, out _))
        {
            property.stringValue = sceneAsset.name;
        }
    }

    private void DrawForInt(Rect position, SerializedProperty property, GUIContent label)
    {
        var oldSceneAsset = GetSceneAssetAtIndex(property.intValue);
        var sceneAsset = EditorGUI.ObjectField(position, label, oldSceneAsset, typeof(SceneAsset), false) as SceneAsset;
        if (oldSceneAsset == sceneAsset) {
            return;
        }

        if (sceneAsset == null) {
            property.intValue = -1;
            return;
        }

        if (EnsureSceneIsConfigured(sceneAsset, out int buildSceneIndex)) 
        {
            property.intValue = buildSceneIndex;;
        }
    }

    private bool EnsureSceneIsConfigured(SceneAsset sceneAsset, out int buildSceneIndex)
    {
        if (GetSceneAssetNamed(sceneAsset.name, out buildSceneIndex) != null) //Já foi configurado
        {
            return true;
        }
        
        bool autoAdd = EditorUtility.DisplayDialog(
            "Cena não configurada",
            $"Para ser usada no jogo, a cena precisa antes ser adicionada nas BuildSettings do projeto. Deseja adicionar a cena \"{sceneAsset.name}.unity\" automaticamente?",
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
        buildSceneIndex = newScenes.Length - 1;

        EditorBuildSettings.scenes = newScenes;
        return true;
    }

    private static SceneAsset GetSceneAssetAtIndex(int buildIndex)
    {
        if (buildIndex < 0 || buildIndex >= EditorBuildSettings.scenes.Length)
        {
            return null;
        }
        return AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[buildIndex].path);
    }

    private static SceneAsset GetSceneAssetNamed(string named, out int buildIndex)
    {
        if (string.IsNullOrEmpty(named))
        {
            buildIndex = -1;
            return null;
        }

        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            var scene = EditorBuildSettings.scenes[i];
            if (scene.path.Replace(".unity", "").EndsWith(named))
            {
                buildIndex = i;
                return AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
            }
        }
        buildIndex = -1;
        return null;
    }
}
