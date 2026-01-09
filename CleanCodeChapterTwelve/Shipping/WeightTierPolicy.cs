namespace Shipping;

public sealed class WeightTierPolicy : IShippingRatePolicy
{
    public decimal Apply(decimal current, ShippingRequest r) => current + (r.WeightKg switch
    {
        <= 1m => 0m,
        <= 5m => 3m,
        _ => 7m
    });
}
