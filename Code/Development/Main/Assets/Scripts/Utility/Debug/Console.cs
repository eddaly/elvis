using UnityEngine;
using System.Collections;
using System;

[Flags]
public enum Channels
{
	None	= 0,

	// Message Priority
	Error		= 1 << 1,
	Warning		= 1 << 2,
	Debug		= 1 << 3,
	Info		= 1 << 4,
	
	// Platforms				
	General		= 1 << 9,
	Ps3			= 1 << 10,
	Xbox		= 1 << 11,
	Wii			= 1 << 12,
	
	iOS			= 1 << 13,
	Android		= 1 << 14,
	
	PC			= 1 << 15,
	
	// Helpers
	Consoles	= Ps3 | Xbox | Wii,
	Mobile		= iOS | Android,

	DefaultPrio = Info,
}

public class Console
{
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
	static Channels s_AllowedChannels = Channels.PC | Channels.General | Channels.DefaultPrio;	
#elif UNITY_XBOX360
	static Channels s_AllowedChannels = Channels.Xbox | Channels.General | Channels.DefaultPrio;	
#elif UNITY_PS3
	static Channels s_AllowedChannels = Channels.Ps3 | Channels.General | Channels.DefaultPrio;	
#elif WOTD_MOBILE
	static Channels s_AllowedChannels = Channels.iOS | Channels.General | Channels.DefaultPrio;	
#else
	static Channels s_AllowedChannels = Channels.Consoles | Channels.Mobile | Channels.General | Channels.DefaultPrio;	
#endif

	static Channels s_DefaultChannel = Channels.General | Channels.Info;

	public static void SetChannels(Channels channel)
	{
		s_AllowedChannels = channel;
	}

	public static void AddChannel(Channels channel)
	{
		s_AllowedChannels |= channel;
	}

	public static void RemoveChannel(Channels channel)
	{
		s_AllowedChannels &= ~channel;
	}

	public static void WriteLine(Channels channel, string message, params object[] details)
	{
		string result = System.String.Format(message, details);
		WriteLine(channel, result);
	}

	public static void WriteLine(Channels channel, string message)
	{
		// get the first 8 bits
		byte priorityRequest = (byte)channel;
		byte priorityCurrent = (byte)s_AllowedChannels;

		int request = (int)channel;
		request = (request >> 8) << 8;
		Channels requestChannel = (Channels)request;
		//Debug.Log(channel + " : " + message + "\n " + priorityRequest + " " + priorityCurrent + "\n " + requestChannel);
		//message = Time.realtimeSinceStartup + ": " + message;
		if (priorityRequest != 0 && priorityRequest <= priorityCurrent)
		{
			if ((s_AllowedChannels & (Channels)requestChannel) == requestChannel)
			{
				if ((channel & Channels.Error) == Channels.Error)
				{
					EchoDebugLog.LogError(message);
				}
				else if ((channel & Channels.Warning) == Channels.Warning)
				{
					EchoDebugLog.LogWarning(message);
				}
				else
				{
					EchoDebugLog.Log(message);
				}
			}
		}
	}

	public static void WriteLine(Channels channel, object variable)
	{
		WriteLine(channel, variable.ToString());
	}



	public static void WriteLine(string message, params object[] details)
	{
		WriteLine(s_DefaultChannel, message, details);
	}

	public static void WriteLine(object variable)
	{
		WriteLine(s_DefaultChannel, variable.ToString());
	}

	public static void WriteLine(string message)
	{
		WriteLine(s_DefaultChannel, message);
	}
	
	public static void WriteLineWarning(string message)
	{
		WriteLine(Channels.General | Channels.Warning, message);
	}
}
