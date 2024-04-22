using Firebase.Extensions;
using Google;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Roguelike.Core
{
    public class Google : MonoSingleton<Google>, IPlatform
    {
        [SerializeField] private string googleWebAPI;

        private GoogleSignInConfiguration configuration;
        private Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
        private Firebase.Auth.FirebaseAuth auth;
        private Firebase.Auth.FirebaseUser user;

        private UnityAction succeedCallback;

        protected override void Awake()
        {
            base.Awake();
            configuration = new GoogleSignInConfiguration()
            {
                WebClientId = googleWebAPI,
                RequestIdToken = true,
            };
        }

        private void Start()
        {
            InitFirebase();
        }

        private void InitFirebase()
        {
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        }

        public void SignIn(UnityAction callback)
        {

            succeedCallback = callback;

            SignIn();
        }

        public void SignIn()
        {
            GoogleSignIn.Configuration = configuration;
            GoogleSignIn.Configuration.UseGameSignIn = false;
            GoogleSignIn.Configuration.RequestIdToken = true;
            GoogleSignIn.Configuration.RequestEmail = true;

            GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnGoogleAuthenticatedFinished);
        }

        private void OnGoogleAuthenticatedFinished(Task<GoogleSignInUser> task)
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Google Sign In Faulted.");
            }
            else if (task.IsCanceled)
            {
                Debug.LogError("Google Sign In is canceled.");
            }
            
            if(task.IsCompletedSuccessfully)
            {
                Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
                auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
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

                    Debug.Log($"{userData.UserId} / {userData.ProviderId} / {userData.DisplayName}");
                    PopupManager.Instance.OpenPopup((int)PopupKind.PopupLoading);
                    FirestoreManager.Instance.GetUserDataAsync(userData.UserId, OnGetUserData);

                    SceneController.LoadScene("Lobby");
                });
            }
            else
            {
                GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthenticatedFinished);
            }
        }

        private void OnGetUserData(UserData userData)
        {
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
                };
                FirestoreManager.Instance.UserData = data;
                DataManager.Instance.Account.SetUserData(data);
                FirestoreManager.Instance.SetUserData(data);

                FirestoreManager.Instance.IncreaseUniqueNumberAsync();
            }
            else
            {
                // 기존 유저
                Debug.LogError("기존 유저");
                FirestoreManager.Instance.UserData = userData;
                DataManager.Instance.Account.SetUserData(userData);
                FirestoreManager.Instance.SetUserData(userData);
            }



            PopupManager.Instance.ClosePopup((int)PopupKind.PopupLoading);
        }

    }
}
