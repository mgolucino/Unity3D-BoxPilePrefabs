using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

namespace BoxPilePrefabs
{
    public class BoxPileGroup : MonoBehaviour
    {
        public GameObject replacement;
        public bool normalizeScale;

        public void ReplaceBoxes(GameObject replacement)
        {
            int childCount = transform.childCount;

            for (int i = 0; i < childCount; i ++)
            {
                Transform current = transform.GetChild(i);

                BoxPile boxPile = current.GetComponent<BoxPile>();

                if (boxPile != null)
                {
                    boxPile.replacement = replacement;
                    boxPile.normalizeScale = normalizeScale;

                    boxPile.ReplaceBoxes();
                }

                else
                {
                    string objectName = current.gameObject.name;

                    Debug.Log("Could not replace " + objectName + ". This object is missing a <Box Pile> component.");
                }
            }
        }

        public void ResetBoxes()
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

            ReplaceBoxes(cube);

            DestroyImmediate(cube);
        }
    }
}

