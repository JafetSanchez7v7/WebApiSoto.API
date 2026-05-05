namespace WebApiSoto.API.Middleware
{
    public class RateLimitingOptions
    {
        public const string RateLimitOptions = "RateLimit";
        public int PermitLimit { get; set; }
        public int Window { get; set; } 
        public int QueueLimit { get; set; } 
        public int SegmentsPerWindow { get; set; }
        public int TokenLimit { get; set; }
        public int TokensPerPeriod { get; set; }
         public int ReplenishmentPeriod { get; set; }
        public int GlobalPermitLimit { get; set; }
        public int PartitionedPermitLimit { get; set; }

    }
}
