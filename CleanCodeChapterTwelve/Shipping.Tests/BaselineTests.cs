using System;
using Xunit;
using Shipping;

public class BaselineTests
{
    ShippingCalculator Calc() => new(new BaseRatePolicy(), new WeightTierPolicy(), new RegionMultiplierPolicy());

    [Fact]
    public void Intl_Weekend_Applies_Surcharge()
    {
        var c = Calc();
        var req = new ShippingRequest(ShippingSpeed.Express, Region.International, 3m, new DateTime(2026, 1, 10));
        Assert.Equal(21.45m, c.Calculate(req));
    }

    [Fact]
    public void Intl_Weekday_No_Surcharge()
    {
        var c = Calc();
        var req = new ShippingRequest(ShippingSpeed.Express, Region.International, 3m, new DateTime(2026, 1, 6));
        Assert.Equal(19.50m, c.Calculate(req));
    }

    [Fact]
    public void Domestic_Weekend_No_Surcharge()
    {
        var c = Calc();
        var req = new ShippingRequest(ShippingSpeed.Standard, Region.Domestic, 0.5m, new DateTime(2026, 1, 11));
        Assert.Equal(5.00m, c.Calculate(req));
    }
}
