using System;
using Xunit;
using Shipping;

public class RefactorTests
{
    // After refactor, weekend surcharge should apply to ALL shipments, not just International.
    ShippingCalculator CalcRefactored() => new(
        new BaseRatePolicy(),
        new WeightTierPolicy(),
        new RegionMultiplierPolicy()
        // TODO: After refactor, remove weekend logic from RegionMultiplierPolicy and add unified weekend policy.
    );

    [Fact(Skip = "Enable after refactor")]
    public void Domestic_Weekend_Should_Apply_Surcharge()
    {
        var c = CalcRefactored();
        var req = new ShippingRequest(ShippingSpeed.Standard, Region.Domestic, 0.5m, new DateTime(2026, 1, 11));
        Assert.Equal(5.50m, c.Calculate(req));
    }
}
