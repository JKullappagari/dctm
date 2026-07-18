using Asset.Api.Features;

namespace Asset.Api.Tests;

public class AssetLifecycleTests
{
    private static readonly DateTime Now = new(2026, 7, 13, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void Active_asset_offers_forward_actions_only()
    {
        var flags = new AssetFlags { AssetID = 1 };
        var actions = AssetLifecycle.AvailableActions(flags, Now);

        Assert.Contains("writeoff", actions);
        Assert.Contains("restrict", actions);
        Assert.Contains("bar", actions);
        Assert.Contains("decommission", actions);
        Assert.Contains("assign-rfid", actions);
        // Reverse actions not available yet.
        Assert.DoesNotContain("reinstate", actions);
        Assert.DoesNotContain("un-bar", actions);
        Assert.DoesNotContain("deassign-rfid", actions);
        Assert.Equal(["Active"], AssetLifecycle.States(flags, Now));
    }

    [Fact]
    public void Written_off_asset_can_only_reinstate_not_rewriteoff()
    {
        var flags = new AssetFlags { AssetID = 1, IsWriteOff = true };
        var actions = AssetLifecycle.AvailableActions(flags, Now);

        Assert.Contains("reinstate", actions);
        Assert.DoesNotContain("writeoff", actions);
        Assert.Contains("Written Off", AssetLifecycle.States(flags, Now));
    }

    [Fact]
    public void Writeoff_guard_blocks_when_decommissioned()
    {
        var flags = new AssetFlags { AssetID = 1, IsMustered = true };
        Assert.False(AssetLifecycle.Find("writeoff")!.CanApply(flags, Now));
        Assert.True(AssetLifecycle.Find("recommission")!.CanApply(flags, Now));
    }

    [Fact]
    public void Barred_only_within_the_date_window()
    {
        var flags = new AssetFlags
        {
            AssetID = 1,
            BarredStartDate = Now.AddDays(-1),
            BarredEndDate = Now.AddDays(1),
        };
        Assert.True(flags.IsBarred(Now));
        Assert.Contains("un-bar", AssetLifecycle.AvailableActions(flags, Now));
        Assert.DoesNotContain("bar", AssetLifecycle.AvailableActions(flags, Now));

        // Outside the window it is no longer barred.
        Assert.False(flags.IsBarred(Now.AddDays(5)));
    }

    [Fact]
    public void Multiple_conditions_coexist()
    {
        var flags = new AssetFlags { AssetID = 1, IsPermRestrict = true, CurrentRFIDCardNumber = "CARD-1" };
        var states = AssetLifecycle.States(flags, Now);
        Assert.Contains("Restricted", states);
        Assert.Contains("RFID Assigned", states);
        Assert.Contains("de-restrict", AssetLifecycle.AvailableActions(flags, Now));
        Assert.Contains("deassign-rfid", AssetLifecycle.AvailableActions(flags, Now));
    }
}
