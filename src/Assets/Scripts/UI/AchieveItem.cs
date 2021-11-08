using UnityEngine;
using UnityEngine.UI;

namespace Demo
{
    public class AchieveItem : MonoBehaviour
    {
        private Image image;
        private Text condition;
        private Text title;
        private Text label;

        public void SetData(Sprite img, string t, string c, string l)
        {
            image.overrideSprite = img;
            condition.text = c;
            title.text = t;
            label.text = l;
        }

        void Awake()
        {
            image = UnityHelper.Find(transform, "LevelImage").GetComponent<Image>();
            condition = UnityHelper.Find(transform, "AchieveCondition").GetComponent<Text>();
            title = UnityHelper.Find(transform, "AchieveTitle").GetComponent<Text>();
            label = UnityHelper.Find(transform, "Status").GetComponent<Text>();
        }
    }
}
