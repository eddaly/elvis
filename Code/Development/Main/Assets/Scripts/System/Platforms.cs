using UnityEngine;
using System.Collections;
using System;


[Flags]
public enum Platforms
{
	None	= 0,
	PC		= 1 << 0,
	Xbox	= 1 << 1,
	PS3		= 1 << 2,
	iOS		= 1 << 3,
	Android	= 1 << 4,
	Wii		= 1 << 5,
	
	All		= PC + Xbox + PS3 + iOS + Android
}

public class Platform
{
	public static Platforms CurrentPlatform
	{
		get
		{
			switch (Application.platform)
			{
				case RuntimePlatform.Android:
				return Platforms.Android;
								
				case RuntimePlatform.IPhonePlayer:
				return Platforms.iOS;
				
				case RuntimePlatform.PS3:
				return Platforms.PS3;

				case RuntimePlatform.WiiPlayer:
				return Platforms.Wii;
				
				case RuntimePlatform.OSXEditor:
				case RuntimePlatform.OSXPlayer:
				case RuntimePlatform.WindowsEditor:
				case RuntimePlatform.WindowsPlayer:
				return Platforms.PC;
				
				
				
				case RuntimePlatform.XBOX360:
				return Platforms.Xbox;
			}

			return Platforms.None;
		}
	}

	public static bool IsCurrentPlatform(Platforms platform)
	{
		return ((platform & CurrentPlatform) == CurrentPlatform);
	}





}
