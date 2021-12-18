
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Crowdin.Api;
using Crowdin.Api.Core;
using Crowdin.Api.Storage;
using Crowdin.Api.Tests.Core;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Crowdin.Api.Tests.Storage
{
    public class StorageApiTests
    {
        [Fact]
        public async Task ListStorages()
        {
            string responseJson = Core.Resources.Storage.ListStorages;

            var mockClient = TestUtils.CreateMockClientWithDefaultParser();
            
            var queryParams = new Dictionary<string, string>
            {
                { "limit", "25" },
                { "offset", "0" }
            };
            
            mockClient
                .Setup(client => client.SendGetRequest("/storages", queryParams))
                .ReturnsAsync(new CrowdinApiResult
                {
                    StatusCode = HttpStatusCode.OK,
                    JsonObject = JObject.Parse(responseJson)
                });

            var storageApi = new StorageApiExecutor(mockClient.Object);
            ResponseList<StorageResource> response = await storageApi.ListStorages();
            
            Assert.NotEmpty(response.Data);
            Assert.Equal(0, response.Pagination.Offset);
        }

        [Fact]
        public async Task DeleteStorage_Success()
        {
            const int storageId = 1;
            var mockClient = new Mock<ICrowdinApiClient>();

            mockClient
                .Setup(client => client.SendDeleteRequest($"/storages/{storageId}"))
                .ReturnsAsync(HttpStatusCode.NoContent);

            var storageApi = new StorageApiExecutor(mockClient.Object);
            await storageApi.DeleteStorage(storageId);
        }

        [Fact]
        public async Task DeleteStorage_Fail()
        {
            const int storageId = 1;
            var mockClient = new Mock<ICrowdinApiClient>();

            mockClient
                .Setup(client => client.SendDeleteRequest($"/storages/{storageId}"))
                .ReturnsAsync(HttpStatusCode.OK);

            var storageApi = new StorageApiExecutor(mockClient.Object);
            await Assert.ThrowsAsync<CrowdinApiException>(() => storageApi.DeleteStorage(storageId));
        }
    }
}