using System.ComponentModel;
using System.Text;
using UnityEngine;

namespace Roguelike.Core
{
    public enum CryptType
    {
        AES,
        GZip,
        Base64,
        UrlBase64,
    }
    public abstract class Crypter : ScriptableObject
    {
        [ReadOnly] [SerializeField] private CryptType _type;
        [SerializeField] private byte[] _password;
        public CryptType Type { get { return _type; } protected set { _type = value; } }
        public byte[] Password { get { return _password; } set { _password = value; } }

        public abstract byte[] Encrypt(byte[] origin);

        public byte[] Encrypt(string origin)
        {
            return Encrypt(Encoding.UTF8.GetBytes(origin));
        }

        public abstract byte[] Decrypt(byte[] encryptedBytes);

        public byte[] Decrypt(string encryptedBytes)    
        {
            return Decrypt(Encoding.UTF8.GetBytes(encryptedBytes));
        }

        public static T Create<T>() where T : Crypter, new()
        {
            return new T();
        }

        public static T Create<T>(byte[] password) where T : Crypter, new()
        {
            T instance = new T
            {
                Password = password
            };
            return instance;
        }

        public static Crypter Create(CryptType type)
        {
            Crypter crypter = null;
            switch (type)
            {
                case CryptType.AES:
                    crypter = new AES();
                    break;
                case CryptType.GZip:
                    crypter = new GZip();
                    break;
                case CryptType.Base64:
                    crypter = new Base64();
                    break;
                case CryptType.UrlBase64:
                    crypter = new UrlBase64();
                    break;
                default:
                    Debug.LogError("Undefined Crypt Type!!");
                    break;
            }

            return crypter;
        }
    }
}
