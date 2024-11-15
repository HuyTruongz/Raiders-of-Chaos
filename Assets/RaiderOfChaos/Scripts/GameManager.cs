using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

namespace hyhy.RaidersOfChaos
{
    public class GameManager : Singleton<GameManager>
    {
        public GamePlaySetting setting;
        [SerializeField]
        private Player m_player;
        private FreeParallax m_map;
        private WavePlayer m_waveCtr;
        private LevelItem m_curLevel;

        private int m_killed;
        private float m_gplayTimeCounting;
        private int m_missionCoinBouns;
        private int m_missionXpBouns;
        private int m_stars;
        private StateMachine<GameState> m_fsm;

        public Player Player { get => m_player; set => m_player = value; }
        public WavePlayer WaveCtr { get => m_waveCtr; set => m_waveCtr = value; }
        public int Killed { get => m_killed; set => m_killed = value; }
        public float GplayTimeCounting { get => m_gplayTimeCounting; set => m_gplayTimeCounting = value; }
        public int MissionCoinBouns { get => m_missionCoinBouns; set => m_missionCoinBouns = value; }
        public int Stars { get => m_stars;}
        public StateMachine<GameState> Fsm { get => m_fsm;}

        public override void Awake()
        {
            MakeSingleton(false);        
            m_fsm = StateMachine<GameState>.Initialize(this);
            m_fsm.ChangeState(GameState.Playing);
        }

        private void Start()
        {
            Init();
            StartCoroutine(CamFollowDelay());
        }

        public void Init()
        {
            m_curLevel = LevelManager.Ins.levels[GameData.Ins.curLevelId];

            if (m_curLevel == null) return;

            m_missionCoinBouns = Random.Range(m_curLevel.minBonus, m_curLevel.maxBonus);
            m_missionXpBouns = Random.Range(m_curLevel.minXpBonus,m_curLevel.maxXpBonus);
            if (m_curLevel.mapFb)
            {
                Instantiate(m_curLevel.mapFb, Vector3.zero, Quaternion.identity);
            }

            ChangPlayer();

            if (!m_curLevel.waveCtrFb) return;

            m_waveCtr = Instantiate(m_curLevel.waveCtrFb, Vector3.zero, Quaternion.identity);
            m_waveCtr.waveBegins.AddListener(() =>
            {

            });

            m_waveCtr.finalWaveComplete.AddListener(() =>
            {
                MissionCompleted();
            });
            m_waveCtr.StartWave();

            Pref.SpriteOrder = 0;
        }

        public void ChangPlayer()
        {
            if(!m_player) return;
            Vector3 spawnPos = m_player ? m_player.transform.position : Vector3.zero;
            if (m_player)
            {
                Destroy(m_player.gameObject);
            }
            ShopItem shopItem = ShopManager.Ins.items[GameData.Ins.curPlayerId];
            if (shopItem == null) return;
            m_player = Instantiate(shopItem.heroBb, spawnPos, Quaternion.identity);
            m_player.Init();
        }

        public void AddCoin(int coinToAdd)
        {
            GameData.Ins.coin += coinToAdd;
            GameData.Ins.SaveData();
        }

        public void AddHp(int hpToAdd)
        {
            if(!m_player) return;
            m_player.CurHp += hpToAdd;
        }

        public void Gameover()
        {
            m_fsm.ChangeState(GameState.Gameover);

        }

        public void MissionCompleted()
        {
            m_fsm.ChangeState(GameState.Wining);
        }

        public void Replay()
        {
            if(!WaveCtr) return;
            m_waveCtr.StopAllCoroutines();
            SceneController.Ins.LoandGamePlay();
        }

        public void SetMapSpeed(float speed)
        {
            if(!m_map) return;
        }

        private IEnumerator CamFollowDelay()
        {
            yield return new WaitForSeconds(0.3f);
            CameraFollow.ins.target = m_player.transform;
        }

        #region
        private void Staring_Enter() { }
        private void Staring_Update() { }
        private void Staring_Exit() { }
        private void Playing_Enter() { }
        private void Playing_Update()
        {
            m_gplayTimeCounting += Time.deltaTime;
        }
        private void Playing_Exit() { }
        private void Wining_Enter()
        {
            m_player.AddXp(m_missionXpBouns);
            AddCoin(m_missionCoinBouns);
            int timeScore = Mathf.RoundToInt(m_gplayTimeCounting);
            m_stars = m_curLevel.goal.GetStar(timeScore);
            GameData.Ins.UpdatelevelStars(GameData.Ins.curLevelId,m_stars);
            GameData.Ins.UpdateLevelScore(GameData.Ins.curLevelId,timeScore);
            GameData.Ins.curLevelId++;
            GameData.Ins.UpdateLevelUnlocked(GameData.Ins.curLevelId,true);
            GameData.Ins.SaveData();
        }
        private void Wining_Update() { }
        private void Wining_Exit() { }
        private void Gameover_Enter()
        {

        }
        private void Gameover_Update() { }
        private void Gameover_Exit() { }

        #endregion
    }
}
