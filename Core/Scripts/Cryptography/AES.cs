using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "AES", menuName = "TheSalt/Crypter/AES")]
    public class AES : Crypter
    {
        private const string _uniqueID = "B40725BD5EC34332AFFC92A5609C1A3A";
        [SerializeField] private byte[] _defaultPassword;

        public AES()
        {
            Type = CryptType.AES;
        }

        [DebugButton]
        public void GenerateDefaultPassword()
        {
            RijndaelManaged pPassWordCipher = new RijndaelManaged();
            pPassWordCipher.Mode = CipherMode.CBC;
            pPassWordCipher.Padding = PaddingMode.PKCS7;

            pPassWordCipher.KeySize = 128;
            pPassWordCipher.BlockSize = 128;

            byte[] vPassword = Encoding.UTF8.GetBytes(_uniqueID);
            byte[] vIV = new byte[16];

            int nIVlen = vPassword.Length;
            if (nIVlen > vIV.Length)
                nIVlen = vIV.Length;

            Array.Copy(vPassword, vIV, nIVlen);

            pPassWordCipher.Key = vIV;
            pPassWordCipher.IV = vIV;

            ICryptoTransform pCipherTransform = pPassWordCipher.CreateEncryptor();
            _defaultPassword = pCipherTransform.TransformFinalBlock(vPassword, 0, vPassword.Length);

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        [DebugButton]
        public void ChangePasswordToDefault()
        {
            Password = _defaultPassword;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        public byte[] Decrypt(byte[] encryptedBytes, byte[] password)
        {
            if (encryptedBytes.Length == 0)
                return encryptedBytes;

            byte[] decryptedBytes = null;
            // Set your salt here to meet your flavor:
            byte[] saltBytes = password;
            // Example:
            //saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(password, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (CryptoStream cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedBytes, 0, encryptedBytes.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        public byte[] Decrypt(byte[] encryptedBytes, string password)
        {
            return Decrypt(encryptedBytes, Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(password)));
        }

        public byte[] Decrypt(string encryptedBytes, byte[] password)
        {
            return Decrypt(Encoding.UTF8.GetBytes(encryptedBytes), password);
        }

        public byte[] Decrypt(string encryptedBytes, string password)
        {
            return Decrypt(Encoding.UTF8.GetBytes(encryptedBytes), Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(password)));
        }

        public override byte[] Decrypt(byte[] encryptedBytes)
        {
            return Decrypt(encryptedBytes, Password);
        }

        public byte[] Encrypt(byte[] origin, byte[] password)
        {   
            if (origin.Length == 0)
                return origin;

            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            byte[] saltBytes = password;
            // Example:
            //saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(password, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (CryptoStream cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(origin, 0, origin.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public byte[] Encrypt(byte[] origin, string password)
        {
            return Encrypt(origin, Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(password)));
        }

        public byte[] Encrypt(string origin, byte[] password)
        {
            return Encrypt(Encoding.UTF8.GetBytes(origin), password);
        }

        public byte[] Encrypt(string origin, string password)
        {
            return Encrypt(Encoding.UTF8.GetBytes(origin), Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(password)));
        }

        public override byte[] Encrypt(byte[] origin)
        {
            return Encrypt(origin, Password);
        }
    }
}
