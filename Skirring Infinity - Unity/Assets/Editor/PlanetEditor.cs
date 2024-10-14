using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    Planet planet;
    Editor shapeEditor;
    Editor colourEditor;

    // polymorphism in the play here:
    // the OnInspectorGUI is overridden into the custom class PlanetEditor
    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI(); // This line calls the default Inspector GUI implementation provided by Unity.
                                   // ensures the Inspector for the object behaves normally and shows standard fields for the component
            if (check.changed)
            {
                planet.GeneratePlanet();    // this replaces the onValidate function we made initially in the planet script so we can now remove that
            }
        }

        if (GUILayout.Button("Generate Planet"))
        {
            planet.GeneratePlanet();
        }

        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref planet.shapeSettingsFoldout, ref shapeEditor);
        DrawSettingsEditor(planet.colourSettings, planet.OnColourSettingsUpdated, ref planet.colourSettingsFoldout, ref colourEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor) // bool value stored in planet script as PlanetEditor script is serialized and loses its value
    {
        foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
        if (settings != null)
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {

                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();
                    if (check.changed)
                    {
                        if (onSettingsUpdated != null)
                            onSettingsUpdated();
                    }
                }
            }
        }
    }

    //OnEnable method is called when the editor (Inspector window) becomes active for the associated object
    private void OnEnable()
    {
        planet = (Planet)target; // typecasting target object into Planet object and binding it to the planet variable
    }
}
