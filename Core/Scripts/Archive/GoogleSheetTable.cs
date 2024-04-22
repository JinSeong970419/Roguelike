using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "Google Sheet Table", menuName = "TheSalt/Google Sheet/Table")]
    public class GoogleSheetTable : ScriptableObject
    {
        [SerializeField] private string spreadSheetId;
        [SerializeField] private string sheetId;


        [DebugButton]
        public async void Test2()
        {
            string url = $"https://docs.google.com/spreadsheets/d/{spreadSheetId}/export?format=tsv&gid={sheetId}";
            UnityWebRequest www = UnityWebRequest.Get(url);
            var op = www.SendWebRequest();

            while(op.isDone == false)
            {
                await Task.Yield();
            }

            if(www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(www.downloadHandler.text);
            }
        }

    }
}
