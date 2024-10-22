using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hyhy.RaidersOfChaos
{
    [CreateAssetMenu(menuName = "Game Setting",fileName = "GameSetting")]
    public class GamePlaySetting : ScriptableObject
    {
        public bool isOnMobile;
    }
}
