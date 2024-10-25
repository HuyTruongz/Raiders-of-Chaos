using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hyhy.RaidersOfChaos
{
    public enum GameTag
    {
        Vfx,
        HpBar,
        Player,
        AI
    }

    public enum GameLayer
    {
        Player,
        AI,
        Invincible,
        Dead
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
        Right, Up, Down
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
        Attack,
        Jump,
        DoubleJump,
        Fall,
        Dead,
        Ulti,
        Dash
    }

    public enum EnemyState
    {
        Walk,
        Idle,
        Attack,
        Dead,
        Def,
        Hit
    }

    public enum PlayerCollider
    {
        Normal,
        Dead
    }

    [System.Serializable]
    public class Pool
    {
        public GameTag tag;
        public GameObject prefab;
        public int size;
    }
}
