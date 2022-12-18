using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Com.Intsoft.Utilities.UZDST
{
    public class X509
    {
        private X509Certificate cert;

        public X509(byte[] CertBytes)
        {
            cert = new X509Certificate(CertBytes);
            if (cert == null)
                throw new CryptographicException("Bad X509 Certificate");
        }
        public byte[] GetPublicKey()
        {
            byte[] pbk = new byte[768];
            byte[] fpbk = cert.GetPublicKey();
            if (fpbk.Length < 768)
            {
                pbk = new byte[384]; 
                Array.Copy(fpbk, 4, pbk, 0, 384);
            }
            else
                Array.Copy(fpbk, fpbk.Length - 768 - 5, pbk, 0, 768);
            return pbk;
        }
        public String GetSubject() 
        {
            return cert.Subject;
        }
        public String GetSerial()
        {
            return cert.GetSerialNumberString();
        }
        public string GetINN()
        {
            string subject = cert.Subject.ToUpper();
            if (subject.IndexOf("I=") > -1)
                return subject.Substring(subject.IndexOf("I=") + 2, 9);
            else
                return "";
        }
        public string GetExpirationDateString()
        {
            return cert.GetExpirationDateString();
        }
        public DateTime DateOfExpire()
        {
            string[] dateparts = cert.GetExpirationDateString().Split(new string[] { " " }, StringSplitOptions.None);
            string[] dateparts1 = dateparts[0].ToString().Split(new string[] { ".", "/" }, StringSplitOptions.None);

            return new DateTime(Convert.ToInt32(dateparts1[2]), Convert.ToInt32((dateparts[0].Contains("/") ? dateparts1[0] : dateparts1[1])), Convert.ToInt32((dateparts[0].Contains("/") ? dateparts1[1] : dateparts1[0])), 0, 0, 0);
        }
        public DateTime EffectiveDate()
        {
            string[] dateparts = cert.GetEffectiveDateString().Split(new string[] { " " }, StringSplitOptions.None);
            string[] dateparts1 = dateparts[0].ToString().Split(new string[] { "." }, StringSplitOptions.None);

            return new DateTime(Convert.ToInt32(dateparts1[2]), Convert.ToInt32((dateparts[0].Contains("/") ? dateparts1[0] : dateparts1[1])), Convert.ToInt32((dateparts[0].Contains("/") ? dateparts1[1] : dateparts1[0])), 0, 0, 0);
        }


    }
}
