using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    private Planet _planet;
    private Editor _shapeEditor;
    private Editor _colourEditor;
    private Editor _objectGeneratorEditor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();

            if (check.changed)
            {
                _planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate Planet"))
        {
            _planet.GeneratePlanet();
        }

        if (GUILayout.Button("Generate Objects"))
        {
            //_planet.ClearGeneratedObjects();
            _planet.GenerateObjects();
        }

        DrawSettingsEditor(_planet.shapeSettings, _planet.OnShapeSettingsUpdated, ref _planet.shapeSettingsFoldout, ref _shapeEditor);
        DrawSettingsEditor(_planet.colourSettings, _planet.OnColourSettingUpdated, ref _planet.colourSettingsFoldout, ref _colourEditor);
        DrawSettingsEditor(_planet.objectGeneratorSettings, null, ref _planet.objGenSettingsFoldout, ref _objectGeneratorEditor);
    }

    void DrawSettingsEditor(UnityEngine.Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    if (check.changed)
                    {
                        if (onSettingsUpdated != null)
                        {
                            onSettingsUpdated();
                        }
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        _planet = (Planet)target;
    }
}
