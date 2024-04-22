using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Roguelike.Core
{
    public struct LogElement
    {
        public DateTime DateTime;
        public string Text;
        public string StackTrace;
        public LogType Type;

        public override string ToString()
        {
            string dateTime = DateTime.ToString("[HH:mm:ss] ");
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(dateTime);
            stringBuilder.Append("[");
            stringBuilder.Append(Type.ToString());
            stringBuilder.Append("] ");
            stringBuilder.AppendLine(Text);
            if (Type != LogType.Log && Type != LogType.Warning)
            {
                stringBuilder.Append(StackTrace);
            }

            return stringBuilder.ToString();
        }
    }
    public class LogManager : MonoSingleton<LogManager>
    {
        private const string PATH = @"/Logs/";
        private string fileName;
        private List<LogElement> logs = new List<LogElement>();

        [SerializeField] private bool autoSave;

        protected override void Awake()
        {
            base.Awake();
            if (autoSave == false) return;
            if (!Directory.Exists(PATH))
            {
                Directory.CreateDirectory(PATH);
            }
            fileName = Application.productName + DateTime.UtcNow.ToString("_yyMMdd_HHmmss") + ".txt"; 
        }

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void OnApplicationPause(bool pause)
        {
            if(autoSave)
            {
                Save();
            }
        }

        private void OnApplicationQuit()
        {
            if (autoSave)
            {
                Save();
            }
        }

        
        public void Save()
        {
            var folderPath = FileManager.Combine(Application.persistentDataPath, PATH);
            if(Directory.Exists(folderPath) == false)
            {
                Directory.CreateDirectory(folderPath);
            }
            var filePath = FileManager.Combine(Application.persistentDataPath, PATH, fileName);
            using (var stream = File.AppendText(filePath))
            {
                int count = logs.Count;
                for (int i = 0; i < count; i++)
                {
                    stream.Write(logs[i].ToString());
                }
                logs.Clear();
            }
        }

        [DebugButton]
        public static void OpenLogDirectory()
        {
            var path = FileManager.Combine(Application.persistentDataPath, PATH);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            System.Diagnostics.Process.Start(path);
        }

        [DebugButton]
        public static void ClearLogFiles()
        {
            var path = FileManager.Combine(Application.persistentDataPath, PATH);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return;
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] files = directoryInfo.GetFiles();
            int count = files.Length;
            for (int i = 0; i < count; i++)
            {
                files[i].Delete();
            }
            Debug.Log("Log files deleted successfully");
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            LogElement element = new LogElement();
            element.DateTime = DateTime.UtcNow;
            element.Text = logString;
            element.StackTrace = stackTrace;
            element.Type = type;

            logs.Add(element);
        }

    }

}
