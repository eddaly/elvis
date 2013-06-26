using UnityEngine;
using System.Collections;


[System.Serializable]
public class GamePlatform
{
	public bool m_PC;
	public bool m_Xbox;
	public bool m_PS3;
	public bool m_IOS;
	public bool m_Android;
	public bool m_Wii;

	public GamePlatform()
	{
		m_PC = true;
		m_Xbox = true;
		m_PS3 = true;
		m_IOS = true;
		m_Android = true;
		m_Wii = true;
	}


	public bool IsAllowed
	{
		get
		{
			bool allowed = false;

			switch (Application.platform)
			{
				case RuntimePlatform.Android:
				allowed = m_Android;
				break;

				case RuntimePlatform.IPhonePlayer:
				allowed = m_IOS;
				break;

				case RuntimePlatform.PS3:
				allowed = m_PS3;
				break;

				case RuntimePlatform.WiiPlayer:
				allowed = m_Wii;
				break;

				case RuntimePlatform.OSXEditor:
				case RuntimePlatform.OSXPlayer:
				case RuntimePlatform.WindowsEditor:
				case RuntimePlatform.WindowsPlayer:
				allowed = m_PC;
				break;

				case RuntimePlatform.XBOX360:
				allowed = m_Xbox;
				break;
			}

			return allowed;
		}
	}
}
