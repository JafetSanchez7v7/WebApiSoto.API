namespace WebApiSoto.API.Middleware
{
    public class RateLimitingPolicies
    {
        public const string RateLimitPolicies = "RateLimitPolicies";

        public  string? FixedWindow { get; set; }
        public  string? SlidingWindow { get ; set; }
        public  string? TokenBucket { get; set; }
        public string? ConcurrencyLimiter { get; set; }
    }
}
