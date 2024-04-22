using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Roguelike.Core
{
    public class PopupPause : Popup
    {
        [SerializeField] private TMP_Text textTimer;
        [SerializeField] private TMP_Text textKillCount;
        [SerializeField] private TMP_Text textStageName;


        [SerializeField] private Variable<float> playTime;
        [SerializeField] private Variable<int> killCount;

        public override void OnOpen()
        {
            base.OnOpen();
            GameManager.Instance.Pause();
        }

        public override void OnClosed()
        {
            base.OnClosed();
            GameManager.Instance.Resume();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            OnKillCountChanged(killCount.Value);
            var curStage = GameManager.Instance.CurrentStage;
            if (curStage != null)
            {
                textStageName.text = curStage.StageInfo.Name;
            }

            playTime.OnValueChanged.AddListener(OnPlaytimeChanged);
            killCount.OnValueChanged.AddListener(OnKillCountChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            playTime.OnValueChanged.RemoveListener(OnPlaytimeChanged);
            killCount.OnValueChanged.RemoveListener(OnKillCountChanged);
        }


        private void OnPlaytimeChanged(float playtime)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(playtime);
            textTimer.text = timeSpan.ToString(@"mm\:ss");
        }

        private void OnKillCountChanged(int count)
        {
            textKillCount.text = count.ToString();
        }


        public void OnGiveUpButtonClick()
        {
            // TODO : 나갈 건지 한번 더 확인하는 팝업 띄워야 함.
            SceneController.LoadScene("Lobby");
        }

        public void OnSettingButtonClick()
        {
            PopupManager.Instance.OpenPopup((int)PopupKind.PopupSetting);
        }

        public void OnSkillsButtonClick()
        {
            PopupManager.Instance.OpenPopup((int)PopupKind.PopupSkill);
        }

        public void OnResumeButtonClick()
        {
            Close();
        }
    }
}
