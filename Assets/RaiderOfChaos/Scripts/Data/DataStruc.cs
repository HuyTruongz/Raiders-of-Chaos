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

}
