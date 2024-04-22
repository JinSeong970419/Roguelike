using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Roguelike.Core
{
    public class PopupRanking : Popup
    {
        [SerializeField] private Color gold;
        [SerializeField] private Color silver;
        [SerializeField] private Color bronze;

        [SerializeField] private Variable<int> selectedStageIndex;

        [SerializeField] private List<LeaderboardItem> leaderboards = new List<LeaderboardItem>();

        protected override void OnEnable()
        {
            base.OnEnable();

            InactiveAll();

            StageInfo info = DataManager.Instance.StageSettings.StageInfos[selectedStageIndex.Value];

            FirestoreManager.Instance.GetLeaderboardData(info.CodeName, data =>
            {
                if (data == null) return;

                int count = data.Data.Count;
                for (int i = 0; i < count; i++)
                {
                    if (i >= 10) break;

                    LeaderboardItem item = leaderboards[i];
                    if (i == 0)
                    {
                        item.rankBackgroundImage.color = gold;
                    }
                    else if (i == 1)
                    {
                        item.rankBackgroundImage.color = silver;
                    }
                    else if (i == 2)
                    {
                        item.rankBackgroundImage.color = bronze;
                    }
                    else
                    {
                        item.rankBackgroundImage.color = new Color(0, 0, 0, 0);
                    }

                    item.rankText.text = (i + 1).ToString();

                    LeaderboardDataElement dat = data.Data[i];
                    FirestoreManager.Instance.GetUserDataAsync(dat.UserId, userData =>
                    {
                        item.nicknameText.text = userData.Nickname;
                        item.scoreText.text = dat.Score.ToString();
                    });

                    item.rankBackgroundImage.transform.parent.gameObject.SetActive(true);
                }
            });
        }

        private void InactiveAll()
        {
            int count = leaderboards.Count;
            for (int i = 0; i < count; i++)
            {
                LeaderboardItem item = leaderboards[i];
                item.rankBackgroundImage.transform.parent.gameObject.SetActive(false);
                //item.rankBackgroundImage.gameObject.SetActive(false);
                //item.rankText.gameObject.SetActive(false);
                //item.profileImage.gameObject.SetActive(false);
                //item.nicknameText.gameObject.SetActive(false);
                //item.scoreText.gameObject.SetActive(false);
            }
        }

    }

    [System.Serializable]
    public class LeaderboardItem
    {
        [SerializeField] public Image rankBackgroundImage;
        [SerializeField] public TMP_Text rankText;
        [SerializeField] public Image profileImage;
        [SerializeField] public TMP_Text nicknameText;
        [SerializeField] public TMP_Text scoreText;
    }
}
