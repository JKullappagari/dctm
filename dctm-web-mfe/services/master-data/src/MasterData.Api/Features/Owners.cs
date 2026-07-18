using MasterData.Api.Common;

namespace MasterData.Api.Features;

// Columns as returned by iAssetTrack_Sp_Owner_List (joins tblDivision for the assignment).
public sealed class OwnerRow
{
    public int OwnerID { get; set; }
    public string OwnerFirstName { get; set; } = "";
    public string? OwnerLastName { get; set; }
    public string? Email { get; set; }
    public int? DivisionID { get; set; }
    public string? Division { get; set; }
}

public sealed record OwnerWrite(string FirstName, string? LastName, string? Email, int? DivisionId);

public static class Owners
{
    public static readonly LookupSpMap<OwnerWrite> SpMap = new(
        ListSp: "iAssetTrack_Sp_Owner_List",
        UpsertSp: "iAssetTrack_Sp_Owner_Update",
        DeleteSp: "iAssetTrack_Sp_Owner_Delete",
        ExistsSp: "iAssetTrack_Sp_Owner_DoesExist",
        IdParam: "@pIntOwnerID",
        NameParam: "@pVarFirstName",
        IdsParam: "@pVarOwnerIDs",
        NameOf: w => w.FirstName,
        AddUpsertParams: (p, w) =>
        {
            p.Add("@pVarFirstName", w.FirstName);
            p.Add("@pVarLastName", w.LastName ?? "");
            p.Add("@pVarEmail", w.Email ?? "");
            p.Add("@pIntDivisionID", w.DivisionId ?? 0);
        },
        // Owner uniqueness is first + last + email (verified in the DoesExist SP).
        AddExistsParams: (p, w) =>
        {
            p.Add("@pVarFirstName", w.FirstName);
            p.Add("@pVarLastName", w.LastName ?? "");
            p.Add("@pVarEmail", w.Email ?? "");
        });
}
