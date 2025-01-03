using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace hyhy.RaidersOfChaos
{
    [CreateAssetMenu(fileName = "AI Stat", menuName = "Stat/Enemy")]
    public class AIStat : ActorStat
    {
        [Header("Common :")]
        [Range(0f, 1f)]
        public float atkRate;
        [Range(0f, 1f)]
        public float dashRate;
        [Range(0f, 1f)]
        public float ultiRate;
        public float atkTime;
        public float ultiTime;
        public float dashTime;
        [Header("Colect ")]
        public float minXpBonus;
        public float maxXpBonus;
        public float minEnegyBonus;
        public float maxEnegyBonus;
        [Header("Level Up")]
        public float hpUpRate;
        public float dmgUpRate;
        public float ultiUprate;

        public float CurHp
        {
            get => MaxUpgaredeValue(2, hp, hpUpRate);
        }

        public float CurDmg
        {
            get => MaxUpgaredeValue(5, damage, dmgUpRate);
        }

        public float CurUltiRate
        {
            get => MaxUpgaredeValue(4, ultiRate, ultiUprate, true);
        }

        public float XpBonus
        {
            get => Random.Range(minXpBonus, maxXpBonus) * (GameData.Ins.curLevelId + 1);
        }

        public float EnergyBonus
        {
            get => Random.Range(minEnegyBonus, maxEnegyBonus);
        }

        private float MaxUpgaredeValue(float factor, float oldVlue, float upValueRate, bool isPercent = false)
        {
            float maxValue = 0f;

            if (isPercent)
            {
                for (int i = 0; i < GameData.Ins.curLevelId + 1; i++)
                {
                    maxValue += (Helper.UpgradeForm(i, factor) * upValueRate) / 100;
                }
            }
            else
            {
                for (int i = 0; i < GameData.Ins.curLevelId + 1; i++)
                {
                    maxValue += (Helper.UpgradeForm(i, factor) * upValueRate);
                }
            }

            maxValue += oldVlue;
            return maxValue;
        }

        public override void UpgradeCore()
        {
            hp = CurHp;
            damage = CurDmg;
            ultiRate = CurUltiRate;
        }

        public override void UpGrade(UnityAction Success = null, UnityAction Failed = null)
        {
            GameData.Ins.curLevelId++;
            UpgradeCore();
        }
    }
}
