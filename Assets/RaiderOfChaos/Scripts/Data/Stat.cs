using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace hyhy.RaidersOfChaos
{
    public abstract class Stat : ScriptableObject
    {
        public abstract void Save();

        public abstract void Save(int id);

        public abstract void Load();

        public abstract void Load(int id);

        public abstract void UpGrade(UnityAction Success = null, UnityAction Failed = null);

        public abstract bool IsMaxLevel();

        public abstract void ToJson();

    }

}