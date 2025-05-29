using ECommerce.Application.Common;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.DTOs;
using ECommerce.Application.DTOs.BalanceManagement.Order;
using ECommerce.Application.Interfaces;
using ECommerce.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using System.Text;

namespace ECommerce.Infrastructure.Services
{
    public class ClientBalanceService : IClientBalanceService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ClientBalanceService(HttpClient httpClient, IOptions<BalanceServiceSettings> settings)
        {
            _httpClient = httpClient;
            _baseUrl = settings.Value.BaseUrl;
        }

        public async Task<ServiceResult<List<ProductDto>>> GetProductsAsync()
        {
            var uri = $"{_baseUrl}/api/products";
            var response = await _httpClient.GetAsync(uri);

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ApiResponse<List<ProductDto>>>(content);

            if (result == null || !result.Success)
                throw new CustomException(result?.Message ?? "Failed to retrieve products.", result?.Error!, (int)response.StatusCode);

            return ServiceResult<List<ProductDto>>.Ok(result.Data ?? [], result.Message);
        }

        public async Task<PreOrderRootDto> PreOrderAsync(CreateOrderReqDto createOrderReqDto)
        {
            var uri = $"{_baseUrl}/api/balance/preorder";

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Culture = CultureInfo.InvariantCulture
            };

            var json = JsonConvert.SerializeObject(createOrderReqDto, settings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(uri, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ApiResponse<PreOrderRootDto>>(responseContent, settings);

            if (result == null || !result.Success || result.Data == null)
                throw new CustomException(result?.Message ?? "Pre-order failed.", result?.Error!, (int)response.StatusCode);

            return result.Data;
        }

        public async Task<PreOrderRootDto> CompleteAsync(CompleteOrderReqDto completeOrderReqDto)
        {
            var uri = $"{_baseUrl}/api/balance/complete";

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Culture = CultureInfo.InvariantCulture
            };

            var json = JsonConvert.SerializeObject(completeOrderReqDto, settings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(uri, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ApiResponse<PreOrderRootDto>>(responseContent);

            if (result == null || !result.Success || result.Data == null)
                throw new CustomException(result?.Message ?? "Pre-order complete failed.", result?.Error!, (int)response.StatusCode);

            return result.Data;
        }

        public async Task<PreOrderRootDto> CancelAsync(CancelOrderReqDto cancelOrderReqDto)
        {
            var uri = $"{_baseUrl}/api/balance/cancel";

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Culture = CultureInfo.InvariantCulture
            };

            var json = JsonConvert.SerializeObject(cancelOrderReqDto, settings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(uri, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ApiResponse<PreOrderRootDto>>(responseContent);

            if (result == null || !result.Success || result.Data == null)
                throw new CustomException(result?.Message ?? "Pre-order cancellation failed.", result?.Error!, (int)response.StatusCode);

            return result.Data;
        }
    }
}
