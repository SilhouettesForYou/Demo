using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Demo
{
    public class ListView : MonoBehaviour
    {
        public int ItemCount
        {
            get { return items.Count; }
        }

        public Action<GameObject> RemoveMethod = item => Destroy(item);

        [SerializeField]
        private ScrollRect scrollRect;
        [SerializeField]
        private HorizontalOrVerticalLayoutGroup content;

        [SerializeField]
        private Layout layout = Layout.Vertical;

        private float spacing = 50f;



        private float itemLength = -1;
        private List<GameObject> items = new List<GameObject>();

        private void Awake()
        {
            InitLayout();
            content.spacing = spacing;
        }

        private void Start()
        {
            CalcContentLength();
        }

        private void OnValidate()
        {
            InitLayout();

            content.spacing = spacing;

            CalcContentLength();
        }

        public void AddItem(GameObject item)
        {
            item.transform.SetParent(content.transform);
            items.Insert(0, item);
            item.transform.SetAsFirstSibling();
            AdjustContentLength(item.transform, 1);
        }

        private void InitLayout()
        {
            var contentV = GetComponentInChildren<VerticalLayoutGroup>(true);
            var contentH = GetComponentInChildren<HorizontalLayoutGroup>(true);

            scrollRect = GetComponent<ScrollRect>();

            if (layout == Layout.Vertical)
            {
                scrollRect.vertical = true;
                scrollRect.horizontal = false;
                scrollRect.content = contentV.transform as RectTransform;

                contentV.gameObject.SetActive(true);
                contentH.gameObject.SetActive(false);
                content = contentV;
            }
            else
            {
                scrollRect.vertical = false;
                scrollRect.horizontal = true;
                scrollRect.content = contentH.transform as RectTransform;

                contentV.gameObject.SetActive(false);
                contentH.gameObject.SetActive(true);
                content = contentH;
            }
        }

        private void CalcContentLength()
        {
            float contentLength = 0;

            int itemCount = content.transform.childCount;
            contentLength += (itemCount - 1) * spacing;

            foreach (var obj in content.transform)
            {
                RectTransform rect = obj as RectTransform;

                if (layout == Layout.Vertical)
                {
                    contentLength += rect.sizeDelta.y;
                }
                else
                {
                    contentLength += rect.sizeDelta.x;
                }
            }

            RectTransform contentRect = content.transform as RectTransform;
            Vector2 contentSizeDelta = contentRect.sizeDelta;
            if (layout == Layout.Vertical)
            {
                contentSizeDelta.y = contentLength;
            }
            else
            {
                contentSizeDelta.x = contentLength;
            }
            contentRect.sizeDelta = contentSizeDelta;
        }

        private void AdjustContentLength(Transform itemTrans, int power)
        {
            if (itemLength < 0)
            {
                RectTransform rect = itemTrans as RectTransform;
                if (layout == Layout.Vertical)
                {
                    itemLength = rect.sizeDelta.y;
                }
                else
                {
                    itemLength = rect.sizeDelta.x;
                }
            }

            power = power < 0 ? -1 : 1;

            RectTransform contentRect = content.transform as RectTransform;
            Vector2 contentSizeDelta = contentRect.sizeDelta;
            if (layout == Layout.Vertical)
            {
                contentSizeDelta.y += (itemLength + spacing) * power;
            }
            else
            {
                contentSizeDelta.x += (itemLength + spacing) * power;
            }
            contentRect.sizeDelta = contentSizeDelta;
        }


        private enum Layout
        {
            Vertical, 
            Horizontal
        }
    }
}

