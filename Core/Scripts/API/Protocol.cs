using Firebase.Firestore;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core
{
    [FirestoreData]
    public class UniqueNumber
    {
        [FirestoreProperty] public int Number { get; set; }
    }

    [FirestoreData]
    public class UserData
    {
        [FirestoreProperty] public string UserId { get; set; }
        [FirestoreProperty] public string DisplayName { get; set; }
        [FirestoreProperty] public string ProviderId { get; set; }
        [FirestoreProperty] public string Nickname { get; set; }
        [FirestoreProperty] public int Level { get; set; }
        [FirestoreProperty] public float MaxExp { get; set; }
        [FirestoreProperty] public float Exp { get; set; }
        [FirestoreProperty] public float MaxStamina { get; set; }
        [FirestoreProperty] public float Stamina { get; set; }
        [FirestoreProperty] public int Diamond { get; set; }
        [FirestoreProperty] public int Gold { get; set; }
        [FirestoreProperty] public int SelectedStageIndex { get; set; }
        [FirestoreProperty] public DateTime FirstLoginTime { get; set; }
        [FirestoreProperty] public DateTime LastLoginTime { get; set; }
    }

    [FirestoreData]
    public class UserSettingsData
    {
        [FirestoreProperty] public bool Sound { get; set; }
        [FirestoreProperty] public bool Music { get; set; }
        [FirestoreProperty] public bool Vibration { get; set; }
        [FirestoreProperty] public bool ReducedVFX { get; set; }
        [FirestoreProperty] public bool ShowJoystick { get; set; }
        [FirestoreProperty] public string Language { get; set; }
    }

    [FirestoreData]
    public class UserLog
    {
        [FirestoreProperty] public List<string> Messages { get; set; }
    }

    [FirestoreData]
    public class AccountMaxExp
    {
        [FirestoreProperty] public int MaxExp { get; set; }
    }

    [FirestoreData]
    public class LeaderboardData
    {
        [FirestoreProperty] public List<LeaderboardDataElement> Data { get; set; }
    }

    [FirestoreData]
    public class LeaderboardDataElement
    {
        [FirestoreProperty] public string UserId { get; set; }
        [FirestoreProperty] public int Score { get; set; }
    }
}
