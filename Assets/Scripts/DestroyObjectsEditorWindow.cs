using System.Collections.Generic;
using UnityEditor;

public class DestroyObjectsEditorWindow : ScriptableWizard
{
    public List<UnityEngine.Object> objects;
    [MenuItem("Tools/Destroy Nested Objects")]

    static void CreateWizard()
        => DisplayWizard<DestroyObjectsEditorWindow>("Destroy Objects", "Destroy");

    void OnWizardCreate()
    {
        var message = "Are you sure you want to destroy those objects?";
        if (EditorUtility.DisplayDialog("Destroy Objects", message, "Yes", "No"))
            foreach (var obj in objects)
                DestroyImmediate(obj, true);
    }
}