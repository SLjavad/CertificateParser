using System;
using System.Collections.Generic;
using System.Text;

namespace CertificateAnalysis
{
   public class CertificateModel
    {
        public string Name { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public string Issuer { get; set; }
        public string PubKey { get; set; }
    }
}
