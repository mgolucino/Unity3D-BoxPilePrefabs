#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace BoxPilePrefabs
{
    [CustomEditor(typeof(BoxPileGroup))]
    class BoxPileGroupEditor : Editor
    {
        private BoxPileGroup component;

        private void OnEnable()
        {
            component = (BoxPileGroup) target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if(GUILayout.Button("Replace Boxes"))
            {
                GameObject replacement = component.replacement;

                if (replacement != null)
                {
                    BoxCollider boxCollider = replacement.GetComponent<BoxCollider>();

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
                                Debug.Log("Could not start the replacement process. The replacement object must have a centered box collider.");

                            if (!squareFaces)
                                Debug.Log("Could not start the replacement process. The replacement object must have a box collider with equal dimensions.");
                        }

                        else
                        {
                            component.ReplaceBoxes(component.replacement);
                        }
                    }

                    else
                    {
                        Debug.Log("Could not start the replacement process. The replacement object must have a box collider.");
                    }
                }

                else
                {
                    Debug.Log("Could not start the replacement process. The replacement object must not be null.");
                }
            }

            if(GUILayout.Button("Reset Boxes"))
            {
                component.normalizeScale = false;
                component.ResetBoxes();
            }
        }
    }
}

#endif




