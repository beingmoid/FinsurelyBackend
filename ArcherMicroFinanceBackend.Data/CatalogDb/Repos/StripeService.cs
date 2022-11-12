using PanoramBackend.Data.CatalogDb.Stripe;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.CatalogDb.Repos
{
    public class StripeService : IStripeService
    {
        #region Plans
        public StripeResponseDTO<StripePlanResponseDTO> CreatePlan(StripePlanRequestDTO model)
        {
            try
            {
                StripeConfiguration.ApiKey = model.StripeSecretKey;
                var productOptions = new ProductCreateOptions
                {
                    Name = model.PlanName,
                    Description = model.PlanDescription
                };
                var productResponse = new ProductService().Create(productOptions);
                if (productResponse.StripeResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var options = new PriceCreateOptions
                    {
                        UnitAmount = GetFormattedAmountToPay(model.Amount),
                        Currency = model.Currency,
                        Product = productResponse.Id,
                    };

                    if (model.IsRecurring)
                        options.Recurring = new PriceRecurringOptions { Interval = model.Interval };

                    var planResponse = new PriceService().Create(options);
                    if (planResponse.StripeResponse.StatusCode == System.Net.HttpStatusCode.OK)
                        return new StripeResponseDTO<StripePlanResponseDTO>() { Data = new StripePlanResponseDTO { StripeProductId = productResponse.Id, StripePlanId = planResponse.Id } };
                    else
                        return new StripeResponseDTO<StripePlanResponseDTO>() { Data = null, IsSuccessful = false };
                }

                return new StripeResponseDTO<StripePlanResponseDTO>() { Data = null, IsSuccessful = false, ErrorMessage = "Something Went Wrong" };
            }
            catch (StripeException ex)
            {
                return new StripeResponseDTO<StripePlanResponseDTO>() { IsSuccessful = false, ErrorMessage = ex.Message };
            }

        }

        public StripeResponseDTO<StripePlanResponseDTO> UpdatePlan(StripePlanRequestDTO model)
        {
            try
            {
                StripeConfiguration.ApiKey = model.StripeSecretKey;
                var productOptions = new ProductUpdateOptions
                {
                    Name = model.PlanName,
                    Description = model.PlanDescription
                };
                var productResponse = new ProductService().Update(model.StripeProductId, productOptions);
                if (productResponse.StripeResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var service = new PriceService();
                    var oldPriceOptions = new PriceUpdateOptions { Active = false };
                    var oldPriceResponse = service.Update(model.StripePlanId, oldPriceOptions);
                    if (oldPriceResponse.StripeResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var newPriceOptions = new PriceCreateOptions
                        {
                            UnitAmount = GetFormattedAmountToPay(model.Amount),
                            Currency = model.Currency,
                            Product = productResponse.Id,
                        };
                        if (model.IsRecurring)
                        {
                            newPriceOptions.Recurring = new PriceRecurringOptions
                            {
                                Interval = model.Interval,
                            };
                        }
                        var planResponse = service.Create(newPriceOptions);
                        if (planResponse.StripeResponse.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            return new StripeResponseDTO<StripePlanResponseDTO>()
                            {
                                Data = new StripePlanResponseDTO { StripeProductId = productResponse.Id, StripePlanId = planResponse.Id }
                            };
                        }
                    }
                }

                return new StripeResponseDTO<StripePlanResponseDTO>() { Data = null, IsSuccessful = false, ErrorMessage = "Something Went Wrong" };
            }
            catch (StripeException ex)
            {
                return new StripeResponseDTO<StripePlanResponseDTO>() { IsSuccessful = false, ErrorMessage = ex.Message };
            }
        }

        public StripeResponseDTO<bool> DeletePlan(StripePlanRequestDTO model)
        {
            try
            {
                StripeConfiguration.ApiKey = model.StripeSecretKey;
                var oldProductOptions = new ProductUpdateOptions { Active = false };
                var response = new ProductService().Update(model.StripeProductId, oldProductOptions);
                if (response.StripeResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    return new StripeResponseDTO<bool>() { Data = true };

                return new StripeResponseDTO<bool>() { IsSuccessful = false, ErrorMessage = "Something Went Wrong" };
            }
            catch (StripeException ex)
            {
                return new StripeResponseDTO<bool>() { IsSuccessful = false, ErrorMessage = ex.Message };
            }

        }

        #endregion

        #region Customer
        public StripeResponseDTO<Customer> GetCustomer(string stripeSecretKey, string customerId)
        {
            try
            {
                StripeConfiguration.ApiKey = stripeSecretKey;

                var customerResponse = new CustomerService().Get(customerId);
                if (customerResponse.StripeResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    return new StripeResponseDTO<Customer>() { Data = customerResponse };

                return new StripeResponseDTO<Customer>() { IsSuccessful = false, ErrorMessage = "Something Went Wrong" };
            }
            catch (StripeException ex)
            {
                return new StripeResponseDTO<Customer>() { Data = null, IsSuccessful = false, ErrorMessage = ex.Message };
            }
        }

        public StripeResponseDTO<string> CreateCustomer(CreateCustomerDTO model)
        {
            try
            {
                StripeConfiguration.ApiKey = model.StripeSecretKey;

                var options = new CustomerCreateOptions
                {
                    Name = $"{model.FirstName} {model.LastName}",
                    Email = model.Email,
                    Source = model.StripePaymentToken,
                    Phone = model.Phone,
                };

                var customerResponse = new CustomerService().Create(options);
                if (customerResponse.StripeResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    return new StripeResponseDTO<string> { Data = customerResponse.Id };

                return new StripeResponseDTO<string> { Data = null, IsSuccessful = false };
            }
            catch (StripeException ex)
            {
                return new StripeResponseDTO<string> { ErrorMessage = ex.Message, Data = "", IsSuccessful = false };
            }
        }

        public StripeResponseDTO<bool> UpdateBillingDetailOfCustomer(UpdateCustomerDTO model)
        {
            try
            {
                var response = new StripeResponseDTO<bool>();
                StripeConfiguration.ApiKey = model.StripeSecretKey;

                var options = new CustomerUpdateOptions
                {
                    Source = model.StripePaymentToken,
                };

                var customerResponse = new CustomerService().Update(model.CustomerId, options);
                if (customerResponse.StripeResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    return new StripeResponseDTO<bool> { Data = true };
                else
                    return new StripeResponseDTO<bool> { Data = false, IsSuccessful = false };
            }
            catch (StripeException ex)
            {
                return new StripeResponseDTO<bool> { ErrorMessage = ex.Message, Data = false, IsSuccessful = false };
            }
        }

        #endregion

        #region Subscription
        public StripeResponseDTO<Subscription> CreateSubscription(CreateSubscriptionDTO model)
        {
            try
            {
                StripeConfiguration.ApiKey = model.StripeSecretKey;

                var options = new SubscriptionCreateOptions
                {
                    Customer = model.CustomerId,
                    Items = new List<SubscriptionItemOptions>
                    {
                      new SubscriptionItemOptions
                      {
                        Price = model.PlanId,
                        Quantity = model.Quantity
                      }
                    },
                    ProrationBehavior = "none"
                };
                var subscriptionResponse = new SubscriptionService().Create(options);
                if (subscriptionResponse.StripeResponse.StatusCode == System.Net.HttpStatusCode.OK && subscriptionResponse.Status == "active")
                    return new StripeResponseDTO<Subscription> { Data = subscriptionResponse };

                return new StripeResponseDTO<Subscription> { Data = null, IsSuccessful = false, ErrorMessage = "Unable to process your billing details." };
            }
            catch (StripeException ex)
            {
                return new StripeResponseDTO<Subscription> { ErrorMessage = ex.Message, Data = null, IsSuccessful = false };
            }
        }

        public StripeResponseDTO<Subscription> UpdateSubscription(string subscriptionId, int quantity, string stripeSecretKey)
        {
            try
            {
                StripeConfiguration.ApiKey = stripeSecretKey;
                var response = GetSubscription(subscriptionId, stripeSecretKey);
                if (response.IsSuccessful)
                {
                    var options = new SubscriptionItemUpdateOptions
                    {
                        Quantity = quantity,
                        ProrationBehavior = "none"
                    };
                    var serviceResponse = new SubscriptionItemService().Update(response.Data.Items.Data[0].Id, options);
                    if (serviceResponse.StripeResponse.StatusCode == System.Net.HttpStatusCode.OK)
                        return new StripeResponseDTO<Subscription> { Data = response.Data };
                    return new StripeResponseDTO<Subscription> { Data = null, IsSuccessful = false, ErrorMessage = "Something went wrong" };
                }
                else
                    return new StripeResponseDTO<Subscription> { Data = null, IsSuccessful = false, ErrorMessage = "Something went wrong" };
            }
            catch (StripeException ex)
            {
                return new StripeResponseDTO<Subscription> { ErrorMessage = ex.Message, Data = null, IsSuccessful = false };
            }

        }

        public StripeResponseDTO<Subscription> GetSubscription(string subscriptionId, string stripeSecretKey)
        {
            try
            {
                StripeConfiguration.ApiKey = stripeSecretKey;
                var subscriptionResponse = new SubscriptionService().Get(subscriptionId);
                if (subscriptionResponse.StripeResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    return new StripeResponseDTO<Subscription> { Data = subscriptionResponse };
                return new StripeResponseDTO<Subscription> { Data = null, IsSuccessful = false, ErrorMessage = "Something went wrong" };
            }
            catch (StripeException ex)
            {
                return new StripeResponseDTO<Subscription> { ErrorMessage = ex.Message, Data = null, IsSuccessful = false };
            }
        }

        public StripeResponseDTO<bool> CancelSubscription(string subcriptionId, string stripeSecretKey)
        {
            try
            {
                StripeConfiguration.ApiKey = stripeSecretKey;
                var subscriptionResponse = new SubscriptionService().Cancel(subcriptionId);
                if (subscriptionResponse.StripeResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    return new StripeResponseDTO<bool> { Data = true };
                return new StripeResponseDTO<bool> { ErrorMessage = "Something went wrong", IsSuccessful = false };
            }
            catch (StripeException ex)
            {
                return new StripeResponseDTO<bool> { ErrorMessage = ex.Message, IsSuccessful = false };
            }
        }

        #endregion

        #region Invoice
        public StripeResponseDTO<Invoice> GetInvoice(string invoiceId, string stripeSecretKey)
        {
            try
            {
                StripeConfiguration.ApiKey = stripeSecretKey;
                var invoiceResponse = new InvoiceService().Get(invoiceId);
                if (invoiceResponse.StripeResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    return new StripeResponseDTO<Invoice> { Data = invoiceResponse };
                return new StripeResponseDTO<Invoice> { ErrorMessage = "Something went wrong", IsSuccessful = false };
            }
            catch (StripeException ex)
            {
                return new StripeResponseDTO<Invoice> { ErrorMessage = ex.Message, IsSuccessful = false };
            }
        }
        #endregion

        #region Charge
        public StripeResponseDTO<Charge> CreateCharge(CreateChargeDTO model)
        {
            try
            {
                StripeConfiguration.ApiKey = model.StripeSecretKey;
                var options = new ChargeCreateOptions
                {
                    Amount = GetFormattedAmountToPay(model.Amount),
                    Currency = model.Currency,
                    Customer = model.CustomerId
                };

                var response = new ChargeService().Create(options);

                if (response.StripeResponse.StatusCode == System.Net.HttpStatusCode.OK && response.Status == "active")
                    return new StripeResponseDTO<Charge> { Data = response };
                return new StripeResponseDTO<Charge> { ErrorMessage = "Unable to process your billing details.", IsSuccessful = false };
            }
            catch (StripeException ex)
            {
                return new StripeResponseDTO<Charge> { ErrorMessage = ex.Message, IsSuccessful = false };
            }
        }

        public StripeResponseDTO<Charge> GetCharge(string chargeId, string stripeSecretKey)
        {
            try
            {
                StripeConfiguration.ApiKey = stripeSecretKey;
                var chargeResponse = new ChargeService().Get(chargeId);
                if (chargeResponse.StripeResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    return new StripeResponseDTO<Charge> { Data = chargeResponse };
                return new StripeResponseDTO<Charge> { ErrorMessage = "Something went wrong", IsSuccessful = false };
            }
            catch (StripeException ex)
            {
                return new StripeResponseDTO<Charge> { ErrorMessage = ex.Message, IsSuccessful = false };
            }
        }

        #endregion

        #region Card
        public StripeResponseDTO<Card> GetCard(string customerId, string cardId, string stripeSecretKey)
        {
            try
            {
                StripeConfiguration.ApiKey = stripeSecretKey;
                var cardResponse = new CardService().Get(customerId, cardId);
                if (cardResponse.StripeResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    return new StripeResponseDTO<Card> { Data = cardResponse };
                return new StripeResponseDTO<Card> { ErrorMessage = "Something went wrong", IsSuccessful = false };
            }
            catch (StripeException ex)
            {
                return new StripeResponseDTO<Card> { ErrorMessage = ex.Message, IsSuccessful = false };
            }
        }
        #endregion

        public long GetFormattedAmountToPay(decimal AmountToPay)
        {
            return Convert.ToInt64(decimal.Round(AmountToPay, 2) * 100);
        }
    }
    public interface IStripeService
    {
        StripeResponseDTO<StripePlanResponseDTO> CreatePlan(StripePlanRequestDTO model);
        StripeResponseDTO<StripePlanResponseDTO> UpdatePlan(StripePlanRequestDTO model);
        StripeResponseDTO<bool> DeletePlan(StripePlanRequestDTO model);
        StripeResponseDTO<string> CreateCustomer(CreateCustomerDTO model);
        StripeResponseDTO<Subscription> CreateSubscription(CreateSubscriptionDTO model);
        StripeResponseDTO<Subscription> UpdateSubscription(string subscriptionId, int quantity, string stripeSecretKey);
        StripeResponseDTO<Subscription> GetSubscription(string subscriptionId, string stripeSecretKey);
        StripeResponseDTO<Invoice> GetInvoice(string invoiceId, string stripeSecretKey);
        StripeResponseDTO<Charge> GetCharge(string chargeId, string stripeSecretKey);
        StripeResponseDTO<bool> CancelSubscription(string subcriptionId, string stripeSecretKey);
        StripeResponseDTO<bool> UpdateBillingDetailOfCustomer(UpdateCustomerDTO model);
        StripeResponseDTO<Customer> GetCustomer(string stripeSecretKey, string customerId);
        StripeResponseDTO<Card> GetCard(string customerId, string cardId, string stripeSecretKey);
        StripeResponseDTO<Charge> CreateCharge(CreateChargeDTO model);
        long GetFormattedAmountToPay(decimal AmountToPay);
    }
    public class StripeResponseDTO<T>
    {
        public bool IsSuccessful { get; set; } = true;
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }
}
