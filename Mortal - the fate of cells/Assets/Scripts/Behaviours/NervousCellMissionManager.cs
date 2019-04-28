using UnityEngine;

namespace Mortal
{
    public class NervousCellMissionManager : MissionManager
    {
        bool gotToLast = false;
        float maxOffset = .35f;

        protected override void Start()
        {
            base.Start();
            NeuronMissionLastPlateTrigger.gotUser = () => {
                gotToLast = true;
                ProcessEndMission();
            };
        }

        protected override void SetupGenerator()
        {

            SkiesStairsGenerator gen = (SkiesStairsGenerator)generator;
            var stats = GameManager.GM.GetStats(Type);
            gen.seed = stats.missionsWon + 1;

            float currentDifficulty = (float)stats.missionsWon / missionsHardnessInterpolationCount;
            currentDifficulty = Mathf.Clamp(currentDifficulty, 0, 1);
            gen.sectionOffset += (currentDifficulty * maxOffset);
            gen.probability = Mathf.RoundToInt(10 + 80 * currentDifficulty);
            gen.sectionsCount += Mathf.RoundToInt(20 * currentDifficulty);
            timeToCompleteMission = (90.0f - 20.0f * currentDifficulty);
        }

        protected override bool IsSuccess()
        {
            return base.IsSuccess() && gotToLast;
        }
    }
}
