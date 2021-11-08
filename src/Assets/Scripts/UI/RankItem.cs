using UnityEngine;
using UnityEngine.UI;

namespace Demo
{
    public class RankItem : MonoBehaviour
    {
        private Image image;
        private Text rank;
        private Text id;
        private Text time;

        public void SetData(Image img, string r, string i, string t)
        {
            image = img;
            rank.text = r;
            id.text = i;
            time.text = t;
        }

        void Awake()
        {
            image = UnityHelper.Find(transform, "UserImage").GetComponent<Image>();
            rank = UnityHelper.Find(transform, "Text").GetComponent<Text>();
            id = UnityHelper.Find(transform, "ID").GetComponent<Text>();
            time = UnityHelper.Find(transform, "Time").GetComponent<Text>();
        }
    }
}
