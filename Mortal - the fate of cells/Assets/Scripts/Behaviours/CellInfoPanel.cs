using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mortal
{
    public class CellInfoPanel : MonoBehaviour
    {
        public CellType type;
        public TextMeshProUGUI availableCells;
        public TextMeshProUGUI statistics;
        public Button missionButton;

        Camera mc;

        void Start()
        {
            missionButton.onClick.AddListener(RequestSceneLoading);
            mc = Camera.main;
        }

        void Update()
        {
            transform.LookAt(mc.transform);
            transform.Rotate(0, 180, 0);
        }

        private void RequestSceneLoading()
        {
            GameManager.GM.LaunchMission(type);
        }

        private void OnEnable()
        {
            if (GameManager.GM != null)
            {
                var stats = GameManager.GM.GetStats(type);
                missionButton.gameObject.SetActive(stats.availableCells > 0);
                availableCells.text = stats.availableCells.ToString();
                string textforstats = string.Format(
                        "Missions won: {0}\nMission reward: {1} {2}s\nMission price: 1 {3}",
                        stats.missionsWon, stats.rewardQuantity, stats.RewardType, type
                    );
                statistics.text = textforstats;
            }
        }
    }
}
