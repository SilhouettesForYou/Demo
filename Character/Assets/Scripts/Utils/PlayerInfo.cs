﻿using System;
using UnityEngine;

namespace Demo
{
    [CreateAssetMenu(menuName ="Tools/PlayerInfo")]
    [Serializable]
    public class PlayerInfo : ScriptableObject
    {
        public string playerName;
        public float maxSpeed;
        public float jumpForce;
        public float dragOrPushSpeed;

        [Range(0, 1)]
        public float crouchSpeedFactor;
        public float climbSpeed;

        public LayerMask ground;
        public LayerMask ladder;
        public LayerMask player;
        public LayerMask water;

        private string playerInfoJsonPath;

        private void OnEnable()
        {
            playerInfoJsonPath = Application.persistentDataPath + "PlayerInfo_" + playerName + ".json";
            if (System.IO.File.Exists(playerInfoJsonPath))
            {
                JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(playerInfoJsonPath), this);
            }
            else
            {
                System.IO.File.WriteAllText(playerInfoJsonPath, "");
            }
        }

        private void OnDisable()
        {
            System.IO.File.WriteAllText(playerInfoJsonPath, JsonUtility.ToJson(this, true));
        }
    }
}
