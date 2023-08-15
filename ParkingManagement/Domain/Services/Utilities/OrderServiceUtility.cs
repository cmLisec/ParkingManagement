using AutoMapper;
using Lisec.Base.Utilities.ResponseUtilities;
using Lisec.OrderDataManagement.Domain.Mapping;
using Lisec.OrderDataManagement.Domain.Repository.Utilities;
using Lisec.OrderDataManagement.Domain.Services.v2;
using Lisec.OrderDataManagement.Domain.Utility;
using Lisec.OrderDataManagement.Domain.Utility.RestObjects;
using Lisec.OrderDataManagement.DTO.v2;
using Lisec.OrderDataManagement.DTO.v2.NumberRanges;
using Lisec.OrderDataManagement.DTO.v2.Order;
using Lisec.OrderDB.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using FormBits = Lisec.OrderDataManagement.Domain.Utility.FormBits;

namespace Lisec.OrderDataManagement.Domain.Services.Utilities
{
    /// <summary>
    /// Utility class specific for OrderService operations
    /// </summary>
    public class OrderServiceUtility
    {
        private const string _authorizationFail = @"Authorization Fail";
        private const string _responsibleUserIdDoesntExist = @"Given resposible user id doesnt exist";

        private readonly RestClientUtility _restUtil;
        private readonly IMapper _mapper;
        private readonly OrderRepositoryUtility _repoUtility;
        private readonly CommonUtility _commonUtility;
        private readonly NumberRangesService _numberRangeService;

        /// <summary>
        /// constructor Order utility
        /// </summary>
        /// <param name="restUtil"> rest client Utility</param>
        /// <param name="mapper">specify Mapper</param>
        /// <param name="repoUtility">Specify OrderRepositoryUtility class</param>
        ///  <param name="commonUtility">Specify CommonUtility class</param>
        /// <param name="numberRangesService">Specify NumberRangesService class</param>
        public OrderServiceUtility(RestClientUtility restUtil, IMapper mapper, OrderRepositoryUtility repoUtility, CommonUtility commonUtility, NumberRangesService numberRangesService)
        {
            _restUtil = restUtil;
            _mapper = mapper;
            _repoUtility = repoUtility;
            _commonUtility = commonUtility;
            _numberRangeService = numberRangesService;
        }

        /// <summary>
        /// This function add mandatory fileds to the dto with conversion
        /// </summary>
        /// <param name="dbContext">specify OrderDataManagementDBContext </param>
        /// <param name="bearer">Specify bearer token</param>
        /// <param name="orderHeaderResponse">Specify AufKopf object</param>
        /// <param name="orderEntity">specify OrderDTO object</param>
        /// <returns>BaseResponse with string result</returns>
        public async Task<BaseResponse<string>> UpdateOrderHeaderDTOObject(OrderDataManagementDBContext dbContext, string bearer, AufKopf orderHeaderResponse, OrderDTO orderEntity)
        {
            if (orderHeaderResponse.AufWerkId != null)
            {
                var stocks = await _commonUtility.GetMasterStockDataAsync(dbContext);
                var stock = stocks.FirstOrDefault(i => i.StockDesc == orderHeaderResponse.AufWerkId);
                if (stock != null && stock.StockId >= 0)
                    orderEntity.Header.ProductionInfo.StockId = (long)stock.StockId;
            }
            /*if (orderHeaderResponse.KopfSach != null)
            {
                var userId = await GetResponsibleUserIdAsync(orderHeaderResponse.KopfSach, bearer).ConfigureAwait(false);
                if (userId != 0)
                {
                    orderEntity.Header.ResponsibleUserId = userId;
                }
            }*/
            if (orderHeaderResponse.AufAdr != null && orderHeaderResponse.AufAdr.Count > 0)
            {
                await GetCountryAndProvinceIdForCustomerAddressAsync(dbContext, orderHeaderResponse, orderEntity);
                await GetCountryAndProvinceIdForDeliveryInfoAddressAsync(dbContext, orderHeaderResponse, orderEntity);
                await GetCountryAndProvinceIdForCustomerContactAddressAsync(dbContext, orderHeaderResponse, orderEntity);
            }
            return new BaseResponse<string>(@"successfull", StatusCodes.Status200OK);
        }

        private async Task GetCountryAndProvinceIdForCustomerAddressAsync(OrderDataManagementDBContext dbContext, AufKopf orderHeaderResponse, OrderDTO orderEntity)
        {
            var customerAddress = orderHeaderResponse.AufAdr.FirstOrDefault(i => i.AdrArt == 0);
            if (customerAddress != null)
            {
                if (!string.IsNullOrEmpty(customerAddress.Land))
                {
                    var countries = await _commonUtility.GetCountryAndProvinceDataAsync(dbContext);
                    if (countries != null)
                    {
                        var country = countries.FirstOrDefault(i => i.CountryName == customerAddress.Land);
                        if (country != null)
                        {
                            orderEntity.Header.CustomerAddress.CountryId = (int)country.CountryId;
                            if (country.Province != null && !string.IsNullOrEmpty(customerAddress.Province))
                            {
                                var province = country.Province.SingleOrDefault(i => i.PProvinceDesc == customerAddress.Province);
                                if (province != null)
                                    orderEntity.Header.CustomerAddress.ProvinceId = (int)province.PProvinceId;
                            }
                        }
                    }
                }
            }
        }

        private async Task GetCountryAndProvinceIdForDeliveryInfoAddressAsync(OrderDataManagementDBContext dbContext, AufKopf orderHeaderResponse, OrderDTO orderEntity)
        {
            var customerAddress = orderHeaderResponse.AufAdr.FirstOrDefault(i => i.AdrArt == 1);
            if (customerAddress != null)
            {
                if (!string.IsNullOrEmpty(customerAddress.Land))
                {
                    var countries = await _commonUtility.GetCountryAndProvinceDataAsync(dbContext);
                    if (countries != null)
                    {
                        var country = countries.FirstOrDefault(i => i.CountryName == customerAddress.Land);
                        if (country != null)
                        {
                            orderEntity.Header.DeliveryInfo.DeliveryAddress.CountryId = (int)country.CountryId;
                            if (country.Province != null && !string.IsNullOrEmpty(customerAddress.Province))
                            {
                                var province = country.Province.SingleOrDefault(i => i.PProvinceDesc == customerAddress.Province);
                                if (province != null)
                                    orderEntity.Header.DeliveryInfo.DeliveryAddress.ProvinceId = (int)province.PProvinceId;
                            }
                        }
                    }
                }
            }
        }

        private async Task GetCountryAndProvinceIdForCustomerContactAddressAsync(OrderDataManagementDBContext dbContext, AufKopf orderHeaderResponse, OrderDTO orderEntity)
        {
            var customerAddress = orderHeaderResponse.AufAdr.FirstOrDefault(i => i.AdrArt == 3);
            if (customerAddress != null)
            {
                if (!string.IsNullOrEmpty(customerAddress.Land))
                {
                    var countries = await _commonUtility.GetCountryAndProvinceDataAsync(dbContext);
                    if (countries != null)
                    {
                        var country = countries.FirstOrDefault(i => i.CountryName == customerAddress.Land);
                        if (country != null)
                        {
                            orderEntity.Header.CustomerContactAddress.CountryId = (int)country.CountryId;
                            if (country.Province != null && !string.IsNullOrEmpty(customerAddress.Province))
                            {
                                var province = country.Province.SingleOrDefault(i => i.PProvinceDesc == customerAddress.Province);
                                if (province != null)
                                    orderEntity.Header.CustomerContactAddress.ProvinceId = (int)province.PProvinceId;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This function fill ItemReference
        /// </summary>
        /// <param name="header">Specify AufKopf Object</param>
        /// <param name="orderItems">Specify list of OrderItemDTO</param>
        /// <returns>Returns List of OrderItemDTO</returns>
        public List<OrderItemDTO> FillItemReferenceAndTextsForOrderItem(AufKopf header, List<OrderItemDTO> orderItems)
        {
            if (orderItems.Count > 0)
            {
                foreach (var orderItem in orderItems)
                {
                    if (header.AufText.Count > 0)
                    {
                        //AufText is linked with AufKopf only.So fetching  AufText from AufKopf and fill it for each orderItem.
                        AufText aufText = header.AufText?
                                                 .SingleOrDefault(i => i.AufNr == header.AufNr &&
                                                                       i.AufPos == orderItem.ItemNo && i.Variante == 0 &&
                                                                       i.ZlMod == 0 && i.ZlNr == 0 && i.ViewId == 0);
                        if (aufText != null)
                            orderItem.ItemReference = aufText.ZlStr;
                        var formattedTexts = header.AufText.Where(i => i.ZlMod == (decimal)StdArt.FORMATTED_TEXT && i.AufPos == orderItem.ItemNo);
                        if (formattedTexts.Count() > 0)
                        {
                            var textGroups = formattedTexts.GroupBy(i => i.ViewId).ToList();
                            orderItem.Texts = new List<TextParagraphDTO>();
                            for (int i = 0; i < textGroups.Count; i++)
                            {
                                var text = string.Join("", textGroups[i].OrderBy(i => i.ZlNr).Select(i => i.ZlStr));
                                var formBit = textGroups[i].Select(i => i.FormBits).FirstOrDefault();
                                var modifiable = textGroups[i].Select(i => i.Modifiable).FirstOrDefault();
                                var purposeId = textGroups[i].Select(i => i.TextVerwNr).FirstOrDefault();
                                TextParagraphDTO textDto = new TextParagraphDTO()
                                {
                                    Id = textGroups[i].Key,
                                    Modifiable = modifiable == 1 ? true : false,
                                    PurposeId = purposeId,
                                    Visibility = StandardTextMappingHelper.GetVisibilityFromFormBit((FormBits?)formBit),
                                    Text = text
                                };
                                orderItem.Texts.Add(textDto);
                            }

                        }
                    }

                }
            }
            return orderItems;
        }

        /// <summary>
        /// This function create or update OrderReference object
        /// </summary>
        /// <param name="dbContext">specify OrderDataManagementDBContext</param>
        /// <param name="order">Specify OrderDTO object</param>
        /// <param name="orderHeader">Specify AufKopf object</param>
        /// <param name="bearer">Specify bearer token</param>
        /// <returns>BaseResponse with OrderDTO</returns>
        public async Task<BaseResponse<OrderDTO>> CreateOrUpdateOrderReferenceObjects(OrderDataManagementDBContext dbContext, OrderDTO order, AufKopf orderHeader, string bearer)
        {
            if (order.Header != null)
            {
                long? stockId = order.Header.ProductionInfo?.StockId;
                if (stockId.HasValue)
                {
                    List<MasterStock> stocks = await _commonUtility.GetMasterStockDataAsync(dbContext);
                    var stock = stocks.FirstOrDefault(i => i.StockId == order.Header.ProductionInfo.StockId);
                    if (stock == null)
                        return new BaseResponse<OrderDTO>(ParkingManagementConstants.FailedToGetStock, StatusCodes.Status412PreconditionFailed);
                    orderHeader.AufWerkId = stock.StockDesc;
                }
                var userId = order.Header.ResponsibleUserId;
                if (userId > 0)
                {
                    if (string.IsNullOrEmpty(bearer))
                        return new BaseResponse<OrderDTO>(_authorizationFail, StatusCodes.Status424FailedDependency);
                    if (!await CheckResponsibleUserIdIsAvailable(bearer, orderHeader, userId).ConfigureAwait(false))
                        return new BaseResponse<OrderDTO>(_responsibleUserIdDoesntExist, StatusCodes.Status424FailedDependency);
                }
                var response = await UpdateDetailsForCountryAndProvince(dbContext, order, orderHeader).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode())
                    return new BaseResponse<OrderDTO>(response.Message, response.StatusCode);
            }
            return new BaseResponse<OrderDTO>(@"successfull", StatusCodes.Status200OK);
        }

        /// <summary>
        /// This function assign next available document id to the given order.
        /// </summary>
        /// <param name="order">Specify OrderDTO object</param>
        /// <returns>BaseResponse with OrderDTO</returns>
        public async Task<BaseResponse<OrderDTO>> CreateOrderNumberAsync(OrderDTO order)
        {
            if (order.Header != null)
            {
                var numberRangeResponse = await _numberRangeService.PatchNumberRangeAsync(order.Header.OrderType, order.Header.NumberRangeId);
                if (!numberRangeResponse.IsSuccessStatusCode())
                    return new BaseResponse<OrderDTO>(numberRangeResponse.Message, numberRangeResponse.StatusCode);
                order.Header.OrderNo = numberRangeResponse.Resource.DocumentId;
            }
            return new BaseResponse<OrderDTO>(@"successfull", StatusCodes.Status200OK);
        }
        /// <summary>
        /// This function returns NumberRanges based on Id
        /// </summary>
        /// <param name="type">Specify type of number range</param>
        /// <param name="id">Specify Id of NumberRanges</param>
        /// <returns>BaseResponse with NumberRangeDTO</returns>
        public async Task<BaseResponse<NumberRangeDTO>> GetNumberRangesByIdForOrderAsync(NumberRangeDTO.Choices type, long id)
        {
            return await _numberRangeService.GetNumberRangesByIdAsync(type, id);
        }
        /// <summary>
        /// This function returns NumberRanges 
        /// <returns>BaseResponse with NumberRangeDTO</returns>
        public async Task<BaseResponse<List<NumberRangeDTO>>> GetNumberRangesForOrderAsync()
        {
            return await _numberRangeService.GetAllNumberRangesAsync(null); ;
        }
        /// <summary>
        /// This function update country and province details in order object
        /// </summary>
        /// <param name="dbContext">specify OrderDataManagementDBContext</param>
        /// <param name="order">Specify OrderDTO object</param>
        /// <param name="orderHeader">Specify AufKopf object</param>
        /// <returns></returns>
        public async Task<BaseResponse> UpdateDetailsForCountryAndProvince(OrderDataManagementDBContext dbContext, OrderDTO order, AufKopf orderHeader)
        {
            var customerAddressResponse = await SetCountryAndProvinceIdsForCustomerAddressAsync(dbContext, order, orderHeader);
            if (!customerAddressResponse.IsSuccessStatusCode())
                return new BaseResponse(customerAddressResponse.Message, customerAddressResponse.StatusCode);
            var deliveryInfoAddressResponse = await SetCountryAndProvinceIdsForDeliveryInfoAsync(dbContext, order, orderHeader);
            if (!deliveryInfoAddressResponse.IsSuccessStatusCode())
                return new BaseResponse(deliveryInfoAddressResponse.Message, deliveryInfoAddressResponse.StatusCode);
            var customerContactAddressReponse = await SetCountryAndProvinceIdsForCustomerContactAddressAsync(dbContext, order, orderHeader);
            if (!customerContactAddressReponse.IsSuccessStatusCode())
                return new BaseResponse(customerContactAddressReponse.Message, customerContactAddressReponse.StatusCode);

            return new BaseResponse();
        }

        private async Task<BaseResponse> SetCountryAndProvinceIdsForCustomerAddressAsync(OrderDataManagementDBContext dBContext, OrderDTO order, AufKopf orderHeader)
        {
            if (order.Header.CustomerAddress != null)
            {
                var countryId = order.Header.CustomerAddress.CountryId;
                if (countryId > 0)
                {
                    var masterCountries = await _commonUtility.GetCountryAndProvinceDataAsync(dBContext);
                    if (masterCountries != null)
                    {
                        var address = orderHeader.AufAdr.FirstOrDefault(i => i.AdrArt == 0);
                        var masterCountry = masterCountries.FirstOrDefault(i => i.CountryId == countryId);
                        if (masterCountry != null)
                        {
                            if (address != null)
                                address.Land = masterCountry.CountryName;
                            int provinceId = order.Header.CustomerAddress.ProvinceId;
                            if (provinceId > 0 && masterCountry.Province != null)
                            {
                                var province = masterCountry.Province.FirstOrDefault(i => i.PProvinceId == provinceId);
                                if (province != null)
                                    address.Province = province.PProvinceDesc;
                                else
                                    return new BaseResponse(ParkingManagementConstants.ProvinceNotFound, StatusCodes.Status404NotFound);
                            }
                        }
                        else
                        {
                            return new BaseResponse(ParkingManagementConstants.CountryNotFound, StatusCodes.Status404NotFound);
                        }
                    }
                    else
                        return new BaseResponse(ParkingManagementConstants.CountriesNotFound, StatusCodes.Status404NotFound);

                }
            }
            return new BaseResponse();
        }

        private async Task<BaseResponse> SetCountryAndProvinceIdsForDeliveryInfoAsync(OrderDataManagementDBContext dbContext, OrderDTO order, AufKopf orderHeader)
        {
            if (order.Header.DeliveryInfo != null)
            {
                var countryId = order.Header.DeliveryInfo.DeliveryAddress?.CountryId;
                if (countryId.HasValue && countryId > 0)
                {
                    var masterCountries = await _commonUtility.GetCountryAndProvinceDataAsync(dbContext);
                    if (masterCountries != null)
                    {
                        var country = masterCountries.FirstOrDefault(i => i.CountryId == countryId.Value);
                        if (country != null)
                        {
                            var address = orderHeader.AufAdr.FirstOrDefault(i => i.AdrArt == 1);
                            if (address != null)
                                address.Land = country.CountryName;
                            int? provinceId = order.Header.DeliveryInfo.DeliveryAddress?.ProvinceId;
                            if (provinceId.HasValue && provinceId > 0 && country.Province != null)
                            {
                                var province = country.Province.FirstOrDefault(i => i.PProvinceId == provinceId);
                                if (province != null)
                                    address.Province = province.PProvinceDesc;
                                else
                                    return new BaseResponse(ParkingManagementConstants.ProvinceNotFound, StatusCodes.Status404NotFound);
                            }
                        }
                        else
                        {
                            return new BaseResponse(ParkingManagementConstants.CountryNotFound, StatusCodes.Status404NotFound);
                        }
                    }
                    else
                        return new BaseResponse(ParkingManagementConstants.CountriesNotFound, StatusCodes.Status404NotFound);

                }
            }
            return new BaseResponse();
        }

        private async Task<BaseResponse> SetCountryAndProvinceIdsForCustomerContactAddressAsync(OrderDataManagementDBContext dbContext, OrderDTO order, AufKopf orderHeader)
        {
            if (order.Header.CustomerContactAddress != null)
            {
                var countryId = order.Header.CustomerContactAddress.CountryId;
                if (countryId > 0)
                {
                    var masterCountries = await _commonUtility.GetCountryAndProvinceDataAsync(dbContext);
                    if (masterCountries != null)
                    {
                        var country = masterCountries.FirstOrDefault(i => i.CountryId == countryId);
                        if (country != null)
                        {
                            var address = orderHeader.AufAdr.FirstOrDefault(i => i.AdrArt == 3);
                            if (address != null)
                                address.Land = country.CountryName;
                            int provinceId = order.Header.CustomerContactAddress.ProvinceId;
                            if (provinceId > 0 && country.Province != null)
                            {
                                var province = country.Province.FirstOrDefault(i => i.PProvinceId == provinceId);
                                if (province != null)
                                    address.Province = province.PProvinceDesc;
                                else
                                    return new BaseResponse(ParkingManagementConstants.ProvinceNotFound, StatusCodes.Status404NotFound);
                            }
                        }
                        else
                        {
                            return new BaseResponse(ParkingManagementConstants.CountryNotFound, StatusCodes.Status404NotFound);
                        }
                    }
                    else
                        return new BaseResponse(ParkingManagementConstants.CountriesNotFound, StatusCodes.Status404NotFound);

                }
            }
            return new BaseResponse();
        }

        /// <summary>
        /// This function check the userid given is available or not 
        /// </summary>
        /// <param name="bearer">Specify bearer token</param>
        /// <param name="orderHeader">Specify AufKopf object</param>
        /// <param name="userId">Specify User id</param>
        /// <returns>Bool value</returns>
        public async Task<bool> CheckResponsibleUserIdIsAvailable(string bearer, AufKopf orderHeader, long userId)
        {
            var loginName = await GetResponsibleLoginNameAsync(userId, bearer).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(loginName))
            {
                orderHeader.KopfSach = loginName;
                return true;
            }
            return false;
        }
        /// <summary>
        /// This function create orderdto
        /// </summary>
        /// <param name="dbContext">specify OrderDataManagementDBContext</param>
        /// <param name="orderHeaderResponse">Speify BaseResponse of AufKopf object</param>
        /// <param name="responseTotal">Speify BaseResponse of OrderTotals object</param>
        /// <param name="orderItemsResponse">Speify BaseResponse of list of AufPos object</param>
        /// <param name="bearer">Specify bearer token</param>
        /// <returns></returns>
        public async Task<OrderDTO> OrderResponseAsync(OrderDataManagementDBContext dbContext, BaseResponse<AufKopf> orderHeaderResponse, BaseResponse<OrderTotals> responseTotal, BaseResponse<List<AufPos>> orderItemsResponse, string bearer)
        {
            OrderDTO orderEntity = new OrderDTO
            {
                Header = _mapper.Map<AufKopf, OrderHeaderDTO>(orderHeaderResponse.Resource),
                Totals = _mapper.Map<OrderTotals, OrderTotalsDTO>(responseTotal.Resource),
                Items = _mapper.Map<List<AufPos>, List<OrderItemDTO>>(orderItemsResponse.Resource)
            };
            var numberRangeResponse = await GetNumberRangesByIdForOrderAsync(orderEntity.Header.OrderType, orderEntity.Header.OrderNo);
            if (numberRangeResponse.IsSuccessStatusCode())
                orderEntity.Header.NumberRangeId = numberRangeResponse.Resource.Id;
            if (orderHeaderResponse.Resource.KopfSach != null)
            {
                var userId = await GetResponsibleUserIdAsync(orderHeaderResponse.Resource.KopfSach, bearer).ConfigureAwait(false);
                if (userId != 0)
                {
                    orderEntity.Header.ResponsibleUserId = userId;
                }
            }
            FillItemReferenceAndTextsForOrderItem(orderHeaderResponse.Resource, orderEntity.Items);
            if (orderEntity.Totals != null)
                orderEntity.Totals.Checksums = _repoUtility.GetCheckSumData((long)orderHeaderResponse.Resource.AufNr);
            if (orderHeaderResponse.Resource.AufWerkId != null)
            {
                var stocks = await _commonUtility.GetMasterStockDataAsync(dbContext);
                var stock = stocks.FirstOrDefault(i => i.StockDesc == orderHeaderResponse.Resource.AufWerkId);
                if (stock != null && stock.StockId >= 0)
                    orderEntity.Header.ProductionInfo.StockId = (long)stock.StockId;
            }
            await UpdateOrderHeaderDTOObject(dbContext, bearer, orderHeaderResponse.Resource, orderEntity).ConfigureAwait(false);
            return orderEntity;
        }
        /// <summary>
        /// This function chjeck user Login name is avaliable if yes return its userId
        /// </summary>
        /// <param name="userLoginName">Specify user login name </param>
        /// <param name="bearer">specify bearer token</param>
        /// <returns>UserId</returns>
        public async Task<long> GetResponsibleUserIdAsync(string userLoginName, string bearer)
        {
            UserSummaryList userSummaryList = await GetUserSumamry(bearer).ConfigureAwait(false);
            var userId = userSummaryList.Users.Where(i => i.LoginName == userLoginName).Select(i => i.UserId).FirstOrDefault();
            return userId;
        }
        /// <summary>
        /// This function chjeck user id is avaliable if yes return its user login name
        /// </summary>
        /// <param name="userid">Specify user id </param>
        /// <param name="bearer">specify bearer token</param>
        /// <returns>user login name</returns>
        public async Task<string> GetResponsibleLoginNameAsync(long userid, string bearer)
        {
            UserSummaryList userSummaryList = await GetUserSumamry(bearer).ConfigureAwait(false);
            var loginName = userSummaryList.Users.Where(i => i.UserId == userid).Select(i => i.LoginName).FirstOrDefault();
            return loginName;
        }

        private async Task<UserSummaryList> GetUserSumamry(string bearer)
        {
            MemoryCache cache = MemoryCache.Default;
            if (!cache.Contains("users"))
            {
                var response = await _restUtil.GetUserAsync(bearer).ConfigureAwait(false);
                if (response != null)
                {
                    cache.Add("users", response, new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromHours(1) });
                }
            }
            var value = cache["users"] as RestSharp.RestResponse;

            UserSummaryList userSummaryList = JsonConvert.DeserializeObject<UserSummaryList>(value.Content);
            return userSummaryList;
        }

        /// <summary>
        /// This function add order item texts to auftext table
        /// </summary>
        /// <param name="orderNumber">Specify order number</param>
        /// <param name="orderItems">Specify Order items</param>
        /// <returns></returns>
        public List<AufText> HandleTextsForOrderItems(decimal orderNumber, List<OrderItemDTO> orderItems)
        {
            List<AufText> texts = new List<AufText>();
            if (orderItems != null && orderItems.Count > 0)
            {
                foreach (var item in orderItems)
                {
                    if (item.Texts != null && item.Texts.Count > 0)
                    {
                        foreach (var itemText in item.Texts)
                        {
                            Dictionary<StdArt, List<string>> textStrings = new Dictionary<StdArt, List<string>>();
                            int paracount = 0;
                            textStrings.Add(StdArt.PLAIN_TEXT, StandardTextMappingHelper.GetStandardTextParagraphList(StdArt.PLAIN_TEXT, itemText.Text));
                            textStrings.Add(StdArt.FORMATTED_TEXT, StandardTextMappingHelper.GetStandardTextParagraphList(StdArt.FORMATTED_TEXT, itemText.Text));
                            foreach (var textString in textStrings)
                            {
                                foreach (var value in textString.Value)
                                {
                                    AufText aufText = new AufText()
                                    {
                                        ViewId = itemText.Id,
                                        ZlStr = value,
                                        TextVerwNr = itemText.PurposeId,
                                        FormBits = StandardTextMappingHelper.GetFormBitFromVisibilty(itemText.Visibility),
                                        Modifiable = itemText.Modifiable ? 1 : 0,
                                        ZlNr = paracount++,
                                        ZlMod = (decimal)textString.Key,
                                        AufNr = orderNumber,
                                        AufPos = item.ItemNo
                                    };
                                    texts.Add(aufText);
                                }
                            }
                        }
                    }
                }
            }
            return texts;
        }
        /// <summary>
        /// This function create or update ItemReference
        /// </summary>
        /// <param name="order">Specify OrderDTO</param>
        /// <returns></returns>
        public List<AufText> CreateAufTextForItemReference(OrderDTO order)
        {
            List<AufText> aufTexts = new List<AufText>();
            if (order.Items != null && order.Items.Any())
            {
                foreach (var item in order.Items)
                {
                    if (!string.IsNullOrEmpty(item.ItemReference))
                    {
                        AufText text = new AufText
                        {
                            AufNr = order.Header.OrderNo,
                            ZlStr = item.ItemReference,
                            AufPos = item.ItemNo,
                            ZlMod = 0,
                            ZlNr = 0,
                            ViewId = 0,
                            Variante = 0,
                            FormBits = 0,
                            TextVerwNr = 0,
                        };
                        aufTexts.Add(text);
                    }
                }
            }
            return aufTexts;
        }

        /// <summary>
        /// This function fill the number range id for order header.
        /// </summary>
        /// <param name="header">Specify Order Header</param>
        /// <param name="numberRanges">Specify list of NumberRanges</param>
        /// <returns></returns>
        public BaseResponse FillNumberRangeIdForOrderHeader(OrderHeaderDTO header, List<NumberRangeDTO> numberRanges)
        {
            NumberRangeDTO numberRangeEntity = numberRanges.FirstOrDefault(i => (i.StartNumber <= header.OrderNo && i.LimitNumber >= header.OrderNo) && i.Type == header.OrderType);
            if (numberRangeEntity == null)
                return new BaseResponse(ParkingManagementConstants.FailedToGetNumberRange, StatusCodes.Status409Conflict);
            header.NumberRangeId = numberRangeEntity.Id;
            return new BaseResponse();
        }
    }
}
