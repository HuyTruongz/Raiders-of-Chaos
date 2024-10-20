using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace hyhy.RaidersOfChaos.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ActorStat), editorForChildClasses: true)]
    public class ActorStatEditor : UnityEditor.Editor
    {
        protected ActorStat m_target;
        protected string m_fileName;
        private string path = string.Empty;

        public override void OnInspectorGUI()
        {
            m_target = (ActorStat)target;
            path = Application.dataPath + "/Editor/Resources/StatData";
            m_target.thumb = (Sprite)EditorGUILayout.ObjectField(
                m_target.thumb, typeof(Sprite), false, GUILayout.Width(80), GUILayout.Height(80));
            base.OnInspectorGUI();

            m_fileName = $"actor_data_{m_target.id}";
            if (GUILayout.Button("Save"))
            {
                Save();
            }

            if (GUILayout.Button("Load"))
            {
                Load(m_fileName);
            }

            if (GUILayout.Button("Upgrade"))
            {
                Upgrade();
            }

            if (GUILayout.Button("Upgrade To Max"))
            {
                UpgradeToMax();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(this);
            }
        }

        public virtual void UpgradeToMax()
        {
            m_target.UpgradeTomax();
        }

        public virtual void Upgrade()
        {
            m_target.UpgradeCore();
        }

        public virtual void Load(string fileName)
        {
            string data = Resources.Load<TextAsset>($"StatData/{fileName}").ToString();
            if (!string.IsNullOrEmpty(data))
            {
                JsonUtility.FromJsonOverwrite(data, m_target);
            }
        }

        public virtual void Save()
        {
            string filePath = $"{path}/{m_fileName}.txt";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            m_target.id = Helper.GenerateUID();
            m_fileName = $"actor_data_{m_target.id}";
            filePath = $"{path}/{m_fileName}.txt";
            File.WriteAllText(filePath, m_target.ToJson());
        }
    }
}
