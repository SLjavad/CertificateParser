using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CertificateAnalysis
{
    public class CertificateReader
    {
        public static CertificateModel Read(byte[] rawData)
        {
            var certificate = new X509Certificate2(rawData);
            RSACng rsa = certificate.GetRSAPublicKey() as RSACng;
            CertificateModel certificateModel = new CertificateModel
            {
                Name = certificate.SubjectName.Name,
                Issuer = certificate.Issuer,
                ValidTo = certificate.GetExpirationDateString(),
                ValidFrom = certificate.GetEffectiveDateString(),
                PubKey = Convert.ToBase64String(rsa?.ExportSubjectPublicKeyInfo() ?? certificate.GetPublicKey(), Base64FormattingOptions.InsertLineBreaks)
            };

            return certificateModel;  
        }
        public static CertificateModel Read(string rawData)
        {
            var certificate = new X509Certificate2(Convert.FromBase64String(rawData));
            RSACng rsa = certificate.GetRSAPublicKey() as RSACng;
            CertificateModel certificateModel = new CertificateModel
            {
                Name = certificate.SubjectName.Name,
                Issuer = certificate.Issuer,
                ValidTo = certificate.GetExpirationDateString(),
                ValidFrom = certificate.GetEffectiveDateString(),
                PubKey = Convert.ToBase64String(rsa?.ExportSubjectPublicKeyInfo() ?? certificate.GetPublicKey(), Base64FormattingOptions.InsertLineBreaks)
            };
            return certificateModel;
        }

        public static string HPKP(string pubKey)
        {
            byte[] pubkey = Convert.FromBase64String(pubKey);
            SHA256 sHA256 = SHA256.Create();
            byte[] sha256Data = sHA256.ComputeHash(pubkey);
            //StringBuilder stringBuilder = new StringBuilder();
            //foreach (Byte item in sha256Data)
            //{
            //    stringBuilder.Append(item.ToString("x2"));
            //}
            //System.Windows.Forms.MessageBox.Show(stringBuilder.ToString());
            return Convert.ToBase64String(sha256Data, Base64FormattingOptions.InsertLineBreaks);
        }

        
    }
}
