/*
    11/06/2021
    support: David Olshefski - http://deenvironment.com/
    support: Aleksey Pakhalchyk - https://www.artstation.com/aleksey8310
*/

using DE_ALP8310;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DEC Environment Wind
/// </summary>
[ExecuteInEditMode, AddComponentMenu("ALP8310 Controller Global")]
public class ALP8310_ControllerGlobal : MonoBehaviour
{
    #region [General]

    /// <summary>
    ///Unity Wind Zone
    /// </summary>
    public WindZone windZone;

    /// <summary>
    /// Synch WindZone
    /// </summary>
    public bool SynchWindZone = false;

    /// <summary>
    /// Wind Strength
    /// </summary>
    public float WindStrength = 0.01f;

    public float WindDirection = 0f;

    /// <summary>
    /// Wind Pulse
    /// </summary>
    public float WindPulse = 10f;

    /// <summary>
    /// Wind Turbulence
    /// </summary>
    public float WindTurbulence = 0.01f;

    /// <summary>
    /// Wind Randomness
    /// </summary>
    public float WindRandomness = 0;

    #endregion [General]

    #region [Billboard]

    /// <summary>
    /// Billboard Wind
    /// </summary>
    public bool BillboardEnabled = false;

    /// <summary>
    /// Billboard Wind Influence
    /// </summary>
    public float BillboardIntensity = 0;

    #endregion [Billboard]

    #region [Wetness]

    /// <summary>
    /// Global Wetness Enabled
    /// </summary>
    public bool WetnessEnabled = false;

    /// <summary>
    /// Wetness Intensity
    /// </summary>
    public float WetnessIntensity = 0;

    #endregion [Wetness]

    #region [Vegetation Studio Pro]

#if VEGETATION_STUDIO_PRO
        public Texture WindWavesTexture;
        public float WindWavesSize = 10;
        public float WindSpeedFactor = 1;
#endif

    #endregion [Vegetation Studio Pro]

    #region [SineSpace]

#if SPACE_PIPELINE
        public ScriptingRuntimeBaseInternal scriptingRuntime;

        private PublicScriptVariable
            sWindStrength, sWindDirection, sWindPulse, sWindTurbulence, sRandomWind,
            sBillboardWind, sBillboardIntensity,
            sWetnessEnabled, sWetnessIntensity,

#endif

    #endregion [SineSpace]

    #region [GUI]

    /// <summary>
    /// GUI properties
    /// </summary>
    [HideInInspector] public List<bool> foldouts;

    [HideInInspector] public List<Action> actions;
    [HideInInspector] public List<GUIContent> guiContent;

    #endregion [GUI]

    #region [Private Variables]

    /// <summary>
    /// Call Back Values
    /// </summary>
    private float windStrength, windDirection, windPulse, windTurbulence;

    /// <summary>
    /// Global Wind Shader Properties
    /// </summary>
    private readonly string _WindStrength = "_Global_Wind_Main_Intensity", _WindDirection = "_Global_Wind_Main_Direction", _WindPulse = "_Global_Wind_Main_Pulse", _WindTurbulence = "_Global_Wind_Main_Turbulence", _RandomWind = "_Global_Wind_Main_RandomOffset";

    /// <summary>
    /// Global Wind Billboard Properties
    /// </summary>
    private readonly string _BillboardEnabled = "_Global_Wind_Billboard_Enabled", _BillboardIntensity = "_Global_Wind_Billboard_Intensity";

    /// <summary>
    /// Global Wetness Properties
    /// </summary>
    private readonly string _WetnessEnabled = "_Global_Wetness_Enabled", _WetnessIntensity = "_Global_Wetness_Intensity";

    #endregion [Private Variables]

    #region [UnityCalls]

    /// <summary>
    /// Resets Global Wind to no Movement when disabled
    /// </summary>
    private void OnDisable()
    {
        ResetShaders();
    }

    /// <summary>
    /// Resets Global Wind when component is removed
    /// </summary>
    private void OnDestroy()
    {
        ResetShaders();
    }

    /// <summary>
    /// Initialize Windzone when component is enabled
    /// </summary>
    private void OnEnable()
    {
        SetShaders();
    }

    /// <summary>
    /// Updates Windzone every Frame
    /// </summary>
    private void Update()
    {
        SetUpdateValues();
    }

    /// <summary>
    /// Reset Component
    /// </summary>
    private void Reset()
    {
        WindStrength = 5f;
        WindRandomness = 0.2f;
        WindPulse = 0.5f;
        WindTurbulence = 1f;
        WindDirection = 0;

        BillboardEnabled = true;
        BillboardIntensity = 0.5f;

        WetnessEnabled = false;
        WetnessIntensity = 1f;

        ResetVSPProperties();
        SetShaders();
    }

    #endregion [UnityCalls]

    #region [Public Voids]

    /// <summary>
    /// Set Updated Values
    /// </summary>
    public void SetUpdateValues()
    {
        GetDefaultValues();
        GetWindZoneValues();
    }

    /// <summary>
    /// Get Shader Values
    /// </summary>
    private void GetDefaultValues()
    {
        if (!SynchWindZone && (windStrength != _WindStrength.GetGlobalFloat() || transform.rotation.eulerAngles.y != _WindDirection.GetGlobalFloat() || windPulse != _WindPulse.GetGlobalFloat() || windTurbulence != _WindTurbulence.GetGlobalFloat() || windDirection != _WindDirection.GetGlobalFloat()))
        {
            SetShaders();
            windStrength = _WindStrength.GetGlobalFloat();
            windDirection = _WindDirection.GetGlobalFloat();
            windPulse = _WindPulse.GetGlobalFloat();
            windTurbulence = _WindTurbulence.GetGlobalFloat();
        }
    }

    /// <summary>
    /// Get Wind Zone Values
    /// </summary>
    private void GetWindZoneValues()
    {
        if (windZone && SynchWindZone && (windZone.windMain != WindStrength || windZone.windPulseFrequency != WindPulse || windZone.windTurbulence != windTurbulence))
        {
            WindStrength = windZone.windMain;
            WindPulse = windZone.windPulseFrequency;
            WindTurbulence = windZone.windTurbulence;
            SetShaders();
        }
    }

    /// <summary>
    /// Update Wind
    /// </summary>
    public void SetShaders()
    {
        // General
        _WindStrength.SetGlobalFloat(WindStrength);
        _WindDirection.SetGlobalFloat(transform.rotation.eulerAngles.y);
        _WindPulse.SetGlobalFloat(WindPulse);
        _WindTurbulence.SetGlobalFloat(WindTurbulence);
        _RandomWind.SetGlobalFloat(WindRandomness);

        // Billboard
        if (BillboardEnabled)
        {
            _BillboardEnabled.SetGlobalInt(1);
            _BillboardIntensity.SetGlobalFloat(BillboardIntensity);
        }
        else
            _BillboardEnabled.SetGlobalInt(0);

        // Wetness
        if (WetnessEnabled)
        {
            _WetnessEnabled.SetGlobalInt(1);
            _WetnessIntensity.SetGlobalFloat(WetnessIntensity);
        }
        else
            _WetnessEnabled.SetGlobalInt(0);
    }

    /// <summary>
    /// Reset To Zero
    /// </summary>
    private void ResetShaders()
    {
        _WindStrength.SetGlobalFloat(0);
        _WindPulse.SetGlobalFloat(0);
        _WindTurbulence.SetGlobalFloat(0);
        _RandomWind.SetGlobalFloat(0);

        _BillboardEnabled.SetGlobalInt(0);
        _BillboardIntensity.SetGlobalFloat(0);

        _WetnessEnabled.SetGlobalInt(0);
        _WetnessIntensity.SetGlobalFloat(0);
    }

    #endregion [Public Voids]

    #region [Vegetation Studio Pro]

    private void OnRenderObject()
    {
#if VEGETATION_STUDIO_PRO
            if (windZone)
            {
                var dir = windZone.transform.forward;
                var dir4 = new Vector4(dir.x, Mathf.Abs(windZone.windMain) * WindSpeedFactor * 10, dir.z,
                    WindWavesSize);
                Shader.SetGlobalVector("_AW_DIR", dir4);
                if (WindWavesTexture)
                {
                    Shader.SetGlobalTexture("_AW_WavesTex", WindWavesTexture);
                }
            }
#endif
    }

    private void ResetVSPProperties()
    {
#if VEGETATION_STUDIO_PRO
            if (!WindWavesTexture)
                WindWavesTexture = (Texture2D)Resources.Load("PerlinSeamless", typeof(Texture2D));
            WindWavesSize = 10;
            WindSpeedFactor = 1;

#endif
    }

    #endregion [Vegetation Studio Pro]
}