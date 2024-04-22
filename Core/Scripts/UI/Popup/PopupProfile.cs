using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Roguelike.Core
{
    public class PopupProfile : Popup
    {
        [SerializeField] private Account account;
        [SerializeField] private Image imageProfile;
        [SerializeField] private TMP_Text textNickname;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TMP_Text changeNicknameButtonText;

        protected override void OnEnable()
        {
            base.OnEnable();
            Refresh();


            account.Nickname.OnValueChanged.AddListener(OnNicknameChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            account.Nickname.OnValueChanged.RemoveListener(OnNicknameChanged);
        }

        public void OnChangeNicknameButtonClick()
        {
            if(inputField.gameObject.activeSelf)
            {
                // Check and Change nickname
                string nickname = inputField.text;
                if(string.IsNullOrEmpty(nickname) == false)
                {
                    string oldNickname = account.Nickname.Value;
                    account.Nickname.Value = nickname;
                    FirestoreManager.Instance.SetUserData(account.ToUserData());
                    FirestoreManager.Instance.Log($"[{DateTime.UtcNow}] 닉네임 변경 [{oldNickname}] to [{nickname}]");
                }
                
                changeNicknameButtonText.text = "Change Nickname"; // TODO: Localization
                inputField.gameObject.SetActive(false);

            }
            else
            {
                changeNicknameButtonText.text = "Confirm"; // TODO: Localization
                inputField.gameObject.SetActive(true);
            }
        }

        private void OnNicknameChanged(string nickname)
        {
            textNickname.text = nickname;

        }

        public void Refresh()
        {
            OnNicknameChanged(account.Nickname.Value);
        }

    }
}
