using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hyhy.RaidersOfChaos
{
    public class Pref
    {
        public static bool IsFirstTime
        {
            set => SetBool(KeyPref.IsFirstTime.ToString(), value);
            get => GetBool(KeyPref.IsFirstTime.ToString(), true);           
        }

        public static int SpriteOrder
        {
            set => PlayerPrefs.SetInt(KeyPref.SpriteOrder.ToString(), value);
            get => PlayerPrefs.GetInt(KeyPref.SpriteOrder.ToString(), 0);
        }

        public static string GameData
        {
            set => PlayerPrefs.SetString(KeyPref.GameData.ToString(), value);
            get => PlayerPrefs.GetString(KeyPref.GameData.ToString(),"");
        }

        public static void SetBool(string k, bool v)
        {
            PlayerPrefs.SetInt(k, v ? 1 : 0);
        }

        public static bool GetBool(string k, bool defaultValue = false)
        {
           return PlayerPrefs.HasKey(k) ? PlayerPrefs.GetInt(k) == 1 ? true : false : defaultValue;
        }
    }
}
