using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mortal
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager GM { get; private set; }
        [SerializeField]
        string selectionSceneName;
        [SerializeField]
        string bloodCellSceneName;
        [SerializeField]
        string neuronCellSceneName;

        public GameObject gameOverPanel;
        public Button exitGameButton;
        public Button restartGameButton;

        public AudioSource sceneChangePlay;

        const int defaultInitialLifes = 3;
        const int defaultRewardQuantity = 2;
        const int priceForMission = 1;

        Dictionary<CellType, LifetimeStats> stats;

        private void ResetGame()
        {
            stats = new Dictionary<CellType, LifetimeStats>()
            {
                {
                    CellType.BloodCell, new LifetimeStats()
                    {
                        Type = CellType.BloodCell,
                        availableCells = defaultInitialLifes,
                        missionsWon = 0,
                        RewardType = CellType.Neuron,
                        rewardQuantity = defaultRewardQuantity
                    }
                },
                {
                    CellType.Neuron, new LifetimeStats()
                    {
                        Type = CellType.Neuron,
                        availableCells = defaultInitialLifes,
                        missionsWon = 0,
                        RewardType = CellType.BloodCell,
                        rewardQuantity = defaultRewardQuantity
                    }
                }
            };
            ReturnToSelection();
            gameOverPanel.SetActive(false);
        }

        private void Awake()
        {
            if (GM != null)
            {
                Destroy(this.gameObject);
                return;
            }
            GM = this;
            DontDestroyOnLoad(this);
            ResetGame();
        }

        private void Start()
        {
            exitGameButton.onClick.AddListener(Quit);
            restartGameButton.onClick.AddListener(ResetGame);
        }

        void GameOver()
        {
            gameOverPanel.SetActive(true);
        }

        void Quit()
        {
            Application.Quit();
        }

        void SwitchSession(ref string sceneName)
        {
            sceneChangePlay?.Play();
            SceneManager.LoadScene(sceneName);
        }

        public void UpdateStats(MissionResults results)
        {
            if (results.IsSucceeded)
            {
                var type = stats[results.Type];
                type.missionsWon++;
                stats[type.RewardType].availableCells += type.rewardQuantity;
            } else
            {
                int allLives = 0;
                foreach (var item in stats)
                {
                    allLives += item.Value.availableCells;
                }
                if (allLives == 0)
                {
                    GameOver();
                }
            }
        }

        public LifetimeStats GetStats(CellType type)
        {
            return stats[type];
        }

        public void ReturnToSelection()
        {
            SwitchSession(ref selectionSceneName);
        }

        public void LaunchMission(CellType ct)
        {
            stats[ct].availableCells -= priceForMission;
            switch (ct)
            {
                case CellType.BloodCell:
                    SwitchSession(ref bloodCellSceneName);
                    break;
                case CellType.Neuron:
                    SwitchSession(ref neuronCellSceneName);
                    break;
            }
        }
    }
}
