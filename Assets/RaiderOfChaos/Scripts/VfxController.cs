using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hyhy.RaidersOfChaos
{
    public class VfxController : MonoBehaviour
    {
        public bool useCamShake;
        public float shakeDur = 0f;
        public float shakeFreq = 0f;
        public float shakeAmp = 1f;

        private AudioSource m_aus;
        public AudioClip[] sounds;

        private void Awake()
        {
            m_aus = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            if (AudioController.Ins)
            {
                AudioController.Ins.PlaySound(sounds,m_aus);
            }
            CamShakeVfx();
        }
        private void OnDisable()
        {
            if (m_aus)
            {
                m_aus.Stop();
            }
            else if(AudioController.Ins)
            {
                AudioController.Ins.sfxAus.Stop();
            }
        }

        private void CamShakeVfx()
        {
            if (!useCamShake || !CamShake.ins) return;
            CamShake.ins.ShakeTrigger(shakeDur, shakeFreq, shakeAmp);
        }
    }
}
