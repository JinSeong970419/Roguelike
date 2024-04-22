using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Roguelike.Core
{
    public class FileManager : MonoSingleton<FileManager>
    {
        public static string PersistentPath { get { return Application.persistentDataPath; } }

        protected override void Awake()
        {
            base.Awake();
        }

        public static void CreateDirectory(string filename)
        {
            Directory.CreateDirectory((new FileInfo(filename)).Directory.FullName);
        }

        public static string CombinePersistentPath(string filename)
        {
            return string.Format("{0}/{1}", PersistentPath, filename);
        }


        public static void CreateFileFromTexture(string filePath, Texture2D texture, ImageExtension imageExtension)
        {
            CreateDirectory(filePath);
            switch (imageExtension)
            {
                case ImageExtension.PNG:
                    File.WriteAllBytes(filePath, texture.EncodeToPNG());
                    break;

                case ImageExtension.JPG:
                    File.WriteAllBytes(filePath, texture.EncodeToJPG());
                    break;
            }
        }

        public static string[] GetFile(string szAddress, string szSearchPattern, SearchOption eOption = SearchOption.AllDirectories)
        {
            return Directory.GetFiles(szAddress, szSearchPattern, eOption);
        }

        public static void DeleteFile(string szFileAddress)
        {
            File.Delete(szFileAddress);
        }

        public static DateTime GetFileLastWriteTime(string szFileAddress)
        {
            return File.GetLastWriteTime(szFileAddress);
        }

        public static bool CheckFileExist(string szFileAddress)
        {
            return File.Exists(szFileAddress);
        }

        public static bool CheckDirectoryExist(string szDirAddress)
        {
            return Directory.Exists(szDirAddress);
        }

        public static string GetFileNameAndExtension(string szFullAddress)
        {
            int nIndex = szFullAddress.LastIndexOf("\\");
            if (nIndex <= 0)
                nIndex = szFullAddress.LastIndexOf("/");

            return szFullAddress.Substring(nIndex + 1);
        }
        public static string[] GetFileNameAndExtension(string[] szFullAddress)
        {
            string[] vRetVal = new string[szFullAddress.Length];
            for (int nInd = 0; nInd < vRetVal.Length; ++nInd)
                vRetVal[nInd] = GetFileNameAndExtension(szFullAddress[nInd]);

            return vRetVal;
        }
        public static string GetFileName(string szFullAddress)
        {
            szFullAddress = GetFileNameAndExtension(szFullAddress);

            int nIndex = szFullAddress.LastIndexOf(".");
            if (nIndex > 0)
                szFullAddress = szFullAddress.Substring(0, nIndex);

            return szFullAddress;
        }
        public static string[] GetFileName(string[] szFullAddress)
        {
            string[] vRetVal = new string[szFullAddress.Length];
            for (int nInd = 0; nInd < vRetVal.Length; ++nInd)
                vRetVal[nInd] = GetFileName(szFullAddress[nInd]);

            return vRetVal;
        }

        public static string Combine(params string[] paths)
        {
            int pathCount = paths.Length;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < pathCount; i++)
            {
                stringBuilder.Append(paths[i]);
            }

            return stringBuilder.ToString();
        }
    }
}
