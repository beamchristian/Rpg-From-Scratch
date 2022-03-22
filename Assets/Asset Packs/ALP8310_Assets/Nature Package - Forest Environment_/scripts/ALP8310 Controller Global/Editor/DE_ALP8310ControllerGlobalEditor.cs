/*
    11/06/2021
    support: David Olshefski - http://deenvironment.com/
    support: Aleksey Pakhalchyk - https://www.artstation.com/aleksey8310
*/

#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if SPACE_PIPELINE
using SineSpace.Scripting.Components;
#endif

[CustomEditor(typeof(ALP8310_ControllerGlobal))]
[CanEditMultipleObjects]
public class ALP8310ControllerGlobalEditor : Editor
{
    #region [Properties]

    /// <summary>
    /// Publisher Global Controller
    /// </summary>
    private ALP8310_ControllerGlobal controller;

    private static int currentToolBoxIndex = 0;

    /// <summary>
    /// Wind Properties
    /// </summary>
    private SerializedProperty windZone,
        SynchWindZone,
        WindStrength,
        WindPulse,
        WindDirection,
        WindTurbulence,
        WindRandomness;

    /// <summary>
    /// Billboard Properties
    /// </summary>
    private SerializedProperty BillboardEnabled, BillboardIntensity;

    /// <summary>
    /// Global Wetness
    /// </summary>
    private SerializedProperty WetnessEnabled, WetnessIntensity;

    private SerializedProperty SavedTab;
#if VEGETATION_STUDIO_PRO
/// <summary>
/// Vegetation Studio Pro
/// </summary>
    SerializedProperty WindWavesTexture, WindWavesSize, WindSpeedFactor;
    /// <summary>
    /// Vegetation Studio Pro Icon
    /// </summary>
    Texture2D VSPlogo;
#endif
#if SPACE_PIPELINE
/// <summary>
/// SineSpace Scripting Runtime
/// </summary>
    SerializedProperty scriptingRuntime;
    /// <summary>
    /// scripting Runtime Button Content
    /// </summary>
    string scriptingRuntimeButtonContent;
#endif

    /// <summary>
    /// GUI Button
    /// </summary>
    private string windZoneButtonContent;

    #endregion [Properties]

    #region [Unity Actions]

    private void OnEnable()
    {
        controller = (target as ALP8310_ControllerGlobal);

        windZone = serializedObject.FindProperty("windZone");
        SynchWindZone = serializedObject.FindProperty("SynchWindZone");
        WindStrength = serializedObject.FindProperty("WindStrength");
        WindPulse = serializedObject.FindProperty("WindPulse");
        WindTurbulence = serializedObject.FindProperty("WindTurbulence");
        WindRandomness = serializedObject.FindProperty("WindRandomness");
        WindDirection = serializedObject.FindProperty("WindDirection");

        BillboardEnabled = serializedObject.FindProperty("BillboardEnabled");
        BillboardIntensity = serializedObject.FindProperty("BillboardIntensity");

        WetnessEnabled = serializedObject.FindProperty("WetnessEnabled");
        WetnessIntensity = serializedObject.FindProperty("WetnessIntensity");

        controller.foldouts = new List<bool>()
        {
            true,
            false,
            false,
            false
        };
        controller.actions = new List<System.Action>()
        {
            WindGUI,
            WetnessGUI,
        };
        controller.guiContent = new List<GUIContent>()
        {
            new GUIContent("Wind", "Click here to open Wind settings"),
            new GUIContent("Wetness", "Click here to open Wetness settings"),
        };

#if VEGETATION_STUDIO_PRO
        WindWavesTexture = serializedObject.FindProperty("WindWavesTexture");
        WindWavesSize = serializedObject.FindProperty("WindWavesSize");
        WindSpeedFactor = serializedObject.FindProperty("WindSpeedFactor");
        VSPlogo = (Texture2D)Resources.Load("AWESOME_Vegetation_Studio_Pro_Editor", typeof(Texture2D));
        controller.foldouts.Add(false);
        controller.actions.Add(VSPGUI);
        controller.guiContent.Add((new GUIContent("Vegetation Studio Pro", "Click here to open Vegetation Studio Pro settings")));
#endif

#if SPACE_PIPELINE
        scriptingRuntime = serializedObject.FindProperty("scriptingRuntime");
        controller.foldouts.Add(false);
        controller.actions.Add(SineSpaceGUI);
        controller.guiContent.Add((new GUIContent("SineSpace", "Click here to open SineSpace settings")));
#endif
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        GeneralGUI();
        /*      for (int i = 0; i < controller.foldouts.Count; i++)
              {
                  if (i != controller.foldouts.Count - 1)
                      controller.foldouts[i] = controller.foldouts[i].FoldoutField(controller.guiContent[i]);
                  else
                      controller.foldouts[i] =
                          controller.foldouts[i].FoldoutField(controller.guiContent[i], isLastInRow: true);
                  if (controller.foldouts[i])
                      controller.actions[i]();
              }
              */

        serializedObject.ApplyModifiedProperties();

        if (EditorGUI.EndChangeCheck())
            controller.SetShaders();
    }

    #endregion [Unity Actions]

    #region [GUI]

    /// <summary>
    /// General GUI
    /// </summary>
    private void GeneralGUI()
    {
        currentToolBoxIndex = GUILayout.Toolbar(
            currentToolBoxIndex,
            new string[] { "Wind", "Environmental" }, null,
            GUI.ToolbarButtonSize.Fixed, GUILayout.Height(50));

        switch (currentToolBoxIndex)
        {
            case 0:
                EditorGUILayout.BeginVertical("Box");
                WindZoneGUI();
                EditorGUILayout.EndVertical();
                WindGUI();
                break;

            case 1:
                WetnessGUI();
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical("Box");
                break;

            default: break;
        }
    }

    /// <summary>
    /// Main Wind GUI
    /// </summary>
    private void WindGUI()
    {
        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Main Wind Setting", EditorStyles.boldLabel);
        EditorGUI.BeginDisabledGroup(controller.SynchWindZone && controller.windZone);
        WindStrength.PropertyField("Main", "wind main strength");
        WindTurbulence.PropertyField("Turbulence", "wind variation in wind direction");
        WindPulse.PropertyField("Pulse Frequency", "wind length & frequency of the wind pulses.y");
        EditorGUI.EndDisabledGroup();

        WindDirection.PropertyField("Direction", "wind zone transform rotation Y");
        WindRandomness.PropertyField("Random Offset", "wind randomness offset");
        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("Box");
        BillboardWindGUI();
        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// Main WindZone GUI
    /// </summary>
    private void WindZoneGUI()
    {
        EditorGUI.BeginDisabledGroup(!controller.windZone);
        SynchWindZone.PropertyField("Sync WindZone", "Synch <Global Controller> with Wind Zone");
        EditorGUI.EndDisabledGroup();
        windZone.PropertyField("Wind Zone", "Unity Wind Zone");
        if (!controller.windZone)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (controller.GetComponent<WindZone>())
                windZoneButtonContent = "Add attached Wind Zone";
            else
                windZoneButtonContent = "Add new Wind Zone";
            if (GUILayout.Button(windZoneButtonContent))
            {
                if (controller.GetComponent<WindZone>())
                    controller.windZone = controller.GetComponent<WindZone>();
                else
                    controller.windZone = controller.gameObject.AddComponent<WindZone>();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        EditorGUILayout.Space();
    }

    /// <summary>
    /// Billboard GUI
    /// </summary>
    private void BillboardWindGUI()
    {
        EditorGUILayout.LabelField("Mode Wind Billboard", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Enable", GUILayout.Width(Screen.width / 4));
        BillboardEnabled.intValue = EditorGUILayout.IntPopup(BillboardEnabled.intValue,
            new string[] { "Off", "Active" }, new[] { 0, 1 });

        EditorGUILayout.EndHorizontal();

        EditorGUI.BeginDisabledGroup(!controller.BillboardEnabled);
        BillboardIntensity.PropertyField("Main");
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.Space();
    }

    /// <summary>
    /// Wetness GUI
    /// </summary>
    private void WetnessGUI()
    {
        EditorGUILayout.LabelField("Wetness", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Enable", GUILayout.Width(Screen.width / 4));
        WetnessEnabled.intValue = EditorGUILayout.IntPopup(WetnessEnabled.intValue,
            new string[] { "Off", "Active" }, new[] { 0, 1 });

        EditorGUILayout.EndHorizontal();

        EditorGUI.BeginDisabledGroup(!controller.WetnessEnabled);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Intensity", GUILayout.Width(Screen.width / 4));
        WetnessIntensity.SliderField(0, 1, "Intensity", "");
        EditorGUILayout.EndHorizontal();
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.Space();
    }

#if VEGETATION_STUDIO_PRO
    private void VSPGUI()
    {
        EditorGUILayout.LabelField("Vegetation Studio Pro Settings", EditorStyles.boldLabel);
        VSPlogo.DisplayLogo();
        GUILayout.BeginVertical("box");
        EditorGUILayout.HelpBox("This wind module is used to control the wind for Vegetation Studio grass patches. You need this for scenes that does not have a VegetationSystem component.", MessageType.Info);
        GUILayout.EndVertical();
        GUILayout.BeginVertical("box");
        EditorGUILayout.LabelField(new GUIContent("Wind Wave Noise Texture2D"));
        EditorGUILayout.PropertyField(WindWavesTexture, GUIContent.none);
        GUILayout.EndVertical();
        EditorGUILayout.Space();
        EditorGUILayout.Slider(WindSpeedFactor, 0, 5, new GUIContent("Wind Speed Factor"));
        EditorGUILayout.Slider(WindWavesSize,0, 30, new GUIContent("Wind Wave Size"));
        EditorGUILayout.Space();
    }
#endif
#if SPACE_PIPELINE
    private void SineSpaceGUI()
    {
        EditorGUILayout.LabelField("SineSpace Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(scriptingRuntime);
        if (!controller.scriptingRuntime)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (controller.GetComponent<ScriptingRuntimeBaseInternal>())
                scriptingRuntimeButtonContent = "Add attached Scripting Runtime";
            else
                scriptingRuntimeButtonContent = "Add new Scripting Runtime";
            if (GUILayout.Button(scriptingRuntimeButtonContent))
            {
                if (controller.GetComponent<ScriptingRuntimeBaseInternal>())
                    controller.scriptingRuntime = controller.GetComponent<ScriptingRuntimeBaseInternal>();
                else
                    controller.scriptingRuntime = controller.gameObject.AddComponent<ScriptingRuntimeBaseInternal>();
                controller.scriptingRuntime.PublicVariablesLocked = true;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.Space();
    }
#endif

    #endregion [GUI]

    #region [Menu Itemm]

    /// <summary>
    /// Create Global ALP8310 Wind Controller
    /// </summary>
    [MenuItem("Window/ALP8310/Global Controller/Add Global Controller...", priority = 311)]
    public static void CreateGlobalALP8310WindController()
    {
        GameObject WindController = new GameObject("ALP8310 Controller Global");
        var egc = WindController.AddComponent<ALP8310_ControllerGlobal>();
        var wz = WindController.AddComponent<WindZone>();
        egc.windZone = wz;
        egc.SynchWindZone = true;

        if (SceneView.lastActiveSceneView != null)
        {
            WindController.transform.position = Vector3.zero;
        }
    }

    /// <summary>
    /// Create Global ALP8310 Wind Controller
    /// </summary>
    [MenuItem("GameObject/3D Object/ALP8310/Add Global Controller...", priority = 9999)]
    public static void CreateGlobalALP8310WindControllerGameObject()
    {
        GameObject WindController = new GameObject("ALP8310 Controller Global");
        var egc = WindController.AddComponent<ALP8310_ControllerGlobal>();
        var wz = WindController.AddComponent<WindZone>();
        egc.windZone = wz;
        egc.SynchWindZone = true;

        if (SceneView.lastActiveSceneView != null)
        {
            WindController.transform.position = Vector3.zero;
        }
    }

    #endregion [Menu Itemm]
}

#endif