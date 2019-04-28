using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Mortal
{
    [System.Serializable]
    public struct CellGridDefinition
    {
        public GameObject cellGo;
        public Vector2Int gridPosition;
    }

    public class SelectionGrid : MonoBehaviour
    {
#if UNITY_EDITOR
        public Vector2Int gridSize = new Vector2Int(6, 6);
        public GameObject plate;

        void ClearChildren()
        {
            for (int i = transform.childCount; i > 0; i--)
            {
                if (EditorApplication.isPlaying)
                {
                    Destroy(transform.GetChild(i - 1).gameObject);
                }
                else
                {
                    DestroyImmediate(transform.GetChild(i - 1).gameObject);
                }
            }
        }

        [SerializeField, InspectorButton("CreateGrid")]
        protected string createGrid;


        void CreateGrid()
        {
            ClearChildren();
            float yoffset = 0;// gridSize.y;
            float xoffset = gridSize.x * 0.5f;
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    var go = Instantiate(plate);
                    go.transform.parent = transform;
                    go.transform.localPosition = new Vector3(-xoffset + x, 0, yoffset + y);
                }
            }
        }
#endif
    }
}
