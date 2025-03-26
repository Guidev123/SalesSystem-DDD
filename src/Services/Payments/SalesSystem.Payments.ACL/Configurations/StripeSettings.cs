namespace SalesSystem.Payments.ACL.Configurations
{
    public class StripeSettings
    {
        public StripeSettings(string apiKey, string frontendUrl, string stripeMode, string paymentMethodTypes)
        {
            ApiKey = apiKey;
            FrontendUrl = frontendUrl;
            StripeMode = stripeMode;
            PaymentMethodTypes = paymentMethodTypes;
        }

        public string ApiKey { get; set; } = null!;
        public string FrontendUrl { get; set; } = null!;
        public string StripeMode { get; set; } = null!;
        public string PaymentMethodTypes { get; set; } = null!;
        public string WebhookSecret { get; set; } = null!;
    }
}
