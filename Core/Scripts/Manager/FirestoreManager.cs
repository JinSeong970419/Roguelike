using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Roguelike.Core
{
    public class FirestoreManager : MonoSingleton<FirestoreManager>
    {
        [SerializeField] private Account account;
        private FirebaseFirestore db;
        private ListenerRegistration registration;

        private UserData userData;

        public UserData UserData { get { return userData; } set { userData = value; } }

        public int UniqueNumber { get; set; }

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnApplicationQuit()
        {
            registration.Stop();
        }

        private void Start()
        {
            db = FirebaseFirestore.DefaultInstance;

            registration = db.Collection("global").Document("uniqueNumber").Listen(snapshot =>
            {
                UniqueNumber num = snapshot.ConvertTo<UniqueNumber>();
                UniqueNumber = num.Number;
            });
        }

        public void GetUniqueNumberAsync(UnityAction<int> callback)
        {

            db.Collection("global").Document("uniqueNumber").GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                int uniqueNumber = task.Result.ConvertTo<UniqueNumber>().Number;
                callback(uniqueNumber);
            });
        }

        public void IncreaseUniqueNumberAsync()
        {
            db.Collection("global").Document("uniqueNumber").GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                var uniqueNumber = task.Result.ConvertTo<UniqueNumber>();
                uniqueNumber.Number += 1;

                db.Collection("global").Document("uniqueNumber").SetAsync(uniqueNumber);
            });

        }

        public void GetUserDataAsync(string userId, UnityAction<UserData> callback)
        {
            userData = null;
            Debug.Log("Test");

            db.Collection("users").Document(userId).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError($"Task is faulted. / {task.Exception.Message}");
                }
                else if (task.IsCanceled)
                {
                    Debug.LogError("Task is canceled.");
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("Task is completed.");

                    if (task.Result.Exists)
                        Debug.Log("결과값 있음");
                    else
                        Debug.Log("결과값 없음");

                    userData = task.Result.ConvertTo<UserData>();
                    account.SetUserData(userData);
                    callback.Invoke(userData);
                }
            });
        }

        public void SetUserData(UserData userData)
        {
            PopupManager.Instance.OpenPopup((int)PopupKind.PopupLoading);
            //string userIDTest = "2i3fK3HmEUPuEhl3lAkiJ7ggAcv2";
            DocumentReference docRef = db.Collection("users").Document(userData.UserId);
            //DocumentReference docRef = db.Collection("users").Document(userIDTest);
            docRef.SetAsync(userData).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {

                }
                else if (task.IsCanceled)
                {

                }
                else if (task.IsCompleted)
                {
                    account.SetUserData(userData);
                }
                PopupManager.Instance.ClosePopup((int)PopupKind.PopupLoading);
            });
        }

        public void GetUserSettingsDataAsync(string userId, UnityAction<UserSettingsData> callback)
        {
            db.Collection("user-settings").Document(userId).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                var settingsData = task.Result.ConvertTo<UserSettingsData>();
                callback(settingsData);
            });
        }

        public void SetUserSettingsData(string userId, UserSettingsData userSettingsData)
        {
            DocumentReference docRef = db.Collection("user-settings").Document(userId);
            docRef.SetAsync(userSettingsData);
        }

        public void Log(string message)
        {
            if (string.IsNullOrEmpty(account.UserId.Value))
            {
                Debug.LogError("UserId is null or empty!!");
                return;
            }

            db.Collection("user-log").Document(account.UserId.Value).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                UserLog userLog = task.Result.ConvertTo<UserLog>();
                if (userLog == null)
                {
                    userLog = new UserLog()
                    {
                        Messages = new List<string>(),
                    };
                    userLog.Messages.Add(message);
                }
                else
                {
                    userLog.Messages.Add(message);
                }

                DocumentReference docRef = db.Collection("user-log").Document(account.UserId.Value);
                docRef.SetAsync(userLog);
            });



        }

        public void GetMaxExp(int level, UnityAction<int> callback)
        {
            db.Collection("max-exp").Document(level.ToString()).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                var exp = task.Result.ConvertTo<AccountMaxExp>();
                callback(exp.MaxExp);
            });
        }

        public void SetMaxExp()
        {
            AccountMaxExp accountMaxExp = new AccountMaxExp()
            {
                MaxExp = 50,
            };

            for (int i = 1; i <= 100; i++)
            {
                DocumentReference docRef = db.Collection("max-exp").Document(i.ToString());
                docRef.SetAsync(accountMaxExp);
            }
        }

        public void GetLeaderboardData(string stage, UnityAction<LeaderboardData> callback)
        {
            db.Collection("leaderboard").Document(stage).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                var leaderboard = task.Result.ConvertTo<LeaderboardData>();
                callback(leaderboard);
            });
        }

        public void SetLeaderboardData(string stage, LeaderboardDataElement data)
        {
            db.Collection("leaderboard").Document(stage).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                LeaderboardData leaderboard = task.Result.ConvertTo<LeaderboardData>();
                if (leaderboard == null)
                {
                    leaderboard = new LeaderboardData();
                    leaderboard.Data = new List<LeaderboardDataElement>
                    {
                        data
                    };
                }
                else
                {
                    bool existUserId = false;
                    for (int i = 0; i < leaderboard.Data.Count; i++)
                    {
                        var element = leaderboard.Data[i];
                        if (element.UserId == data.UserId)
                        {
                            existUserId = true;
                            element.Score = Mathf.Max(element.Score, data.Score);
                            break;
                        }
                    }

                    if (existUserId == false)
                    {
                        leaderboard.Data.Add(data);
                    }

                    leaderboard.Data = leaderboard.Data.OrderByDescending(o => o.Score).ToList();
                    if (leaderboard.Data.Count > 10)
                    {
                        leaderboard.Data = leaderboard.Data.Take(10).ToList();
                    }
                }

                DocumentReference docRef = db.Collection("leaderboard").Document(stage);
                docRef.SetAsync(leaderboard);
            });
        }
    }
}
