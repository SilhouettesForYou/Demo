using UnityEngine;

namespace Demo
{
    enum FocusOn { FocusOnPankapu, FocusOnAn, FocusOnPool, FocusOnSelf, FocusOnBoss }
    public class CameraFollow : MonoBehaviour
    {
        public float cameraSpeed = 1f;
        public float maxX = 32f;
        public float pankapuMinX = 0f;
        public float anMinX = 0f;
        public float poolMinX = 0f;
        private float moveSpeed = 0.5f;
        private float traslateSpeed = 1f;
        private float maxCameraSize = 20f;
        private float deltaYLevel1 = 3;
        private float deltaYLevel2 = 3;

        private Transform an;
        private Transform pankapu;
        private Transform pool;
        private Transform self;
        private Vector3 bossPostion;
        private Camera _camera;

        private FocusOn focus = Demo.FocusOn.FocusOnPankapu;
        private bool isAnDead = false;
        private bool isZoomOut = false;
        private bool isBossCamera = false;
        void Awake()
        {
            self = transform;
            _camera = transform.GetComponent<Camera>();
            _camera.orthographicSize = 10f;
            if (GameObject.FindGameObjectWithTag("BossCameraCenter") != null)
                bossPostion = GameObject.FindGameObjectWithTag("BossCameraCenter").transform.position;
        }
        // Start is called before the first frame update
        void Start()
        {
            if (FindObjectOfType<An>() != null)
                an = FindObjectOfType<An>().transform;
            if (FindObjectOfType<Pankapu>() != null)
                pankapu = FindObjectOfType<Pankapu>().transform;
            if (FindObjectOfType<UnderWater>() != null)
                pool = FindObjectOfType<UnderWater>().transform;

            if (an != null)
            {
                anMinX = an.transform.position.x;
            }
            if (pankapu != null)
            {
                pankapuMinX = pankapu.transform.position.x;
            }
            if (pool != null)
            {
                poolMinX = pool.transform.position.x;
            }

            EventCenter.AddListener<FocusOn>(EventType.FocusOn, ChangeFocusOn);
            EventCenter.AddListener(EventType.AnDeath, CheckAnDeath);
            EventCenter.AddListener(EventType.AnRespawn, CheckAnRespawn);
            EventCenter.AddListener(EventType.ZoomOut, ZoomOut);
            EventCenter.AddListener(EventType.BossCamera, BossCamera);
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 vec = transform.position;
            if (isZoomOut)
            {
                focus = Demo.FocusOn.FocusOnSelf;
                _camera.orthographicSize += Time.deltaTime;
            }

            if (isBossCamera)
            {
                focus = Demo.FocusOn.FocusOnBoss;
                if (_camera.orthographicSize < maxCameraSize)
                {
                    _camera.orthographicSize += moveSpeed;
                }
            }
            //focus = Demo.FocusOn.FocusOnAn;
            switch (focus)
            {
                case Demo.FocusOn.FocusOnPankapu:
                    //Debug.Log("Focus on pankapu.");
                    FocusOn(pankapu, vec, pankapuMinX);
                    break;
                case Demo.FocusOn.FocusOnAn:
                    //Debug.Log("Focus on an.");
                    FocusOn(an, vec, anMinX);
                    break;
                case Demo.FocusOn.FocusOnPool:
                    //Debug.Log("Focus on pankapu.");
                    FocusOn(pool, vec, poolMinX);
                    break;
                case Demo.FocusOn.FocusOnSelf:
                    FocusAll(self, vec);
                    break;
                case Demo.FocusOn.FocusOnBoss:
                    CameraTranslate(bossPostion);
                    break;
            }
        }

        void OnDestroy()
        {
            EventCenter.RemoveListener<FocusOn>(EventType.FocusOn, ChangeFocusOn);
            EventCenter.RemoveListener(EventType.AnDeath, CheckAnDeath);
            EventCenter.RemoveListener(EventType.AnRespawn, CheckAnRespawn);
            EventCenter.RemoveListener(EventType.ZoomOut, ZoomOut);
            EventCenter.RemoveListener(EventType.BossCamera, BossCamera);
        }
        private void FocusOn(Transform t, Vector3 vec, float minX)
        {
            if (isAnDead)
                traslateSpeed = 1.5f;
            else
                traslateSpeed = 5f;
            vec.x = Mathf.Lerp(vec.x, t.position.x, Time.deltaTime * traslateSpeed);
            if (CanvasControl.sceneName.Equals("Level-1"))
                vec.y = Mathf.Lerp(vec.y, t.position.y + deltaYLevel1, Time.deltaTime * cameraSpeed);
            if (CanvasControl.sceneName.Equals("Level-2"))
                vec.y = Mathf.Lerp(vec.y, t.position.y + deltaYLevel2, Time.deltaTime * cameraSpeed);
            transform.position = vec;
        }

        private void FocusAll(Transform t, Vector3 vec)
        {
            if (isAnDead)
                traslateSpeed = 1.5f;
            else
                traslateSpeed = 5f;
            vec.x = Mathf.Lerp(vec.x, t.position.x, Time.deltaTime * traslateSpeed);
            vec.y = Mathf.Lerp(vec.y, t.position.y, Time.deltaTime * cameraSpeed);
            transform.position = vec;
        }

        private void CameraTranslate(Vector3 destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * 2);
        }

        private void ChangeFocusOn(FocusOn focus)
        {
            this.focus = focus;
        }

        private void CheckAnDeath()
        {
            isAnDead = true;
        }
        private void CheckAnRespawn()
        {
            isAnDead = false;
        }

        private void ZoomOut()
        {
            isZoomOut = true;
        }

        private void BossCamera()
        {
            isBossCamera = true;
        }
    }
}

