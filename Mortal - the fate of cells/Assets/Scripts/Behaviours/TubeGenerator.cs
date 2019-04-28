using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Mortal
{
    public class TubeGenerator : BaseGenerator
    {

        public uint sectionsCount = 50;
        [Range(4, 20)]
        public uint platesPerSection = 10;
        public float radius = 1.0f;
        public uint sectionsToCompleteMission = 500;
        public uint totalCheckpointsToPass = 10;
        public GameObject powerUpGO;
        Player player;
        GameObject sectionTemplate;

        int blockedCount = 0;

        protected override void Init()
        {
            base.Init();
            blockedCount = 0;
            sectionTemplate = null;
        }

        void Start()
        {
            CreateGrid();
            player = Player.PL;
            prng = new System.Random(seed);
        }
        
        void Update()
        {
            var section = active.Peek();
            if (section.transform.position.z + 5 < player.transform.position.z)
            {
                pool.Enqueue(active.Dequeue());
                NextSection();
            }
        }
        
        GameObject GetTemplateInstance()
        {
            if (sectionTemplate == null)
            {
                sectionTemplate = new GameObject("Section");
                for (int i = 0; i < platesPerSection; i++)
                {
                    float currSection = (float)i / (float)platesPerSection;
                    float x = Mathf.Sin(2 * Mathf.PI * currSection) * radius;
                    float y = Mathf.Cos(2 * Mathf.PI * currSection) * radius;
                    var go = Instantiate(plate);
                    go.transform.parent = sectionTemplate.transform;
                    var pos = new Vector3(x, y, 0);
                    go.transform.localPosition = pos;
                    Vector3 targetDir = new Vector3(0, 0, 0) - pos;
                    float angle = Vector3.SignedAngle(targetDir, Vector3.up, Vector3.forward);
                    go.transform.Rotate(Vector3.forward, -angle);
                    

                }
                sectionTemplate.SetActive(false);
                sectionTemplate.transform.parent = transform;
            }
            if (pool.Count > 0)
            {
                return pool.Dequeue();
            }
            return Instantiate(sectionTemplate);
        }
        
        void CreateSection(int depth, Vector2 offCenter = default, Vector2 newOffCenter = default)
        {
            bool block = offCenter != newOffCenter;
            if (block)
                blockedCount++;
            GameObject sectionGo = GetTemplateInstance();
            sectionGo.SetActive(true);
            sectionGo.transform.parent = transform;
            sectionGo.transform.localPosition = new Vector3(newOffCenter.x, newOffCenter.y, depth);
            if (block)
                sectionGo.transform.localScale = new Vector3(2, 2, 4);
            else
                sectionGo.transform.localScale = Vector3.one;
            sectionGo.transform.localRotation = Quaternion.Euler(0, 0, 360*(Mathf.Sin(((float)depth/ positionChangeMod) * Mathf.PI)));
            active.Enqueue(sectionGo);
        }
        

        void NextSection()
        {
            int i = sectionsSpawned;
            newOffCenter = offCenter;
            int usablesections = (int)sectionsToCompleteMission - prewarmSections;
            if (i < sectionsToCompleteMission && i >= prewarmSections)
            {
                FillOffCenter(i);

                if (i % (usablesections / totalCheckpointsToPass) == 0 || i == sectionsToCompleteMission - 1)
                {
                    var pupGO = Instantiate(powerUpGO);
                    pupGO.transform.parent = transform;
                    pupGO.transform.localPosition = new Vector3(offCenter.x, offCenter.y, i);
                    var pu = pupGO.AddComponent<BloodMissionPowerUp>();
                    pu.IsFinal = i == sectionsToCompleteMission - 1;
                    if (pu.IsFinal)
                    {
                        pupGO.transform.localScale = Vector3.one * 2.5f;
                    } else
                    {
                        pupGO.transform.localScale = Vector3.one * 1.5f;
                    }
                }
            }
            CreateSection(i, offCenter, newOffCenter);

            offCenter = newOffCenter;
            sectionsSpawned++;
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
