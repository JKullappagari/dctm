using MasterData.Api.Common;

namespace MasterData.Api.Features;

// Columns from iAssetTrack_Sp_MobileDevice_List (legacy DeviceReg.aspx grid).
public sealed class MobileDeviceRow
{
    public int ID { get; set; }
    public string DeviceID { get; set; } = "";
    public string DeviceName { get; set; } = "";
    public int? SiteID { get; set; }
    public string? Site { get; set; }
    public int Status { get; set; }
    public string? StatusValue { get; set; }
}

public sealed record MobileDeviceWrite(string DeviceID, string DeviceName, int? SiteId);

public static class MobileDevices
{
    // DeviceReg.aspx registers RFID readers / mobile devices. DoesExist is composite
    // (device id + device name).
    public static readonly LookupSpMap<MobileDeviceWrite> SpMap = new(
        ListSp: "iAssetTrack_Sp_MobileDevice_List",
        UpsertSp: "iAssetTrack_Sp_MobileDevice_Update",
        DeleteSp: "iAssetTrack_Sp_MobileDevice_Delete",
        ExistsSp: "iAssetTrack_Sp_MobileDevice_DoesExist",
        IdParam: "@pIntID",
        NameParam: "@pVarDeviceName",
        IdsParam: "@pVarIDs",
        NameOf: w => w.DeviceName,
        AddUpsertParams: (p, w) =>
        {
            p.Add("@pVarDeviceID", w.DeviceID);
            p.Add("@pVarDeviceName", w.DeviceName);
            p.Add("@pIntSiteID", w.SiteId ?? 0);
        },
        AddExistsParams: (p, w) =>
        {
            p.Add("@pVarDeviceID", w.DeviceID);
            p.Add("@pVarDeviceName", w.DeviceName);
        });
}
