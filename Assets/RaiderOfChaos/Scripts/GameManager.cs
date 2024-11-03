using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hyhy.RaidersOfChaos
{
    public class GameManager : Singleton<GameManager>
    {
        public GamePlaySetting setting;
        [SerializeField]
        private Player m_player;

        public Player Player { get => m_player; set => m_player = value; }

        public override void Awake()
        {
            MakeSingleton(false);
        }
    }
}
