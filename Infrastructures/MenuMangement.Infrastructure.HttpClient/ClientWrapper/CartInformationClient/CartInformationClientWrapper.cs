using MenuMangement.Infrastructure.HttpClient.ClientWrapper.BaseClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMangement.Infrastructure.HttpClient.ClientWrapper.CartInformationClient
{
    public class CartInformationClientWrapper : BaseClientWrapper
    {
        public CartInformationClientWrapper(IHttpClientFactory httpClientFactory
            ,ILogger<BaseClientWrapper> logger) 
            : base(httpClientFactory, logger)
        {
        }
    }
}
