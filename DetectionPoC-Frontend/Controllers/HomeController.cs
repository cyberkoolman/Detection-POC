using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DetectionPoC_Frontend.Models;
using Microsoft.Azure.Cosmos;

namespace DetectionPoC_Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var computeInstances = new List<AzureResource>();

            var collectionId = "Alerts";
            var databaseId = "ResourceHealth";
            var queryText = "SELECT DISTINCT c.resourceId FROM c";
            var resourcelist = new List<string>();

            CosmosClient client = new CosmosClient("https://<cosmosuri>.documents.azure.com:443/", "<accessKey>");

            QueryRequestOptions options = new QueryRequestOptions() { EnableScanInQuery = false };
            FeedIterator<AzureResource> resourcequeryFeed =  client.GetContainer(databaseId, collectionId).GetItemQueryIterator<AzureResource>(queryText, requestOptions: options);

            while (resourcequeryFeed.HasMoreResults)
            {
                foreach (AzureResource resource in await resourcequeryFeed.ReadNextAsync())
                {

                    var subQueryText = "SELECT TOP 1 c.resourceId, c.currentHealthStatus, c.eventTimestamp FROM c where c.resourceId = '"+ resource.ResourceId +"' ORDER BY c._ts desc";
                    FeedIterator<AzureResource> queryFeed = client.GetContainer(databaseId, collectionId).GetItemQueryIterator<AzureResource>(subQueryText, requestOptions: options);
                    while (queryFeed.HasMoreResults)
                    {
                        foreach (AzureResource _resource in await queryFeed.ReadNextAsync())
                        {
                            _resource.SubscriptionId = _resource.ResourceId.Split("/")[2];
                            _resource.ResourceGroup = _resource.ResourceId.Split("/")[4];
                            _resource.ResourceType = _resource.ResourceId.Split("/")[6];
                            _resource.ResourceId = _resource.ResourceId.Split("/")[8];
                            computeInstances.Add(_resource);
                        }
                    }

                }
            }

            return View(computeInstances.OrderBy(p => p.ResourceId).ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
