using System;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Roguelike.Core
{
    public class PopupGameResult : Popup
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text stageNumber;
        [SerializeField] private TMP_Text stageName;

        [SerializeField] private TMP_Text survivalTime;
        [SerializeField] private TMP_Text gold;
        [SerializeField] private TMP_Text exp;

        [SerializeField] private Button backToLobbyButton;

        private void Awake()
        {
            backToLobbyButton.onClick.AddListener(OnClick);
        }

        public void SetData(GameResult result)
        {
            title.text = result.IsVictory ? "½Â¸®" : "ÆÐ¹è";
            stageNumber.text = result.StageSubName;
            stageName.text = result.StageName;

            TimeSpan timeSpan = TimeSpan.FromSeconds(result.PlayTime);
            survivalTime.text = timeSpan.ToString(@"mm\:ss");
            gold.text = result.Gold.KiloFormat();
            exp.text = String.Format("{0:#,##0}", (int)result.Exp);
        }

        private void OnClick()
        {
            GameManager.Instance.Resume();
            SceneController.LoadScene("Lobby");
        }

    }

    public class GameResult
    {
        public bool IsVictory;
        public string StageName;
        public string StageSubName;
        public float PlayTime;
        public float Gold;
        public float Exp;
    }
}
