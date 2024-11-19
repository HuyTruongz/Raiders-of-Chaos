using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace hyhy.RaidersOfChaos
{
    public class SceneController : Singleton<SceneController>
    {
        public void LoandGamePlay()
        {
            SceneManager.LoadScene(GameScene.Gameplay.ToString());
            Time.timeScale = 1f;
        }

        public void LoanScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
            Time.timeScale = 1f;
        }
    }
}
