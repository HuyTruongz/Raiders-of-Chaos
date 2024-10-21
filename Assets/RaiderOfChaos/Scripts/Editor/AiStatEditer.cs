using hyhy.RaidersOfChaos.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace hyhy.RaidersOfChaos.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AIStat),editorForChildClasses: true)]
    public class AiStatEditer : ActorStatEditor
    {
        public override void Upgrade()
        {
            Load(m_fileName);
            m_target.UpGrade();
        }
    }
}
