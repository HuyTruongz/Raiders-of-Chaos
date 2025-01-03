using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hyhy.SPM;

namespace hyhy.RaidersOfChaos
{
    public enum GameTag
    {
        Collectable,
        Player,
        Enemy
    }

    public enum GameScene
    {
        Gameplay,
        MainMenu
    }

    public enum KeyPref
    {
        GameData,
        IsFirstTime,
        SpriteOrder
    }

    public enum Direction
    {
        Left,
        Right, Top, Bottom
    }

    public enum GameState
    {
        Staring,
        Playing,
        Wining,
        Gameover
    }

    public enum PlayerState
    {
        Idle,
        Walk,
        Run,
        Attack,
        Jump,
        DoubleJump,
        Hit,
        Fall,
        Dead,
        Ultimate,
        Dash
    }

    public enum AIState
    {
        Walk,
        Dash,
        Ultimate,
        Attack,
        Dead,
        Hit
    }

    public enum PlayerCollider
    {
        Normal,
        Dead
    }

    [System.Serializable]
    public class CollectableItem
    {
        [Range(0f, 1f)] public float spawnRate;
        public int amount;
        [PoolerKeys(target = PoolerTarget.NONE)]
        public string collectablePool;
    }

    [System.Serializable]
    public class ShopItem
    {
        public int Price;
        public string heroName;
        public Sprite preview;
        public Sprite avatar;
        public Player heroBb;
    }

    [System.Serializable]
    public class LevelItem
    {
        public int minBonus;
        public int maxBonus;
        public int minXpBonus;
        public int maxXpBonus;
        public Goal goal;
        public WavePlayer waveCtrFb;
        public FreeParallax mapFb;
    }

    [System.Serializable]
    public class Goal
    {
        public int timeOneSatr;
        public int timeTwoSatr;
        public int timeThreeSatr;

        public int GetStar(int time)
        {
            if (time < timeThreeSatr)
            {
                return 3;
            }
            else if (time < timeTwoSatr)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
    }

}
