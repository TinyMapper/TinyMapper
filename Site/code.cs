using System.Security.Cryptography.X509Certificates;
using Infotecs.Zulu.WPF;
using Infotecs.Pki.X509;

namespace Infotecs.Pki.CA.CertificateServiceManagmentConsole.ViewModels
{
    /// <summary>
    /// Представляет информацию о запросе на сертификат.
    /// </summary>
    public sealed class OfficialRequestViewModel<T> : BaseViewModel
    {
        private string commonName;

        /// <summary>
        /// Общее имя для имени субъекта запроса на сертификат.
        /// </summary>
        public string CommonName
        {
            get { return commonName; }
            set
            {
                commonName = value;
                OnPropertyChanged(() => CommonName);
            }
        }

        /// <summary>
        /// Конструирует объект имени.
        /// </summary>
        /// <returns>
        /// Отличающееся имя.
        /// </returns>
        public X500DistinguishedName GetSubjectName<T>()
        {
            var subject = new DistinguishedNameBuilder();
            subject.AddCommonName(commonName);
            return new X500DistinguishedName(subject.Encode());
        }
    }
}
