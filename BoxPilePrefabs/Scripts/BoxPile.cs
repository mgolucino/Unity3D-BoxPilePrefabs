using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

namespace BoxPilePrefabs
{
    public class BoxPile : MonoBehaviour
    {
        public GameObject replacement;
        public bool normalizeScale;

        private List<Transform> currentChildren = new List<Transform>();

        public void ReplaceBoxes()
        {
            if (replacement.transform.parent != gameObject.transform)
            {
                if (ValidBoxPile())
                    Replace();
            }

            else
            {
                Debug.Log("Could not replace " + gameObject.name + ". The replacement object must not be a child of this object.");
            }
        }

        public void ResetBoxes()
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

            GameObject temp = replacement;
            replacement = cube;

            ReplaceBoxes();

            // ================================================

            replacement = temp;
            normalizeScale = false;

            DestroyImmediate(cube);
        }

        private bool ValidBoxPile()
        {
            bool valid = true;
            int childCount = transform.childCount;

            string common = "Could not replace " + gameObject.name + ". ";
            string name = "";

            for (int i = 0; i < childCount; i ++)
            {
                BoxCollider boxCollider = transform.GetChild(i).GetComponent<BoxCollider>();

                name = transform.GetChild(i).gameObject.name;

                if (boxCollider != null)
                {
                    Vector3 center = boxCollider.center;
                    Vector3 size = boxCollider.size;

                    bool centeredHorizontal = Mathf.Approximately(center.x, 0f) && Mathf.Approximately(center.z, 0f);
                    bool centeredVertical = Mathf.Approximately(center.y, 0f) || Mathf.Approximately(center.y, size.y / 2.0f);

                    bool centered = centeredHorizontal && centeredVertical;

                    bool squareFaces = Mathf.Approximately(size.x, size.y) && Mathf.Approximately(size.y, size.z);

                    if (!centered || !squareFaces)
                    {
                        if (!centered)
                            Debug.Log(common + "All children on this object must have centered box colliders. Please check the box collider on " + name + ".");

                        if (!squareFaces)
                            Debug.Log(common + "All children on this object must have box colliders with equal dimensions. Please check the box collider on " + name + ".");

                        valid = false;
                        break;
                    }
                }

                else
                {
                    Debug.Log(common + "All children on this object must have a box collider. Please add a box collider to " + name + ".");

                    valid = false;
                    break;
                }
            }

            return valid;
        }

        private void Replace()
        {
            #if UNITY_EDITOR

            if (PrefabUtility.IsOutermostPrefabInstanceRoot(gameObject))
            {
                PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.Completely, InteractionMode.UserAction);
            }

            #endif

            // =====================================================================================

            currentChildren.Clear();

            int childCount = transform.childCount;

            for (int i = 0; i < childCount; i ++)
            {
                currentChildren.Add(transform.GetChild(i));
            }

            // =====================================================================================
            
            Vector3 originalScale = transform.localScale;
            
            Transform parent = transform.parent;
            int siblingIndex = transform.GetSiblingIndex();

            transform.SetParent(null);
            transform.localScale = Vector3.one / transform.GetChild(0).localScale.x;

            for (int i = 0; i < childCount; i ++)
            {
                currentChildren[i].SetParent(null);
                currentChildren[i].localScale = Vector3.one;
            }

            transform.localScale = Vector3.one;

            for (int i = 0; i < childCount; i ++)
            {
                currentChildren[i].SetParent(gameObject.transform);
            }

            // =====================================================================================

            BoxCollider currentCollider = transform.GetChild(0).GetComponent<BoxCollider>();
            BoxCollider replacementCollider = replacement.GetComponent<BoxCollider>();

            float currentColliderSize = currentCollider.size.x;
            float replacementColliderSize = replacementCollider.size.x;

            bool currentCenteredOrigin = false;
            bool replacementCenteredOrigin = false;

            if (currentCollider.center == Vector3.zero)
                currentCenteredOrigin = true;

            if (replacementCollider.center == Vector3.zero)
                replacementCenteredOrigin = true;

            // =====================================================================================

            GameObject temp = GameObject.Instantiate(gameObject);

            temp.transform.localScale = Vector3.one * (replacementColliderSize / currentColliderSize);

            // =====================================================================================

            for (int i = 0; i < childCount; i ++)
            {
                Transform currentChild = temp.transform.GetChild(0);
                Transform newChild = Instantiate(replacement.transform, currentChild.position, currentChild.rotation, temp.transform);

                newChild.localScale = Vector3.one * (currentColliderSize / replacementColliderSize);

                // ==========================================================

                if (currentCenteredOrigin != replacementCenteredOrigin)
                {
                    int sign = 1;

                    if (currentCenteredOrigin)
                        sign = -1;

                    float x = newChild.localPosition.x;
                    float y = newChild.localPosition.y + currentColliderSize / 2.0f * sign;
                    float z = newChild.localPosition.z;

                    newChild.localPosition = new Vector3(x, y, z);
                }

                DestroyImmediate(currentChild.gameObject);
            }

            // =====================================================================================

            if (normalizeScale)
                temp.transform.localScale /= replacementColliderSize;

            for (int i = 0; i < childCount; i ++)
            {
                Transform newChild = temp.transform.GetChild(0);

                newChild.SetParent(gameObject.transform);
                newChild.gameObject.name = replacement.name;
                newChild.gameObject.SetActive(true);

                if (i > 0)
                    newChild.gameObject.name += " (" + i + ")";

                DestroyImmediate(transform.GetChild(0).gameObject);
            }

            // =====================================================================================

            DestroyImmediate(temp);

            if (parent != null)
            {
                transform.SetParent(parent);
                transform.SetSiblingIndex(siblingIndex);
            }
            
            transform.localScale = originalScale;
        }
    }
}

