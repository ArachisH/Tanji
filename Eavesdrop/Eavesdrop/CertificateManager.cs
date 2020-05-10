using System;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using Org.BouncyCastle.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;

namespace Eavesdrop
{
    public class CertificateManager : IDisposable
    {
        private AsymmetricKeyParameter _privateKey;
        private readonly IDictionary<string, X509Certificate2> _certificateCache;

        public string Issuer { get; }
        public string RootCertificateName { get; }
        public bool IsStoringPersonalCertificates { get; set; }

        public X509Store MyStore { get; }
        public X509Store RootStore { get; }

        public CertificateManager(string issuer, string rootCertificateName)
        {
            _certificateCache = new Dictionary<string, X509Certificate2>();

            Issuer = issuer;
            RootCertificateName = rootCertificateName;

            MyStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            RootStore = new X509Store(StoreName.Root, IsAdministrator() ? StoreLocation.LocalMachine : StoreLocation.CurrentUser);
        }

        public bool CreateTrustedRootCertificate()
        {
            return (InstallCertificate(RootStore, RootCertificateName) != null);
        }
        public bool DestroyTrustedRootCertificate()
        {
            return DestroyCertificates(RootStore);
        }
        public bool ExportTrustedRootCertificate(string path)
        {
            X509Certificate2 rootCertificate =
                InstallCertificate(RootStore, RootCertificateName);

            path = Path.GetFullPath(path);
            if (rootCertificate != null)
            {
                byte[] data = rootCertificate.Export(X509ContentType.Cert);
                File.WriteAllBytes(path, data);
            }
            return File.Exists(path);
        }

        public X509Certificate2Collection FindCertificates(string subjectName)
        {
            return FindCertificates(MyStore, subjectName);
        }
        protected virtual X509Certificate2Collection FindCertificates(X509Store store, string subjectName)
        {
            X509Certificate2Collection certificates = store.Certificates
                .Find(X509FindType.FindBySubjectDistinguishedName, subjectName, false);

            return (certificates.Count > 0 ? certificates : null);
        }

        public X509Certificate2 GenerateCertificate(string certificateName)
        {
            return InstallCertificate(MyStore, certificateName);
        }
        protected virtual X509Certificate2 InstallCertificate(X509Store store, string certificateName)
        {
            if (_certificateCache.TryGetValue(certificateName, out X509Certificate2 certificate))
            {
                return certificate;
            }
            lock (store)
            {
                try
                {
                    store.Open(OpenFlags.ReadWrite);
                    string subjectName = $"CN={certificateName}, O={Issuer}";

                    certificate = FindCertificates(store, subjectName)?[0];
                    if (certificate == null)
                    {
                        certificate = CreateCertificate(subjectName, (store == RootStore), certificateName);
                        if (certificate != null)
                        {
                            if (store == RootStore || IsStoringPersonalCertificates)
                            {
                                store.Add(certificate);
                            }
                        }
                    }
                    return certificate;
                }
                catch { return certificate = null; }
                finally
                {
                    store.Close();
                    if (certificate != null && !_certificateCache.ContainsKey(certificateName))
                    {
                        _certificateCache.Add(certificateName, certificate);
                    }
                }
            }
        }
        protected virtual X509Certificate2 CreateCertificate(string subjectName, bool isCertificateAuthority, string altName)
        {
            // Generating Random Numbers
            var randomGenerator = new CryptoApiRandomGenerator();
            var random = new SecureRandom(randomGenerator);

            // The Certificate Generator
            var certificateGenerator = new X509V3CertificateGenerator();
            if (!isCertificateAuthority)
            {
                certificateGenerator.AddExtension(X509Extensions.ExtendedKeyUsage.Id, true, new ExtendedKeyUsage(KeyPurposeID.IdKPServerAuth));
            }
            else
            {
                certificateGenerator.AddExtension(X509Extensions.BasicConstraints.Id, true, new BasicConstraints(true));
            }

            // Serial Number
            BigInteger serialNumber = BigIntegers.CreateRandomInRange(BigInteger.One, BigInteger.ValueOf(Int64.MaxValue), random);
            certificateGenerator.SetSerialNumber(serialNumber);

            // Issuer and Subject Name
            var subjectDN = new X509Name(true, subjectName);
            certificateGenerator.SetSubjectDN(subjectDN);
            certificateGenerator.SetIssuerDN(isCertificateAuthority ? subjectDN : new X509Name(true, $"CN={RootCertificateName}, O={Issuer}"));

            if (!isCertificateAuthority)
            {
                var subjectAltName = new GeneralNames(new GeneralName(GeneralName.DnsName, altName));
                certificateGenerator.AddExtension(X509Extensions.SubjectAlternativeName, false, subjectAltName);
            }

            // Valid For
            DateTime notBefore = DateTime.UtcNow.Date;
            certificateGenerator.SetNotBefore(notBefore);

            DateTime notAfter = notBefore.AddYears(1);
            certificateGenerator.SetNotAfter(notAfter);

            // Subject Public Key
            var keyGenerationParameters = new KeyGenerationParameters(random, 1024);
            var keyPairGenerator = new RsaKeyPairGenerator();
            keyPairGenerator.Init(keyGenerationParameters);

            AsymmetricCipherKeyPair subjectKeyPair = keyPairGenerator.GenerateKeyPair();
            certificateGenerator.SetPublicKey(subjectKeyPair.Public);

            AsymmetricKeyParameter issuerPrivateKey;
            if (isCertificateAuthority)
            {
                issuerPrivateKey = subjectKeyPair.Private;
                _privateKey = issuerPrivateKey;
            }
            else
            {
                if (_privateKey == null)
                {
                    X509Certificate2 rootCA = InstallCertificate(RootStore, RootCertificateName);
                    _privateKey = TransformRSAPrivateKey(rootCA.GetRSAPrivateKey().ExportParameters(true));
                }
                issuerPrivateKey = _privateKey;
            }

            var privateKey = (RsaPrivateCrtKeyParameters)issuerPrivateKey;
            if (!isCertificateAuthority)
            {
                PrivateKeyInfo info = PrivateKeyInfoFactory.CreatePrivateKeyInfo(subjectKeyPair.Private);
                var seq = (Asn1Sequence)Asn1Object.FromByteArray(info.ParsePrivateKey().GetDerEncoded());
                var rsa = RsaPrivateKeyStructure.GetInstance(seq);

                privateKey = new RsaPrivateCrtKeyParameters(rsa.Modulus, rsa.PublicExponent,
                    rsa.PrivateExponent, rsa.Prime1, rsa.Prime2, rsa.Exponent1, rsa.Exponent2, rsa.Coefficient);
            }

            // Self-Sign Certificate
            ISignatureFactory signatureFactory = new Asn1SignatureFactory("SHA256WITHRSA", issuerPrivateKey, random);
            X509Certificate2 certificate = GetSignedCertificate(certificateGenerator.Generate(signatureFactory), privateKey);

            certificate.FriendlyName = subjectName;
            return certificate;
        }

        private bool IsAdministrator()
        {
            using (var identity = WindowsIdentity.GetCurrent())
            {
                return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
        private AsymmetricKeyParameter TransformRSAPrivateKey(RSAParameters parameters)
        {
            return new RsaPrivateCrtKeyParameters(
                new BigInteger(1, parameters.Modulus),
                new BigInteger(1, parameters.Exponent),
                new BigInteger(1, parameters.D),
                new BigInteger(1, parameters.P),
                new BigInteger(1, parameters.Q),
                new BigInteger(1, parameters.DP),
                new BigInteger(1, parameters.DQ),
                new BigInteger(1, parameters.InverseQ));
        }
        private X509Certificate2 GetSignedCertificate(Org.BouncyCastle.X509.X509Certificate bcCertificate, RsaPrivateCrtKeyParameters privateKey)
        {
            string alias = bcCertificate.SubjectDN.ToString();

            var pkcs12 = new Pkcs12Store();
            var entry = new X509CertificateEntry(bcCertificate);

            pkcs12.SetCertificateEntry(alias, entry);
            pkcs12.SetKeyEntry(alias, new AsymmetricKeyEntry(privateKey), new[] { entry });
            using (var pfxStream = new MemoryStream())
            {
                pkcs12.Save(pfxStream, null, new SecureRandom());
                return new X509Certificate2(pfxStream.ToArray(), (string)null, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
            }
        }

        public void DestroyCertificates()
        {
            DestroyCertificates(MyStore);
            DestroyCertificates(RootStore);
        }
        public bool DestroyCertificates(X509Store store)
        {
            lock (store)
            {
                try
                {
                    store.Open(OpenFlags.ReadWrite);
                    X509Certificate2Collection certificates = store.Certificates.Find(X509FindType.FindByIssuerName, Issuer, false);

                    store.RemoveRange(certificates);
                    IEnumerable<string> subjectNames = certificates
                        .Cast<X509Certificate2>().Select(c => c.GetNameInfo(X509NameType.SimpleName, false));

                    foreach (string subjectName in subjectNames)
                    {
                        if (!_certificateCache.ContainsKey(subjectName)) continue;
                        _certificateCache.Remove(subjectName);
                    }
                    return true;
                }
                catch { return false; }
                finally { store.Close(); }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                MyStore.Close();
                RootStore.Close();
            }
        }
    }
}