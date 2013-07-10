using UnityEngine;
using System.Collections;

public class SecuredPlayerPrefsTests  {
	static void Assert(bool condition, string message = "") {
    	if (!condition) {
			Debug.LogError("Assert error: " + message);
		}
	}

	public static void RunTests() {
		SecuredPlayerPrefs.SetSecretKey("FhuDwyXfFQO9gD9tg9PRq");
		SecuredPlayerPrefs.DeleteAll();
		SecuredPlayerPrefs.Save();
		
		// int test
		int a = SecuredPlayerPrefs.GetInt("int1", 0);
		Assert(a == 0);
		SecuredPlayerPrefs.SetInt("int1", 1);
		a = SecuredPlayerPrefs.GetInt("int1", 0);
		Assert(a == 1);

		// float test
		float b = SecuredPlayerPrefs.GetFloat("float1", 1f);
		Assert(b == 1f);
		SecuredPlayerPrefs.SetFloat("float1", 2f);
		b = SecuredPlayerPrefs.GetFloat("float1", 2f);
		Assert(b == 2f);

		// string test
		string c = SecuredPlayerPrefs.GetString("string1", "a");
		Assert(c == "a");
		SecuredPlayerPrefs.SetString("string1", "b");
		c = SecuredPlayerPrefs.GetString("string1", "");
		Assert(c == "b");

		// bool test
		bool d = SecuredPlayerPrefs.GetBool("bool1", false);
		Assert(d == false);
		SecuredPlayerPrefs.SetBool("bool1", true);
		d = SecuredPlayerPrefs.GetBool("bool1", false);
		Assert(d == true);
		
		// intarray test
		int[] testArray1 = {1, 2, 3};
		int[] e = SecuredPlayerPrefs.GetIntArray("intarray1", testArray1);
		Assert(ToStringArray(new ArrayList(e)) == ToStringArray(new ArrayList(testArray1)));
		int[] testArray2 = {1, 2, 3, 4};
		SecuredPlayerPrefs.SetIntArray("intarray1", testArray2);
		e = SecuredPlayerPrefs.GetIntArray("intarray1", testArray1);
		Assert(ToStringArray(new ArrayList(e)) == ToStringArray(new ArrayList(testArray2)));
		
		// stringarray test
		string[] strArray1 = {"a", "b"};
		string[] f = SecuredPlayerPrefs.GetStringArray("strarray1", strArray1);
		Assert(ToStringArray(new ArrayList(f)) == ToStringArray(new ArrayList(strArray1)));
		string[] strArray2 = {"a", "b", "c"};
		SecuredPlayerPrefs.SetStringArray("strarray1", strArray2);
		f = SecuredPlayerPrefs.GetStringArray("strarray1", strArray1);
		Assert(ToStringArray(new ArrayList(f)) == ToStringArray(new ArrayList(strArray2)));
		
		// floatarray test
		float[] floatArray1 = {1f, 4.123f};
		float[] g = SecuredPlayerPrefs.GetFloatArray("floatarray1", floatArray1);
		Assert(ToStringArray(new ArrayList(g)) == ToStringArray(new ArrayList(floatArray1)));
		float[] floatArray2 = {4f, 7.13943f};
		SecuredPlayerPrefs.SetFloatArray("floatarray1", floatArray2);
		g = SecuredPlayerPrefs.GetFloatArray("floatarray1", floatArray1);
		Assert(ToStringArray(new ArrayList(g)) == ToStringArray(new ArrayList(floatArray2)));
		
		SecuredPlayerPrefs.Save();

		
		// Vector2 test
		Vector2 vector1 = new Vector2(1f, 4.123f);
		Vector2 h = SecuredPlayerPrefs.GetVector2("vector1", vector1);
		Assert(h == vector1);
		Vector2 vector2 = new Vector2(7.123f, 4.123f);
		SecuredPlayerPrefs.SetVector2("vector1", vector2);
		h = SecuredPlayerPrefs.GetVector2("vector1", vector1);
		Assert(h == vector2);

		// Color test
		Color color1 = new Color(1f, 4.123f, 1.1f, 0.4f);
		Color i = SecuredPlayerPrefs.GetColor("color1", color1);
		Assert(i == color1);
		Color color2 = new Color(7.123f, 4.123f, 0.1f, 0.3f);
		SecuredPlayerPrefs.SetColor("color1", color2);
		i = SecuredPlayerPrefs.GetColor("color1", color1);
		Assert(i == color2);

		// Quaternion test
		Quaternion quat1 = new Quaternion(1f, 4.123f, 12f, 123.3f);
		Quaternion j = SecuredPlayerPrefs.GetQuaternion("quat1", quat1);
		Assert(j == quat1);
		Quaternion quat2 = new Quaternion(7.123f, 4.123f, 1f, 4.123f);
		SecuredPlayerPrefs.SetQuaternion("quat1", quat2);
		j = SecuredPlayerPrefs.GetQuaternion("quat1", quat1);
		Assert(j == quat2);

		// Vector3 test
		Vector3 vectorb1 = new Vector3(9f, 2.123f, 12f);
		Vector3 k = SecuredPlayerPrefs.GetVector3("vectorb1", vectorb1);
		Assert(k == vectorb1);
		Vector3 vectorb2 = new Vector3(123f, 123f, 122f);
		SecuredPlayerPrefs.SetVector3("vectorb1", vectorb2);
		k = SecuredPlayerPrefs.GetVector3("vectorb1", vectorb1);
		Assert(k == vectorb2);

		
		// Debug.Log("str= " + SecuredPlayerPrefs.ToPrettyString());
		// Save
		SecuredPlayerPrefs.Save();
		// Delete all inmemory
		SecuredPlayerPrefs.DeleteAll();
		// Reload
		SecuredPlayerPrefs.SetSecretKey("FhuDwyXfFQO9gD9tg9PRq");
		// Print
		// Debug.Log("str= " + SecuredPlayerPrefs.ToPrettyString());

		SecuredPlayerPrefs.DeleteAll();
		SecuredPlayerPrefs.Save();
	}

	static string ToStringArray(ArrayList a1) {
		string res = "[";
		for (int i = 0; i < a1.Count; ++i) {
			string sep = ", ";
			if (i == a1.Count -1)
				sep = "";
			res += "" + a1[i] + sep;
		}
		res += "]";
		return res;
	}
	
}
