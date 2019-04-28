using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Mortal
{
    public class BaseGenerator : MonoBehaviour
    {
        public GameObject plate;
        public int seed = 1;
        public int probability = 50;
        public uint positionChangeMod = 3;
        public float sectionOffset = 0.45f;
        public int prewarmSections = 20;

        protected System.Random prng;
        protected Queue<GameObject> pool = new Queue<GameObject>();
        protected Queue<GameObject> active = new Queue<GameObject>();
        protected Vector2 offCenter;
        protected Vector2 newOffCenter;
        protected int sectionsSpawned = 0;

        protected virtual void Init()
        {
            pool = new Queue<GameObject>();
            active = new Queue<GameObject>();
            sectionsSpawned = 0;
            ClearChildren();
            prng = new System.Random(seed);
            offCenter = Vector2.zero;
        }

        protected void FillOffCenter(int i)
        {
            newOffCenter = offCenter;
            if (i >= prewarmSections && i % positionChangeMod == 0)
            {
                var nx = prng.Next(0, 100);
                if (nx <= probability)
                {
                    newOffCenter.x += nx >= probability / 2 ? sectionOffset : -sectionOffset;
                }
                nx = prng.Next(0, 100);
                if (nx <= probability)
                {
                    newOffCenter.y += nx >= probability / 2 ? sectionOffset : -sectionOffset;
                }

            }
        }

        protected virtual void ClearChildren()
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

        protected virtual void CreateGrid()
        {
        }
    }
}
