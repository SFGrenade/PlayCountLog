using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Win32;
using Modding;
using UnityEngine;

namespace PlayCountLog;

[UsedImplicitly]
public class PlayCountLog : Mod
{
    public override void Initialize()
    {
        #region Achievements

        foreach (string keyname in Registry.CurrentUser.OpenSubKey("SOFTWARE").OpenSubKey("Team Cherry").OpenSubKey("Hollow Knight").GetValueNames())
        {
            if (keyname.Contains("_"))
            {
                string paddedName = keyname.Substring(0, keyname.LastIndexOf('_'));
                try
                {
                    string decryptedName = Encryption.Decrypt(paddedName);
                    string ret = (string) typeof(PlayerPrefsSharedData).GetMethod("ReadEncrypted", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(Platform.Current.EncryptedSharedData, new object[] { decryptedName });
                    Log($"'{keyname}'/'{decryptedName}' => '{ret}'");
                }
                catch (Exception)
                {
                    string retString = PlayerPrefs.GetString(paddedName, "DOESN'T EXIST");
                    if (retString.Equals("DOESN'T EXIST"))
                    {
                        float retFloat = PlayerPrefs.GetFloat(paddedName, -1337.7f);
                        if (retFloat == -1337.7f)
                        {
                            int retInt = PlayerPrefs.GetInt(paddedName, -1337);
                            if (retInt == -1337)
                            {
                                Log($"Couldn't read '{paddedName}'!");
                            }
                            else
                            {
                                Log($"'{paddedName}' => '{retInt}'");
                            }
                        }
                        else
                        {
                            Log($"'{paddedName}' => '{retFloat}'");
                        }
                    }
                    else
                    {
                        if (paddedName.Equals("unity.player_session_count"))
                            Log($"You have started Hollow Knight {retString} times!");
                        else
                            Log($"'{paddedName}' => '{retString}'");
                    }
                }
            }
        }

        #endregion

    }
}