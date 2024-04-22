using System;
using System.Text;
using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "UrlBase64", menuName = "TheSalt/Crypter/UrlBase64")]
    public class UrlBase64 : Crypter
    {
        public UrlBase64()
        {
            Type = CryptType.UrlBase64;
        }
        public override byte[] Decrypt(byte[] encryptedBytes)
        {
            string str = Encoding.UTF8.GetString(encryptedBytes);
            return Convert.FromBase64String(str.Replace(",", "=").Replace("-", "+").Replace("/", "_"));
        }

        public override byte[] Encrypt(byte[] origin)
        {
            string base64 = Convert.ToBase64String(origin).Replace("=", ",").Replace("+", "-").Replace("_", "/");
            return Encoding.UTF8.GetBytes(base64);
        }
    }
}
