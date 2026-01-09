namespace Shipping;

public interface IShippingRatePolicy
{
    decimal Apply(decimal current, ShippingRequest request);
}
