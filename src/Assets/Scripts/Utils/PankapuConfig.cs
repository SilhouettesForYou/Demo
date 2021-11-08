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
        public float jumpVelocity;
        public float pushSpeed;
        public float swimSpeed;
        public float climbSpeed;
        public float jumpTime;

        public LayerMask an;
        public LayerMask ground;
        public LayerMask water;
        public LayerMask pankapu;
        public LayerMask monster;
        public LayerMask pushable;
        public LayerMask movingPlatform;

        public static float height { get; set; }
        public static float width { get; set; }

        public static Vector2 ComputeRect(Transform transform)
        {
            float pixelsPerUnit = transform.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
            float width = transform.GetComponent<SpriteRenderer>().sprite.rect.width / pixelsPerUnit;
            float height = transform.GetComponent<SpriteRenderer>().sprite.rect.height / pixelsPerUnit;
            width *= transform.localScale.x;
            height *= transform.localScale.y;
            return new Vector2(width, height);
        }

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
