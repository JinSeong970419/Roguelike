using ICSharpCode.SharpZipLib.GZip;
using System.IO;
using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "GZip", menuName = "TheSalt/Crypter/GZip")]
    public class GZip : Crypter
    {
        public GZip()
        {
            Type = CryptType.GZip;
        }
        public override byte[] Decrypt(byte[] gzip)
        {
            try
            {
                GZipInputStream gis = new GZipInputStream(new MemoryStream(gzip));
                MemoryStream outputMemStream = new MemoryStream();

                byte[] buffer = new byte[1024];
                int len;
                while ((len = gis.Read(buffer, 0, 1024)) > 0)
                    outputMemStream.Write(buffer, 0, len);

                byte[] val = outputMemStream.ToArray();

                //close resources
                outputMemStream.Close();
                gis.Close();

                return val;
            }
            catch (IOException)
            {
                return null;
            }
        }

        public override byte[] Encrypt(byte[] origin)
        {
            MemoryStream outputMemStream = new MemoryStream();
            GZipOutputStream zipStream = new GZipOutputStream(outputMemStream);

            zipStream.SetLevel(3); //0-9, 9 being the highest level of compression
            zipStream.Write(origin, 0, origin.Length);
            zipStream.IsStreamOwner = false;    // False stops the Close also Closing the underlying stream.
            zipStream.Close();          // Must finish the ZipOutputStream before using outputMemStream.

            outputMemStream.Position = 0;
            return outputMemStream.ToArray();
        }
    }
}
