using UnityEngine;
using UnityEngine.EventSystems;

namespace Demo
{
    public class UIEventListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        // define a event delegate
        public delegate void UIEventProxy(GameObject gameObject);

        public event UIEventProxy OnKeyDown;
        public event UIEventProxy OnKeyUp;
        

        public void OnPointerUp(PointerEventData eventData)
        {
            OnKeyUp?.Invoke(this.gameObject);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnKeyDown?.Invoke(this.gameObject);
        }

    }
}
