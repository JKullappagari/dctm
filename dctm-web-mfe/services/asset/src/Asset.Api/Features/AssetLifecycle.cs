namespace Asset.Api.Features;

/// <summary>The legacy lifecycle flags on tblAsset that define an asset's state.</summary>
public sealed class AssetFlags
{
    public int AssetID { get; set; }
    public bool IsWriteOff { get; set; }
    public bool IsMustered { get; set; }
    public bool IsPermRestrict { get; set; }
    public DateTime? BarredStartDate { get; set; }
    public DateTime? BarredEndDate { get; set; }
    public string? CurrentRFIDCardNumber { get; set; }

    public bool IsBarred(DateTime now) =>
        BarredStartDate is { } start && BarredEndDate is { } end && start <= now && now <= end;

    public bool HasRfidCard => !string.IsNullOrWhiteSpace(CurrentRFIDCardNumber);
}

/// <summary>
/// The asset lifecycle as a set of guarded transitions. Each maps to a legacy SP.
/// An asset isn't a single-state machine — restrict/bar/writeoff/decommission and
/// RFID assignment are independent toggles — so state is a set of active conditions
/// and available actions are those whose guard currently passes.
/// </summary>
public static class AssetLifecycle
{
    public sealed record Transition(string Action, string Label, Func<AssetFlags, DateTime, bool> CanApply);

    public static readonly IReadOnlyList<Transition> Transitions =
    [
        new("writeoff", "Write Off", (f, _) => !f.IsWriteOff && !f.IsMustered),
        new("reinstate", "Reinstate", (f, _) => f.IsWriteOff),
        new("restrict", "Restrict", (f, _) => !f.IsPermRestrict),
        new("de-restrict", "Remove Restriction", (f, _) => f.IsPermRestrict),
        new("bar", "Bar (period)", (f, now) => !f.IsBarred(now)),
        new("un-bar", "Remove Bar", (f, now) => f.IsBarred(now)),
        new("decommission", "Decommission", (f, _) => !f.IsMustered),
        new("recommission", "Recommission", (f, _) => f.IsMustered),
        new("assign-rfid", "Assign RFID Card", (f, _) => !f.HasRfidCard),
        new("deassign-rfid", "De-assign RFID Card", (f, _) => f.HasRfidCard),
    ];

    /// <summary>Human-readable state conditions currently active.</summary>
    public static IReadOnlyList<string> States(AssetFlags f, DateTime now)
    {
        var states = new List<string>();
        if (f.IsWriteOff) states.Add("Written Off");
        if (f.IsMustered) states.Add("Decommissioned");
        if (f.IsPermRestrict) states.Add("Restricted");
        if (f.IsBarred(now)) states.Add("Barred");
        if (f.HasRfidCard) states.Add("RFID Assigned");
        if (states.Count == 0) states.Add("Active");
        return states;
    }

    public static IReadOnlyList<string> AvailableActions(AssetFlags f, DateTime now) =>
        Transitions.Where(t => t.CanApply(f, now)).Select(t => t.Action).ToList();

    public static Transition? Find(string action) =>
        Transitions.FirstOrDefault(t => t.Action.Equals(action, StringComparison.OrdinalIgnoreCase));
}
