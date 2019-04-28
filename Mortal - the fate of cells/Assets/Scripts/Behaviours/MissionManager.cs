using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mortal
{
    public class MissionManager : MonoBehaviour
    {
        public TextMeshProUGUI remainingTime;
        public GameObject startPannel;
        public GameObject endPannel;
        public BaseGenerator generator;
        public Button startButton;
        public Button closeButton;
        public GameObject activateOnStart;
        public TextMeshProUGUI missionState;
        public TextMeshProUGUI missionStatistics;
        public CellType Type;
        public bool checkPlayerFall = true;

        public bool showStartMenu = true;
        public float timeToCompleteMission = 90.0f;
        float timePassed = 0.0f;
        protected bool isMissionStarted = false;
        protected const int missionsHardnessInterpolationCount = 40;

        protected virtual void Start()
        {
            startButton.onClick.AddListener(OnStartButtonClick);
            closeButton.onClick.AddListener(OnCloseButtonClick);
            endPannel.SetActive(false);
            if (!showStartMenu)
            {
                OnStartButtonClick();
            }
            else
            {
                activateOnStart.SetActive(false);
                startPannel.SetActive(true);
            }
        }

        protected virtual void SetupGenerator()
        {

        }

        protected virtual void Awake()
        {
            SetupGenerator();
        }

        private void OnCloseButtonClick()
        {
            GameManager.GM.ReturnToSelection();
        }

        private void OnStartButtonClick()
        {
            startPannel.SetActive(false);
            activateOnStart.SetActive(true);
            isMissionStarted = true;
        }

        protected virtual bool IsSuccess()
        {
            return timeToCompleteMission - timePassed > 0;
        }

        protected void ProcessEndMission()
        {
            isMissionStarted = false;
            var result = new MissionResults()
            {
                Type = Type,
                IsSucceeded = IsSuccess(),
                CompletionTime = timeToCompleteMission - timePassed
            };
            missionStatistics.text = result.ToString();
            activateOnStart.SetActive(false);
            endPannel.SetActive(true);
            GameManager.GM.UpdateStats(result);
        }
        
        void Update()
        {
            if (isMissionStarted)
            {
                timePassed += Time.deltaTime;
                remainingTime.text = string.Format("Remaining time: {0}", (int)(timeToCompleteMission - timePassed));
                if (timeToCompleteMission - timePassed <= 0)
                {
                    ProcessEndMission();
                }
            }
        }

        protected void LateUpdate()
        {
            if (isMissionStarted && checkPlayerFall)
            {
                if (Player.PL.transform.position.y < -20)
                {
                    ProcessEndMission();
                }
            }
        }
    }
}
