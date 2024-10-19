using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace hyhy.RaidersOfChaos
{
    public class Dialog : MonoBehaviour
    {
        public Text titleTxt;
        public Text contentTxt;

        public virtual void Show(bool isShow)
        {
            gameObject.SetActive(isShow);
        }

        public virtual void UpdateDialog(string title, string content)
        {
            if (titleTxt)
            {
                titleTxt.text = title;
            }
            if (contentTxt)
            {
                contentTxt.text = content;
            }
        }

        public virtual void UpdateDialog()
        {

        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
