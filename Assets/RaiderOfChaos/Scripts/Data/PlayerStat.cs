using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace hyhy.RaidersOfChaos
{
    [CreateAssetMenu(fileName = "Player Stat", menuName = "Stat/Player")]
    public class PlayerStat : ActorStat
    {
        [Header("Ability: ")]
        public float runSpeed;
        public float atkSpeed;
        public float atkRate;
        public float dashRate;
        public float dashDist;
        public float ultiEnergy;
        public float defense;
        public float luck;

        [Header("Level Up Base")]
        public int maxLevel;
        public int level;
        public int playerLevel;
        public float xp;
        public float lvUpxpRequired;
        public int point;

        [Header("Levle Up")]
        public float xpUpRate;
        public float atkSpeedRate;
        public float hpUpRate;
        public float dmgUpRate;
        public float defUpRate;
        public float luckUpRate;
        public float pointUpRate;

        [Header("Point Required")]
        public int pointRequired;
        public int pointRequiredUp;

        public float LvUpXprequiUp
        {
            get => Helper.UpgradeForm(playerLevel, 2) * xpUpRate;
        }

        public float AtkSpeedUp
        {
            get => Helper.UpgradeForm(level, 250) * atkSpeed;
        }

        public int PointReceiveUp
        {
            get => Mathf.RoundToInt(Helper.UpgradeForm(playerLevel, 2) * pointUpRate);
        }

        public float HpUp
        {
            get => Helper.UpgradeForm(level, 2) * hpUpRate;
        }

        public float DmgUp
        {
            get => Helper.UpgradeForm(level, 6) * dmgUpRate;
        }

        public float DefUp
        {
            get => Helper.UpgradeForm(level, 10) * defUpRate;
        }

        public override bool IsMaxLevel()
        {
            return level >= maxLevel;
        }

        private int PointRequiredUp
        {
            get => Mathf.RoundToInt(Helper.UpgradeForm(level, 2) * pointRequiredUp);
        }

        public float LuckUp
        {
            get => (Helper.UpgradeForm(level, 2) * luckUpRate) / 100;
        }

        public float MaxHp
        {
            get => MaxUpgradeValue(2, hp, hpUpRate);
        }

        public float MaxAtkSpeed
        {
            get => MaxUpgradeValue(250, atkSpeed, atkSpeedRate);
        }

        public float MaxDef
        {
            get => MaxUpgradeValue(10, defense, defUpRate);
        }

        public float MaxDmg
        {
            get => MaxUpgradeValue(6, damage, dmgUpRate);
        }

        public float MaxLuck
        {
            get => MaxUpgradeValue(2, luck, luckUpRate, true);
        }

        public override void Load(int id)
        {
            string data = GameData.Ins.GetPlayerStats(id);
            if (!string.IsNullOrEmpty(data))
            {
                JsonUtility.FromJsonOverwrite(data, this);
            }
        }

        public override void Save(int id)
        {
            string data = JsonUtility.ToJson(this);
            GameData.Ins.UpdatePlayerStats(id, data);
            GameData.Ins.SaveData();
        }

        private float MaxUpgradeValue(float factor, float oldValue, float upValue, bool isPercent = false)
        {
            float maxValue = 0;

            if (isPercent)
            {
                for (int i = level; i < maxLevel; i++)
                {
                    maxValue += (Helper.UpgradeForm(i, factor) * upValue) / 100f;
                }
            }
            else
            {
                for (int i = level; i < maxLevel; i++)
                {
                    maxValue += (Helper.UpgradeForm(i, factor) * upValue);
                }
            }

            maxValue += oldValue;
            return maxValue;
        }

        public override void UpGrade(UnityAction Success = null, UnityAction Failed = null)
        {
            if (!IsMaxLevel() && point >= pointRequired)
            {
                UpgradeCore();

                if (Success != null)
                {
                    Success.Invoke();
                }
            }
            else
            {
                if (Failed != null)
                {
                    Failed.Invoke();
                }
            }
        }

        public override void UpgradeCore()
        {
            point -= pointRequired;
            pointRequired += PointRequiredUp;
            hp += HpUp;
            atkSpeed += AtkSpeedUp;
            damage += DmgUp;
            defense += DefUp;
            luck += LuckUp;
            level++;

            level = Mathf.Clamp(level, 1, maxLevel);
            hp = Mathf.Clamp(hp, 0, MaxHp);
            atkSpeed = Mathf.Clamp(atkSpeed, 1, MaxAtkSpeed);
            damage = Mathf.Clamp(damage, 0, MaxHp);
            defense = Mathf.Clamp(defense, 0f, MaxDef);
            luck = Mathf.Clamp(luck, 0, 1f);

            //Save(GameData.Ins.curPlayerId);
        }

        public override void UpgradeTomax()
        {
            while (level < maxLevel)
            {
                level++;
                UpgradeCore();
            }
        }

        public override void LevelUpCore()
        {
            xp -= lvUpxpRequired;
            lvUpxpRequired += LvUpXprequiUp;
            point += PointReceiveUp;
            playerLevel++;
        }

        public IEnumerator LevelUpCo(UnityAction OnLevelUp = null)
        {
            while (xp >= lvUpxpRequired)
            {
                LevelUpCore();
                if (OnLevelUp != null)
                {
                    OnLevelUp.Invoke();
                }

                Save(GameData.Ins.curPlayerId);

                yield return new WaitForSeconds(0.5f);
            }
            yield return null;
        }
    }
}
