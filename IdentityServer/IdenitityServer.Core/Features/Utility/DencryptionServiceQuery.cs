using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.Utility
{
    public class DencryptionServiceQuery : IRequest<string>
    {
        public string ResponseStream { get; set; }
    }

    public class EncryptionServiceQueryHandler : IRequestHandler<DencryptionServiceQuery, string>
    {
        private readonly IConfiguration _configuration;

        public EncryptionServiceQueryHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> Handle(DencryptionServiceQuery request, CancellationToken cancellationToken)
        {
            Aes aes = GetEncryptionAlgorithm();
            byte[] buffer = Convert.FromBase64String(request.ResponseStream);
            MemoryStream memoryStream = new MemoryStream(buffer);
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            StreamReader streamReader = new StreamReader(cryptoStream);
            return await streamReader.ReadToEndAsync();
        }

        private Aes GetEncryptionAlgorithm()
        {
            Aes aes = Aes.Create();
            var secret_key = Encoding.UTF8.GetBytes(_configuration.GetSection("Encryption:SecretKey").Value);
            var initialization_vector = Encoding.UTF8.GetBytes(_configuration.GetSection("Encryption:EncryptIV").Value);
            aes.Key = secret_key;
            aes.IV = initialization_vector;
            return aes;
        }
    }
}
