using UnityEngine;
using UnityEditor;

#if WOTD_MOBILE
public class MobileConfigWindow : EditorWindow
{
	bool groupEnabled;
	
	//Class to encapsulate different sets of what are static settings
	class IOSPlayerSettings
	{
		public string applicationDisplayName = "wotd";
			
		public ScriptCallOptimizationLevel scriptCallOptimization = ScriptCallOptimizationLevel.SlowAndSafe;

		public iOSSdkVersion sdkVersion = iOSSdkVersion.DeviceSDK;

		public iOSTargetOSVersion targetOSVersion = iOSTargetOSVersion.iOS_5_1;

		public iOSTargetDevice targetDevice = iOSTargetDevice.iPhoneAndiPad;

		public iOSTargetResolution targetResolution = iOSTargetResolution.Native;

		public bool prerenderedIcon = false;

		public bool requiresPersistentWiFi = false;

		public iOSStatusBarStyle statusBarStyle = iOSStatusBarStyle.Default;

		public bool exitOnSuspend = false;

		public iOSShowActivityIndicatorOnLoading showActivityIndicatorOnLoading = iOSShowActivityIndicatorOnLoading.WhiteLarge;
	}
	
	//A subset of all possible player settings that can apply to mobile phones.
	class MobilePlayerSettings
	{
		public UIOrientation defaultInterfaceOrientation = UIOrientation.LandscapeRight;
		public bool statusBarHidden = true;
		public bool use32BitDisplayBuffer = false;
		public string iPhoneBundleIdentifier = "com.echo-peak.wotd";
		public string bundleIdentifier = "com.echopeak.wotd";			//Note, Android won't build with hyphen.
		public string bundleVersion = "1.0";
		public TargetGlesGraphics targetGlesGraphics = TargetGlesGraphics.OpenGLES_2_0;
		public int accelerometerFrequency = 0;
		public ApiCompatibilityLevel apiCompatibilityLevel = ApiCompatibilityLevel.NET_2_0_Subset;
		public StrippingLevel strippingLevel = StrippingLevel.StripByteCode;
		public bool usePlayerLog = false;  //Logging shouldn't be enabled for submission builds...
		public ColorSpace colorSpace = ColorSpace.Gamma;
		public string companyName = "Echo Peak Ltd.";
		public string productName = "Way Of The Dogg";
	}
	
#if UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_OSX
	// Add menu item named "My Window" to the Window menu
	[MenuItem("Edit/Project Settings/Echo Mobile Config")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(MobileConfigWindow));
	}

	void OnGUI()
	{
		GUILayout.Label ("IOS Quality Settings", EditorStyles.boldLabel);
		
		Rect aRect = EditorGUILayout.BeginHorizontal ("Button");
		
		//Apply default iOS quality settings to project.
		if(GUI.Button(aRect, GUIContent.none))
		{
			MobileConfigControl.InitialiseConfig(null);
		}
		
		GUILayout.Label ("Apply Defaults To Project");
		
		EditorGUILayout.EndHorizontal();
		
		GUILayout.Label ("IOS Player Settings", EditorStyles.boldLabel);
		
		aRect = EditorGUILayout.BeginHorizontal ("Button");
		
		if(GUI.Button(aRect, GUIContent.none))
		{
			IOSPlayerSettings settings = new IOSPlayerSettings();
			MobilePlayerSettings msettings = new MobilePlayerSettings();
			
			ApplyIOSPlayerSetting(settings);
			ApplyMobilePlayerSettings(msettings);
		}
		
		GUILayout.Label ("Apply Defaults To Project");
		
		EditorGUILayout.EndHorizontal();
		
	}
#endif
	
	//set the project player settings to defaults (iOS specific)
	private void ApplyIOSPlayerSetting(IOSPlayerSettings iop)  
	{
		PlayerSettings.iOS.applicationDisplayName = iop.applicationDisplayName;
			
		PlayerSettings.iOS.scriptCallOptimization = iop.scriptCallOptimization;

		PlayerSettings.iOS.sdkVersion = iop.sdkVersion;

		PlayerSettings.iOS.targetOSVersion = iop.targetOSVersion;

		PlayerSettings.iOS.targetDevice = iop.targetDevice;

		PlayerSettings.iOS.targetResolution = iop.targetResolution;

		PlayerSettings.iOS.prerenderedIcon = iop.prerenderedIcon;

		PlayerSettings.iOS.requiresPersistentWiFi = iop.requiresPersistentWiFi;

		PlayerSettings.iOS.statusBarStyle = iop.statusBarStyle;

		PlayerSettings.iOS.exitOnSuspend = iop.exitOnSuspend;

		PlayerSettings.iOS.showActivityIndicatorOnLoading = iop.showActivityIndicatorOnLoading;
	}
	
	//set the project player settings to defaults (mobile general).
	private void ApplyMobilePlayerSettings(MobilePlayerSettings mps)
	{
		PlayerSettings.defaultInterfaceOrientation = mps.defaultInterfaceOrientation;
		PlayerSettings.statusBarHidden = mps.statusBarHidden;
		PlayerSettings.use32BitDisplayBuffer = mps.use32BitDisplayBuffer;
		PlayerSettings.iPhoneBundleIdentifier = mps.iPhoneBundleIdentifier;
		PlayerSettings.bundleIdentifier = mps.bundleIdentifier;
		PlayerSettings.bundleVersion = mps.bundleVersion;
		PlayerSettings.targetGlesGraphics = mps.targetGlesGraphics;
		PlayerSettings.accelerometerFrequency = mps.accelerometerFrequency;
		PlayerSettings.apiCompatibilityLevel = mps.apiCompatibilityLevel;
		PlayerSettings.strippingLevel = mps.strippingLevel;
		PlayerSettings.usePlayerLog = mps.usePlayerLog;
		PlayerSettings.colorSpace = mps.colorSpace;
		PlayerSettings.companyName = mps.companyName;
		PlayerSettings.productName = mps.productName;
	}
}
#endif