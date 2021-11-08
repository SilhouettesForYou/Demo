using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace Demo
{
    public class ScrollLevelCard : BaseUI, IBeginDragHandler, IEndDragHandler
    {
        private float smooting = 5;                          //滑动速度
        private float pageCount = 3;                           //每页显示的项目

        private ScrollRect scrollRect;
        private RectTransform levelItems;

        private float pageIndex;                            //总页数
        private bool isDrag = false;                                //是否拖拽结束
        private List<float> listPageValue = new List<float> { 0 };  //总页数索引比列 0-1


        private float targetPos = 0;                                //滑动的目标位置

        private float nowindex = 0;                                 //当前位置索引
        private float beginDragPos;
        private float endDragPos;

        private float sensitivity = 0.95f;                          //灵敏度

        void Awake()
        {
            Time.timeScale = 1;
            scrollRect = transform.GetComponent<ScrollRect>();
            levelItems = scrollRect.content;
            ListPageValueInit();
            ButtonInit();
        }

        //每页比例
        void ListPageValueInit()
        {
            pageIndex = Mathf.CeilToInt((levelItems.childCount / pageCount)) - 1;
            if (levelItems != null && levelItems.transform.childCount != 0)
            {
                for (float i = 1; i <= pageIndex; i++)
                {
                    listPageValue.Add((i / pageIndex));
                }
            }
        }

        void ButtonInit()
        {
            RegisterBtnOnClick("GoLeft", BtnLeftGo);
            RegisterBtnOnClick("GoRight", BtnRightGo);
        }

        void Update()
        {
            if (!isDrag)
            {
                scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition,
                    targetPos, Time.deltaTime * smooting);
            }
                
        }

        /// <summary>
        /// 拖动开始
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            isDrag = true;
            beginDragPos = scrollRect.horizontalNormalizedPosition;
        }

        /// <summary>
        /// 拖拽结束
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            isDrag = false;
            endDragPos = scrollRect.horizontalNormalizedPosition; //获取拖动的值
            endDragPos = endDragPos > beginDragPos ? endDragPos + sensitivity : endDragPos - sensitivity;
            int index = 0;
            float offset = Mathf.Abs(listPageValue[index] - endDragPos);    //拖动的绝对值
            for (int i = 1; i < listPageValue.Count; i++)
            {
                float temp = Mathf.Abs(endDragPos - listPageValue[i]);
                if (temp < offset)
                {
                    index = i;
                    offset = temp;
                }
            }
            targetPos = listPageValue[index];
            nowindex = index;
        }


        public void BtnLeftGo(GameObject go)
        {
            nowindex = Mathf.Clamp(nowindex - 1, 0, pageIndex);
            targetPos = listPageValue[Convert.ToInt32(nowindex)];
        }

        public void BtnRightGo(GameObject go)
        {
            nowindex = Mathf.Clamp(nowindex + 1, 0, pageIndex);
            targetPos = listPageValue[Convert.ToInt32(nowindex)];
        }
    }
}
