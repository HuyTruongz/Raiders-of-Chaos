﻿using UnityEngine;
using System;

namespace hyhy.RaidersOfChaos.Editor
{
    [System.Serializable]
    public class AnimState
    {
        public string name;
        public string clipName;
        public int layerIndex;
    }

    public class AnimatorStatesAttribute : PropertyAttribute
    {

    }
}
