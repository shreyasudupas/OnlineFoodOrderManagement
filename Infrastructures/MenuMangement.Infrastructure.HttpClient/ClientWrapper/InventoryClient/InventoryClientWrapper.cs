using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMangement.Infrastructure.HttpClient.ClientWrapper.InventoryClient
{
    public class InventoryClientWrapper
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;
    }
}
