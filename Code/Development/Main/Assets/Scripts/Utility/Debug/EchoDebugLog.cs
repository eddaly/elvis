
//#define ECHO_DEBUG_BUILD - add this define to the editor for global debug behaviour.

using UnityEngine;
using System.Collections.Generic;
using System.Text;

//Gives an opportunity to take the debug output string and place it on file or screen etc.
public delegate void DebugOutDelegate(string str, bool endOfOutput);
//Gives an opportunity to print specific information about an object, tailored to that object. 
public delegate string DebugObjectInfoDelegate(object obj);
 
//Wrapper for Unity Debu to allow more global control.
public static class EchoDebugLog
{
	//State of the channel.
	public enum ChannelState
	{
		On,
		Off,
		NoChannel,
		Num,
	}
	
	//Debug print options
	public enum DebugPrintModifier
	{
		None				= 0x0,
		Default 			= 0x1,
		NewLine				= 0x2,
		LineNumber			= 0x4,
		Channel				= 0x8,
	}
	
	//reference type to allow updating in a collection.
	private class DebugChannelState
	{
		public DebugChannelState(ChannelState state)
		{
			this.state = state;
		}
		
		public ChannelState state = ChannelState.On;
	}
	
	private static Dictionary<string, DebugChannelState> debugChannels;
	public static readonly string 					DefaultChannelName = "##";
	private static readonly int						OutStringMaxSize = 16384; 	//size of unity debug buffer.
	private static readonly int						OutSafetyMargin = 128;	//each object print should be less than this.
	private static StringBuilder 					outString;	//Use a string builder to avoid allocs when formatting.
	
	//Static Constructor
	static EchoDebugLog()
	{
		Setup();
	}
	
	[System.Diagnostics.Conditional("ECHO_DEBUG_BUILD")]
	private static void Setup()
	{
		debugChannels = new Dictionary<string, DebugChannelState>(8);
		AddChannel(DefaultChannelName, ChannelState.On);
		outString = new StringBuilder(OutStringMaxSize);
		
		Application.RegisterLogCallback(HandleLog);	//remove normal logging.
	}
	
	//Registered callback for Unity logs.
//	[System.Diagnostics.Conditional("ECHO_DEBUG_BUILD")]
	public static void HandleLog(string logString, string stackTrace, LogType type) 
	{
		LogFormatDefault(logString, stackTrace, type);
	}

	//Add a debug channel.
	[System.Diagnostics.Conditional("ECHO_DEBUG_BUILD")]
	public static void AddChannel(string name, ChannelState channelState)
	{
		debugChannels.Add(name, new DebugChannelState(channelState));
	}
	
	//Switch a debug channel on / off
	[System.Diagnostics.Conditional("ECHO_DEBUG_BUILD")]
	public static void SwitchChannel(string name, ChannelState channelState)
	{
		if(debugChannels.ContainsKey(name))
		{
			debugChannels[name].state = channelState;
		}
	}
	
//	[System.Diagnostics.Conditional("ECHO_DEBUG_BUILD")] Conditional attributes only work on void return types.
	public static ChannelState GetChannelState(string name)
	{
		if(debugChannels.ContainsKey(name))
		{
			return debugChannels[name].state;
		}
		else
		{
			return ChannelState.NoChannel;
		}
	}
	
	//Switch all channels on or off.
	[System.Diagnostics.Conditional("ECHO_DEBUG_BUILD")]
	public static void SwitchAllChannels(ChannelState channelState)
	{	
		foreach(KeyValuePair<string, DebugChannelState> keyVPair in debugChannels)
		{
			keyVPair.Value.state = channelState;
		}
	}
	
	//Assert a condition is true, pause editor / print debug if it isn't.
	[System.Diagnostics.Conditional("ECHO_DEBUG_BUILD")]
	public static void Assert(string channel, bool test, bool breakOnFail, params object [] objArray)
	{
		if(test == false)
		{
			if(objArray == null)
			{
				LogFormatObjects(channel, "");
			}
			else
			{
				LogFormatObjects(channel, objArray);
			}
			if(breakOnFail == true)
			{
//				Debug.DebugBreak();	//no idea what difference is, no docs.
				Debug.Break();
			}
		}
	}
	
	//Simplest call to log objects with default settings.
	[System.Diagnostics.Conditional("ECHO_DEBUG_BUILD")]
	public static void LogFormatObjects(string channel, params object [] objArray)
	{
		LogFormatObjects(channel, DebugPrintModifier.Default, null, null, null, objArray);
	}
	
	//Just to print out a single object, stick it in a single item array for code reuse.
	[System.Diagnostics.Conditional("ECHO_DEBUG_BUILD")]
	public static void LogFormatObjects(string channel, DebugPrintModifier mod, DebugOutDelegate dOutDel,
		DebugObjectInfoDelegate dObjInfoDel, string header, object obj)
	{
		object [] objArray = {obj};
		
		LogFormatObjects(channel, mod, dOutDel, dObjInfoDel, header, objArray);
	}
	
	
	//Print out each object in a list with some formatting.
	[System.Diagnostics.Conditional("ECHO_DEBUG_BUILD")]
	public static void LogFormatObjects(string channel, DebugPrintModifier mod, DebugOutDelegate dOutDel,
		DebugObjectInfoDelegate dObjInfoDel, string header, object [] objArray)
	{
		DebugChannelState dState = null;
		
		if((debugChannels.TryGetValue(channel, out dState) == true) && 
			(dState.state == ChannelState.On))
		{
			if((mod & (DebugPrintModifier.Channel | DebugPrintModifier.Default )) != 0)
			{
				outString.Length = 0;
				outString.Append(channel);
				outString.Append(" ; \n");
			}
			
			//Line number header.
			if((mod & DebugPrintModifier.LineNumber) != 0)
			{
				outString.Append("No.;");
			}
			
			//If there is a header, print it.
			if(header != null)
			{
				outString.Append(header);
				outString.Append("\n");
			}
			
			//We may need to print this in more than one go due to the 
			//limited size of Unitys debug buffer..
			object ob = null;
			for(int i = 0; i < objArray.Length; i++)
			{
				ob = objArray[i];
				
				//Line number at beginning of line.
				if((mod & DebugPrintModifier.LineNumber) == DebugPrintModifier.LineNumber)
				{
					outString.Append(i);
					outString.Append(";");
				}
				//New line per object print.
				if((mod & DebugPrintModifier.NewLine) != 0)
				{
					outString.AppendFormat(" {0}\n", ((dObjInfoDel == null) ? ob : dObjInfoDel(ob)));
				}
				else
				{
					outString.AppendFormat(" {0}", ((dObjInfoDel == null) ? ob : dObjInfoDel(ob)));
				}
				
				//check whether we have buffer space left for more object prints.
				if(outString.Length > (OutStringMaxSize - OutSafetyMargin))
				{
					//print it and reset the out string, we'll make another debug entry.
					if(dOutDel != null)
					{
						dOutDel(outString.ToString(), false);
					}
					else
					{
						Debug.Log(outString);
					}
					outString.Length = 0;
				}
			}
			
			//print it and reset the out string, we'll make another debug entry.
			if(dOutDel != null)
			{
				dOutDel(outString.ToString(), true);
			}
			else
			{
				Debug.Log(outString);
			}
			outString.Length = 0;
		}
	}
	
	//Variable parameters Log
	[System.Diagnostics.Conditional("ECHO_DEBUG_BUILD")]
	public static void LogFormat(string channel, params object[] logInfo)
	{
		LogFormatObjects(channel, logInfo);
	}
	
	//Allow print modifiers to customise output.
	[System.Diagnostics.Conditional("ECHO_DEBUG_BUILD")]
	public static void LogFormatModified(string channel, DebugPrintModifier mod, params object[] logInfo)
	{
		LogFormatObjects(channel, mod, null, null, null, logInfo);
	}
	
	
	//Use default channel
	[System.Diagnostics.Conditional("ECHO_DEBUG_BUILD")]
	public static void LogFormatDefault(params object[] logInfo)
	{
		LogFormatObjects(DefaultChannelName,logInfo);
	}
	
	//------------------------------------------------Replacements
	
	//Simple replacement for log
	[System.Diagnostics.Conditional("ECHO_DEBUG_BUILD")]
	public static void Log(object message)
	{
		if(!(debugChannels[DefaultChannelName].state == ChannelState.Off))
		{	
			Debug.Log(message);
		}
	}
	
	//Simple replacement for log and context
	[System.Diagnostics.Conditional("ECHO_DEBUG_BUILD")]
	public static void Log(object message, UnityEngine.Object context)
	{
		if(!(debugChannels[DefaultChannelName].state == ChannelState.Off))
		{
			Debug.Log(message, context);
		}
	}
	
	//Simple replacement for warning
	[System.Diagnostics.Conditional("ECHO_DEBUG_BUILD")]
	public static void LogWarning(object o)
	{
		if(!(debugChannels[DefaultChannelName].state == ChannelState.Off))
		{
			Debug.LogWarning(o);
		}
	}
	
	//Simple replacement for warning and context
	[System.Diagnostics.Conditional("ECHO_DEBUG_BUILD")]
	public static void LogWarning(object message, UnityEngine.Object context)
	{
		if(!(debugChannels[DefaultChannelName].state == ChannelState.Off))
		{
			Debug.LogWarning(message, context);
		}
	}
	
	//Simple replacement for error
	[System.Diagnostics.Conditional("ECHO_DEBUG_BUILD")]
	public static void LogError(object message)
	{
		if(!(debugChannels[DefaultChannelName].state == ChannelState.Off))
		{
			Debug.LogError(message);
		}
	}
	
	//Simple replacement for error and context
	[System.Diagnostics.Conditional("ECHO_DEBUG_BUILD")]
	public static void LogError(object message, UnityEngine.Object context)
	{
		if(!(debugChannels[DefaultChannelName].state == ChannelState.Off))
		{
			Debug.LogError(message, context);
		}
	}
	
	//Simple replacement for exception.
	[System.Diagnostics.Conditional("ECHO_DEBUG_BUILD")]
	public static void LogException(System.Exception exception)
	{
		if(!(debugChannels[DefaultChannelName].state == ChannelState.Off))
		{
			Debug.LogException(exception);
		}
	}
	
	//Simple replacement for exception and context
	[System.Diagnostics.Conditional("ECHO_DEBUG_BUILD")]
	public static void LogException(System.Exception exception, UnityEngine.Object context)
	{
		if(!(debugChannels[DefaultChannelName].state == ChannelState.Off))
		{
			Debug.LogError(exception, context);
		}
	}
}
