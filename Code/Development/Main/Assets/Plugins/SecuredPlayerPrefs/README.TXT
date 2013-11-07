Unity Secured PlayerPrefs Plugin
================================

Description
-----------

This plugin aims to replace the standard PlayerPrefs functions. It
signs and encrypts data on disk so a malicious user can't modify data
of your app.

Use it when you need to store sensible data like virtual money,
virtual items, high score, etc.

It also adds a convenient function often asked by unity users:
`string[] GetKeys()` that list all keys stored in preferences.

Usage
-----

An example scene and code is provided in the package.

If you copy/paste code in your application, don't forget to change the
call "SetKey()" with your own key.

You can use SecuredPlayerPrefs as you used PlayerPrefs. The main
difference is the call to the `SetKey(string)` method.

Example to store an int value:

    SecuredPlayerPrefs.SetInt("my-int-key", 1);

Example to fetch an int value:

    int myVar = SecuredPlayerPrefs.GetInt("my-int-key", 0); // 0 is the default value

FAQ
---
* Q: how to change all my code from PlayerPrefs to SecuredPlayerPrefs ?
* A: You can use perl to change all your .cs files,
``$ find ~/Projects/Unity/MyGame/Assets/ -name "*.cs" | perl -pi -e "s/PlayerPrefs/SecuredPlayerPrefs/g"``

* Q: I accidentally change the key between MyApp-version1 and
  MyApp-version2, my users reports data loss. What can I do ?
* A: You can't do nothing. NEVER CHANGE the key in your App, it will
  destroy previous data of your users. This is the main purpose of
  this plugin: nobody can make changes to the stored file so you can
  store valuable data such as virtual money, achievements, high
  scores.

Full API Specification
----------------------

    // Set the signature key. You must call this function before any other of
    // this plugin. This is very important.
    // WARNING: Never change it or this plugins will not be able to open "old"
    // save files.
    void SetSecretKey(string key) 

    // -- Classic PlayerPrefs Methods
    // Returns true if key exists in the preferences, false else.
    bool HasKey(string key) 

    // Removes key and its corresponding value from the preferences. Note: you
    // must call Save() if you want changes to be reflected on disk after this
    // method.
    void DeleteKey(string key) 

    // DeleteAll keys from the preferences. Note: you must call Save() if you
    // want changes to be reflected on disk after this method.
    void DeleteAll() 

    string GetString(string key, string defaultValue) 

    void SetString(string key, string newValue) 

    float GetFloat(string key, float defaultValue) 

    void SetFloat(string key, float newValue) 

    int GetInt(string key, int defaultValue) 

    void SetInt(string key, int newValue) 

    // Save the preferences memory representation to filesystem. You should
    // call .Save() only when it's necessary. For instance:
    //   - When the user quits your app
    //   - When the user changes settings
    //   - When the user ends a game
    void Save() 

    // -- New Getter and Setter Methods
    void SetBool(string key, bool newValue) 

    bool GetBool(string key, bool defaultValue) 

    void SetIntArray(string key, int[] newValue) 

    int[] GetIntArray(string key, int[] defaultValue) 

    void SetStringArray(string key, string[] newValue) 

    string[] GetStringArray(string key, string[] defaultValue) 

    void SetFloatArray(string key, float[] newValue) 

    float[] GetFloatArray(string key, float[] defaultValue) 

    void SetVector2(string key, Vector2 newValue) 

    Vector2 GetVector2(string key, Vector2 defaultValue) 

    void SetVector3(string key, Vector3 newValue) 

    Vector3 GetVector3(string key, Vector3 defaultValue) 

    void SetColor(string key, Color newValue) 

    Color GetColor(string key, Color defaultValue) 

    void SetQuaternion(string key, Quaternion newValue) 

    Quaternion GetQuaternion(string key, Quaternion defaultValue) 

    // -- Convenience methods
    // List all keys stored in preferences.
    string[] GetKeys() 

    // Return a JSON string representation of all keys and values stored.
    // Used if you want to synchronize over network.
    string ToJson() 

    // Return a representation of all keys and values stored. Used for debugging.
    string ToPrettyString() 

ChangeLog
---------

### Version 1.3 - 2012/10/29
 - New unit tests
 - New methods:

    void SetBool(string key, bool newValue)
    bool GetBool(string key, bool defaultValue)
    void SetIntArray(string key, int[] newValue)
    int[] GetIntArray(string key, int[] defaultValue)
    void SetStringArray(string key, string[] newValue)
    string[] GetStringArray(string key, string[] defaultValue)
    void SetFloatArray(string key, float[] newValue)
    float[] GetFloatArray(string key, float[] defaultValue)
    void SetVector2(string key, Vector2 newValue)
    Vector2 GetVector2(string key, Vector2 defaultValue)
    void SetVector3(string key, Vector3 newValue)
    Vector3 GetVector3(string key, Vector3 defaultValue)
    void SetColor(string key, Color newValue)
    Color GetColor(string key, Color defaultValue)
    void SetQuaternion(string key, Quaternion newValue)
    Quaternion GetQuaternion(string key, Quaternion defaultValue)

### Version 1.2 - 2012/10/29
 - New method:

    // Return a JSON string representation of all keys and values stored.
    // Used if you want to synchronize over network.
    string ToJson()

### Version 1.1 - 2012/10/29
 - Bug fixes

### Version 1.0 - 2012/10/29
 - First version
