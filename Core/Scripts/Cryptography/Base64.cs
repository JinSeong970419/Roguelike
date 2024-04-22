using System;
using System.Text;
using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "Base64", menuName = "TheSalt/Crypter/Base64")]
    public class Base64 : Crypter
    {
        public Base64()
        {
            Type = CryptType.Base64;
        }
        public override byte[] Encrypt(byte[] origin)
        {
            return Encoding.UTF8.GetBytes(Convert.ToBase64String(origin));
        }
        public override byte[] Decrypt(byte[] encryptedBytes)
        {
            return Convert.FromBase64String(Encoding.UTF8.GetString(encryptedBytes));
        }
    }
}
