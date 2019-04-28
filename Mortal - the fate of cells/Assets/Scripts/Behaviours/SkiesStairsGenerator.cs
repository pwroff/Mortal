using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mortal
{
    public class SkiesStairsGenerator : BaseGenerator
    {
        public int sectionsCount = 10;
        public GameObject bloodCellPrefab;

        GameObject GetPlateGo(int depth)
        {
            GameObject go;
            if (pool.Count > 0)
            {
                go = pool.Dequeue();
            } else
            {
                go = Instantiate(plate);
            }
            go.transform.parent = transform;
            go.transform.localPosition = new Vector3(depth, newOffCenter.x,  newOffCenter.y);
            go.SetActive(true);
            return go;
        }

        private void Start()
        {
            CreateGrid();
        }

        void NextSection()
        {
            FillOffCenter(sectionsSpawned);
            var go = GetPlateGo(sectionsSpawned);
            active.Enqueue(go);
            sectionsSpawned++;
            offCenter = newOffCenter;
            if (sectionsSpawned == sectionsCount)
            {
                
                var b = Instantiate(bloodCellPrefab);
                b.transform.parent = go.transform;
                b.transform.localPosition = new Vector3(0, 0.25f, 0);
                b.transform.localScale = Vector3.one * 0.25f;
                var highlighter = go.GetComponentInChildren<CollisionHighlights>();
                highlighter.gameObject.AddComponent<NeuronMissionLastPlateTrigger>();

            }
        }

        protected override void CreateGrid()
        {
            Init();
            for (int i = 0; i < sectionsCount; i++)
            {
                NextSection();
            }
        }
    }
}
