using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class SelectObjectWithType : ScriptableWizard
{
    [SerializeField]
    private Object _selectObject = null;

    [MenuItem("Tools/Select All Of Type...")]
    public static void SelectAllOfTagWizard()
    {
        DisplayWizard<SelectObjectWithType>("Select All Of Type...", "Make Selection");
    }

    private void OnWizardCreate()
    {        
        Selection.objects =
            FindObjectsOfType(_selectObject.GetType())
                .Select(selectObject => (Object)(selectObject as Component).gameObject)
                .ToArray();
    }
}