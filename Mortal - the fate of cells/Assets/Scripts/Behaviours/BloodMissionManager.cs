using TMPro;
using UnityEngine;

namespace Mortal
{
    [System.Serializable]
    public class BloodMissionsExtrems
    {
        public float maxPowerUps = 20.0f;
        public float maxPowerupsExtrapolation = 2.0f;
        public float maxOffsetDisplacement = .5f;
    }
    public class BloodMissionManager : MissionManager
    {
        public TextMeshProUGUI oxigenDelivered;
        int totalPowerupsCollected = 0;
        int minPowerupsToWin = 5;
        public AudioSource powerUpSource;

        public BloodMissionsExtrems missionExtrems;

        protected override void Awake()
        {
            base.Awake();
            BloodMissionPowerUp.OnTrigger += HandlePowerUpTrigger;
        }

        protected override void SetupGenerator()
        {

            TubeGenerator gen = (TubeGenerator)generator;
            var stats = GameManager.GM.GetStats(Type);
            gen.seed = stats.missionsWon + 1;

            float currentDifficulty = (float)stats.missionsWon / missionsHardnessInterpolationCount;
            currentDifficulty = Mathf.Clamp(currentDifficulty, 0, 1);

            gen.positionChangeMod = (uint)(50 - Mathf.RoundToInt(40 * currentDifficulty));
            gen.probability = Mathf.RoundToInt(10 + 80 * currentDifficulty);
            gen.sectionOffset += 0.5f * currentDifficulty;
            minPowerupsToWin = Mathf.RoundToInt(currentDifficulty * missionExtrems.maxPowerUps + minPowerupsToWin);
            int powerupsExtrapolation = Mathf.RoundToInt((1 - currentDifficulty)* missionExtrems.maxPowerupsExtrapolation * minPowerupsToWin + minPowerupsToWin);
            gen.totalCheckpointsToPass = (uint)powerupsExtrapolation;
            timeToCompleteMission = (90.0f - 30.0f * currentDifficulty);
            oxigenDelivered.text = string.Format("Oxygen Delivered: {0}/{1}", totalPowerupsCollected, minPowerupsToWin);
        }

        private void HandlePowerUpTrigger(BloodMissionPowerUp powerup)
        {
            totalPowerupsCollected++;
            powerUpSource?.Play();
            oxigenDelivered.text = string.Format("Oxygen Delivered: {0}/{1}", totalPowerupsCollected, minPowerupsToWin);
            if (powerup.IsFinal)
            {
                ProcessEndMission();
            }
        }

        private void OnDestroy()
        {
            BloodMissionPowerUp.OnTrigger -= HandlePowerUpTrigger;
        }

        protected override bool IsSuccess()
        {
            return totalPowerupsCollected >= minPowerupsToWin;
        }
    }
}
