using Firebase.Auth;
using Firebase.Extensions;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Roguelike.Core
{
    public class Guest : MonoSingleton<Guest>, IPlatform
    {
        private FirebaseAuth auth;
        private FirebaseUser user;

        private UnityAction succeedCallback;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            auth = FirebaseAuth.DefaultInstance;
        }

        public void SignIn(UnityAction callback)
        {
            succeedCallback = callback;
            SignIn();
        }

        public void SignIn()
        {
            auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithCredentialAsync was canceled.");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception); ;
                    return;
                }

                user = auth.CurrentUser;

                UserData userData = new UserData()
                {
                    UserId = user.UserId,
                    DisplayName = user.DisplayName,
                    ProviderId = user.ProviderId,
                };

                Debug.Log($"{userData.UserId} / {userData.ProviderId}");
                PopupManager.Instance.OpenPopup((int)PopupKind.PopupLoading);
                FirestoreManager.Instance.GetUserDataAsync(userData.UserId, OnGetUserData);

                SceneController.LoadScene("Lobby");
            });
        }

        private void OnGetUserData(UserData userData)
        {
            DateTime loginTime = DateTime.UtcNow;

            if (userData == null)
            {
                // 신규 유저
                Debug.LogError("신규 유저");
                UserData data = new UserData()
                {
                    UserId = user.UserId,
                    DisplayName = user.DisplayName,
                    ProviderId = user.ProviderId,
                    Nickname = "Player" + FirestoreManager.Instance.UniqueNumber,
                    MaxStamina = 100,
                    Stamina = 100,
                    Level = 1,
                    FirstLoginTime = loginTime,
                    LastLoginTime = loginTime,
                };
                FirestoreManager.Instance.UserData = data;
                DataManager.Instance.Account.SetUserData(data);
                FirestoreManager.Instance.SetUserData(data);

                FirestoreManager.Instance.IncreaseUniqueNumberAsync();

                DataManager.Instance.UserSettings.Save();

            }
            else
            {
                // 기존 유저
                Debug.LogError($"기존 유저 {loginTime}");

                if(userData.FirstLoginTime.Year < 2023)
                {
                    userData.FirstLoginTime = loginTime;
                }

                userData.LastLoginTime = loginTime;

                FirestoreManager.Instance.UserData = userData;
                DataManager.Instance.Account.SetUserData(userData);
                FirestoreManager.Instance.SetUserData(userData);

                DataManager.Instance.UserSettings.Load();
            }

            PopupManager.Instance.ClosePopup((int)PopupKind.PopupLoading);
        }
    }
}
