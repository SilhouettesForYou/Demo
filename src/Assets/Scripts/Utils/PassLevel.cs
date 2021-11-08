using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Demo
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PassLevel : BaseUI
    {
        public static string level;
        private void Reset()
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            An an = collision.GetComponent<An>();
            if (an != null)
            {
                level = transform.name;
                UIManager.lastUIName = "GameUI";
                UIManager.lastLevelName = level;
                if (level.Equals("Level-1"))
                    LevelPassInfo.levelOnePass = true;
                if (level.Equals("Level-2"))
                    LevelPassInfo.levelTwoPass = true;
                SceneManager.LoadScene(0);
                AudioManager.PlayEffect("MusicVictory");
                Close();
            }
        }
    }
}
