using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hyhy.RaidersOfChaos
{
    public class MainMenu : MonoBehaviour
    {
        private void Start()
        {
            if (!Pref.IsFirstTime)
            {
                GameData.Ins.LoadData();
            }
            else
            {
                ShopManager.Ins.Init();
                LevelManager.Ins.Init();
                GameData.Ins.musicVol = AudioController.Ins.musicVolume;
                GameData.Ins.soundVol = AudioController.Ins.sfxVolume;
                GameData.Ins.SaveData();
            }

            AudioController.Ins.SetMusicVolume(GameData.Ins.musicVol);
            AudioController.Ins.SetSoundVolume(GameData.Ins.soundVol);

            AudioController.Ins.StopPlayMusic();
            AudioController.Ins.PlayMusic(AudioController.Ins.menus);

            Pref.IsFirstTime = false;
        }
    }
}
