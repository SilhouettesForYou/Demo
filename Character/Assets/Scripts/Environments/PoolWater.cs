using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class PoolWater : MonoBehaviour
    {

        public delegate void TurnOutfallOff();
        public static event TurnOutfallOff turnOff;

        private bool done;
        private bool isTrigger;
        private bool isLeak;

        private Transform waterBackground;
        private Transform water;
        private float pixelsPerUnitBackground;
        private float pixelsPerUnitWater;
        private float widthOfBackground;
        private float heightOfBackground;
        private float widthOfWater;
        private float heightOfWater;
        private float yOfBackgroundBottom;
        private float yOfWaterBottom;
        private Vector3 scaleOfBackground;
        private Vector3 scaleOfWater;
        public float deltaOfBackgroundScale = 1f;
        public float deltaOfWaterScale = 2f;

        // Start is called before the first frame update
        void Start()
        {
            // delegate
            MammothControl.turnOn += TurnOutfallOn;

            done = false;
            isTrigger = false;
            Transform p = transform.Find("Water");
            waterBackground = p.Find("W");
            water = p.Find("Bottom");

            // Find the ancher of water background.
            pixelsPerUnitBackground = waterBackground.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
            widthOfBackground = waterBackground.GetComponent<SpriteRenderer>().sprite.rect.width / pixelsPerUnitBackground;
            heightOfBackground = waterBackground.GetComponent<SpriteRenderer>().sprite.rect.height / pixelsPerUnitBackground;
            widthOfBackground *= waterBackground.localScale.x;
            heightOfBackground *= waterBackground.localScale.y;
            scaleOfBackground = waterBackground.localScale;

            // Find the ancher of water bottom.
            pixelsPerUnitWater = water.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
            widthOfWater = water.GetComponent<SpriteRenderer>().sprite.rect.width / pixelsPerUnitWater;
            heightOfWater = water.GetComponent<SpriteRenderer>().sprite.rect.height / pixelsPerUnitWater;
            widthOfWater *= water.localScale.x;
            heightOfWater *= water.localScale.y;
            scaleOfWater = water.localScale;

            // not use
            //SetAncher(waterBackground, widthOfBackground, heightOfBackground);
            //SetAncher(water, widthOfWater, heightOfWater);

            // scale the water background
            waterBackground.localScale = new Vector3(waterBackground.localScale.x, 0, waterBackground.localScale.z);
            yOfBackgroundBottom = waterBackground.position.y - heightOfBackground / 2;
            waterBackground.position = new Vector3(waterBackground.position.x, yOfBackgroundBottom, waterBackground.position.z);
            yOfBackgroundBottom = waterBackground.localPosition.y;

            // scale the water bottom
            water.localScale = new Vector3(water.localScale.x, 0, water.localScale.z);
            yOfWaterBottom = water.position.y - heightOfWater / 2;
            water.position = new Vector3(water.position.x, yOfWaterBottom, water.localScale.z);
            yOfWaterBottom = water.localPosition.y;
        }

        // Update is called once per frame
        void Update()
        {
            if (isTrigger)
            {
                SurfaceUp(waterBackground, scaleOfBackground, deltaOfBackgroundScale, yOfBackgroundBottom);
                SurfaceUp(water, scaleOfWater, deltaOfWaterScale, yOfWaterBottom);
                SetAncher(water, widthOfWater, heightOfWater);
            }
            if (done)
            {
                turnOff();
            }
        }

        private void SetAncher(Transform parent, float width, float height)
        {
            Transform leftTop = parent.Find("AncherLeftTop");
            Transform rightTop = parent.Find("AncherRightTop");
            Transform leftBottom = parent.Find("AncherLeftBottom");
            Transform rightBottom = parent.Find("AncherRightBottom");

            leftTop.position = new Vector3(parent.position.x - width / 2, parent.position.y - height / 2, parent.position.z);
            rightTop.position = new Vector3(parent.position.x + width / 2, parent.position.y - height / 2, parent.position.z);
            leftBottom.position = new Vector3(parent.position.x - width / 2, parent.position.y + height / 2, parent.position.z);
            rightBottom.position = new Vector3(parent.position.x + width / 2, parent.position.y + height / 2, parent.position.z);
        }

        private void SurfaceUp(Transform t, Vector3 originalScale, float delta, float yOfBottom)
        {
            Vector3 updatedScale = t.localScale;
            if (updatedScale.y < originalScale.y)
            {
                updatedScale.y += Time.deltaTime * delta;
                float y = yOfBottom + (heightOfBackground / originalScale.y) * updatedScale.y / 2;
                Vector3 pos = new Vector3(t.localPosition.x, y, t.localPosition.z);
                t.localPosition = pos;
                t.localScale = updatedScale;
            }
            else
            {
                done = true;
            }
        }

        private void SurfaceDown(Transform t, Vector3 originalScale, float delta, float yOfBottom)
        {

        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(5.0f);
        }

        private void TurnOutfallOn()
        {
            isTrigger = true;
        }

    }
}
