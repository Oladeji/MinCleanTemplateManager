using Microsoft.Extensions.Logging;
using System.Net.Http.Json;


namespace HttpQuery
{

    public interface IRetrieveResult
    {
        Task<T> Retrieve<T>(HttpClient httpClient, ILogger logger, string url);
       
    }

    public class RetrieveResultWrapper : IRetrieveResult
    {
        public async Task<T> Retrieve<T>(HttpClient httpClient, ILogger logger, string url)
        {
            return await RetrieveResult<T>.Retrieve(httpClient, logger, url);
        }


    }

    public static class RetrieveResult<T>
    {


        public static async Task<T> Retrieve(HttpClient httpClient, ILogger logger, string url)
        {
            try
            {

                var response = await httpClient.GetAsync(url);
                var result = await response.Content.ReadFromJsonAsync<T>();

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex}, Error in RetrieveResult RetrieveResult");
                throw;

            }
        }


    }


    public interface IHttpPutAsJsonAsyncService
    {
        Task<HttpResponseMessage> PutAsJsonAsync<T>(HttpClient _client, string url, T data, CancellationToken cancellationToken);
    }

    public class HttpPutAsJsonAsyncService : IHttpPutAsJsonAsyncService
    {
        public Task<HttpResponseMessage> PutAsJsonAsync<T>(HttpClient _client,string url, T data, CancellationToken cancellationToken)  => _client.PutAsJsonAsync(url, data, cancellationToken);
          
    }

    public record RMAProductMovingEventCreateRequestDTO(RMAProductsToMoveDTO[] ProductsToMove);
    public record RMAProductsToMoveDTO(Int32 ProductId, int RmaNumber, string DestinationStage, string? MovingNotes = "");
    public record RMACloseEventCreateRequestDTO(int RMANumber, string Notes,string? SalesIdNo ,bool UseSalesIdNo);

    public interface IHttpPostAsJsonAsyncService
    {
        Task<HttpResponseMessage> PostAsJsonAsync<T>(HttpClient _client, string url, T data, CancellationToken cancellationToken);
    }

    public class HttpPostAsJsonAsyncService : IHttpPostAsJsonAsyncService
    {
        public Task<HttpResponseMessage> PostAsJsonAsync<T>(HttpClient _client, string url, T data, CancellationToken cancellationToken) => _client.PostAsJsonAsync(url, data, cancellationToken);

    }




    public static class GetResourceAuthority
    {
        public static async Task<ResourceResponsibilityResponseDTO> GetResourceAuthoritiesEmail(HttpClient httpClient, ILogger logger, string _baseUrl, string ResourceName)
        {

            var url = $"{_baseUrl}v1/EMailSenders/{ResourceName}";
            var result = await RetrieveResult<ResourceResponsibilityResponseDTO>.Retrieve(httpClient, logger, url);
            if (result == null || result.Email == null)
            {
                logger.LogError($"No email found for {ResourceName}");
                return new ResourceResponsibilityResponseDTO("", "", "", "", "", Guid.Empty);

            }
            return result;

        }
    }
    public record ResourceResponsibilityResponseDTO(string ResourceName, string Description, string Email, string OfficialName, string Designation, Guid GuidId);

}