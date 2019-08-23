using System;
using UnityEngine;

namespace Demo
{
    [CreateAssetMenu(menuName = "Tools/PankapuConfig")]
    [Serializable]
    public class PankapuConfig :ScriptableObject
    {
        public float walkSpeed;
        public float jumpForce;
        public float pushSpeed;
        public float swimSpeed;
        public float climbSpeed;

        public LayerMask ground;
        public LayerMask water;
        public LayerMask pankapu;
        public LayerMask monster;

        private string playerInfoJsonPath;
        private void OnEnable()
        {
            playerInfoJsonPath = Application.persistentDataPath + "Pankapu.json";
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
