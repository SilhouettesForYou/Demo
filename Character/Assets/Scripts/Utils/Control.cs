using System;
using System.Collections.Generic;
using UnityEngine;
namespace Demo
{ 
    public class Control : MonoBehaviour
    {
        [HideInInspector]
        public AssistPlayerControl player;
        [HideInInspector]
        public event Action OnPlayerHurt;

        private List<Enemy> enemies = new List<Enemy>();

        void Awake()
        {
            Enemy[] m_Enemies = GetComponentsInChildren<Enemy>();
        }

        void Update()
        {
            foreach(var enemy in enemies)
            {
                if (enemy.CheckPlayerBeHurt(player))
                {
                    OnPlayerHurt?.Invoke();
                }
            }
        }
    }
}
