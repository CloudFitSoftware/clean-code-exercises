using System.Collections.Generic;

namespace Shipping;

public sealed class ShippingCalculator
{
    private readonly IReadOnlyList<IShippingRatePolicy> _policies;

    public ShippingCalculator(params IShippingRatePolicy[] policies)
    {
        _policies = policies;
    }

    public decimal Calculate(ShippingRequest request)
    {
        decimal total = 0m;
        foreach (var p in _policies)
        {
            total = p.Apply(total, request);
        }
        return decimal.Round(total, 2);
    }
}
