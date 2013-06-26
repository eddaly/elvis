using UnityEngine;
using System;

//JW:TODO - Really this should be merged with the PlayerSettings configs in MobileConfigWindow.  
//Low priority right now, to refactor if / when we need multiple configs (pre-beta).

#if WOTD_MOBILE
//An individual mobile configuration, an XML serialisable class with responsibility for storing
//config details.  It's possible to set defaults for the record or to set it up from an XML file
//using MobileConfigControl.
//It's also possible to save the record as an XML file using MobileConfigControl, but only in the editor.
public class MobileConfig
{
	//Quality Settings.
	public int 						pixelLightCount = 0; //no pixel lights, expensive for GPU.
	public AnisotropicFiltering		anisotropicFiltering = AnisotropicFiltering.Disable; //Anisotropic expensive for GPU.
	public int						antiAliasing = 0; //anti-aliasing very expensive on mobile.
	//Animation uses CPU, this could be turned down for that.
	public BlendWeights				blendWeights = BlendWeights.FourBones; 	
	//This means that mip-mapped textures are at half-res to keep memory down.
	public int						masterTextureLimit = 1;
	public int						maximumLODLevel = 0; //All LODs are shown (if any)
	public float					lodBias = 0.3f; //LOD switching distances can be set per LOD group but they are multiplied (scaled) by this one setting.
	public int						particleRaycastBudget = 1; //collision testing ray-casts per-frame.t
	public int 						vSyncCount = 2; //collision testing ray-casts per-frame.
	public bool						softVegetation = false; //timing for 30fps on a 60hz device (iphone).
	public float					shadowDistance = 0.0f;  //don't render shadows.
	public int						shadowCascades = 0; //""
	public int						maxQueuedFrames = -1;	//no limit on graphics driver queued frames, note : at time of writing this is ignored by OpenGL.
}

//Controls the loading and set-up of different configurations
//for the different mobile platforms.
public static class MobileConfigControl
{
	public static MobileConfig currentConfig;
	public static readonly string StdFileName = "MobileConfig";
	public static readonly string StdFileExtension = ".xml";
	public static readonly string StdDirectory = "";
	
	//Looad and apply config, null will just create the default.
	public static void InitialiseConfig(string filename)
	{
		currentConfig = LoadConfigFile(filename);
		ApplyConfig(currentConfig);
	}
	
	//Loads a given config and returns a new MobileConfig object 
	//containing its data.
	private static MobileConfig LoadConfigFile(string fileName)
	{
		if(fileName == null)
		{
			return new MobileConfig();
		}
		return null;
	}
	
#if UNITY_EDITOR
	//Saves a given config file with a given name
	private static void SaveConfigFile(MobileConfig mConf, string fileName)
	{
		
	}
#endif
	
	//Apply config to quality settings.
	public static void ApplyConfig(MobileConfig mConf) 
	{
		//Quality Settings. (Deprecated)
		//------------------------------------------------------
/*
		QualitySettings.pixelLightCount = mConf.pixelLightCount;
		QualitySettings.anisotropicFiltering = mConf.anisotropicFiltering;
		QualitySettings.antiAliasing = mConf.antiAliasing;
		QualitySettings.blendWeights = mConf.blendWeights; 	
		QualitySettings.masterTextureLimit = mConf.masterTextureLimit;
		QualitySettings.maximumLODLevel = mConf.maximumLODLevel;
		QualitySettings.lodBias = mConf.lodBias;
		QualitySettings.particleRaycastBudget = mConf.particleRaycastBudget;
		QualitySettings.vSyncCount = mConf.vSyncCount;
		QualitySettings.softVegetation = mConf.softVegetation;
		QualitySettings.shadowDistance = mConf.shadowDistance;
		QualitySettings.shadowCascades = mConf.shadowCascades;
		QualitySettings.maxQueuedFrames = mConf.maxQueuedFrames;
		
		MobileGame.SetLightShadows(LightShadows.None);
*/
	}
}
#endif
