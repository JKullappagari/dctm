using MasterData.Api.Common;
using MasterData.Api.Features;

namespace MasterData.Api.Tests;

/// <summary>In-memory stand-in for the Dapper store, mirroring legacy SP semantics.</summary>
public sealed class FakeManufacturerStore : ILookupStore<ManufacturerRow, ManufacturerWrite>
{
    private int _nextId = 4;

    public List<ManufacturerRow> Rows { get; } =
    [
        new() { MfgID = 1, MfgName = "Dell", Description = "Servers" },
        new() { MfgID = 2, MfgName = "Cisco", Description = "Network" },
    ];

    /// <summary>Soft-deleted names → ids, mirroring rows with Status = 0 in the legacy table.</summary>
    public Dictionary<string, int> SoftDeleted { get; } =
        new(StringComparer.OrdinalIgnoreCase) { ["Nortel"] = 3 };

    public Task<IReadOnlyList<ManufacturerRow>> ListAsync(int id = 0) =>
        Task.FromResult<IReadOnlyList<ManufacturerRow>>(
            id == 0 ? [.. Rows] : Rows.Where(r => r.MfgID == id).ToList());

    public Task<int> UpsertAsync(int id, ManufacturerWrite dto, int userId)
    {
        SoftDeleted.Remove(dto.MfgName); // upserting a soft-deleted name reactivates it
        if (id == 0)
        {
            id = _nextId++;
            Rows.Add(new ManufacturerRow { MfgID = id, MfgName = dto.MfgName, Description = dto.Description });
        }
        else
        {
            var row = Rows.FirstOrDefault(r => r.MfgID == id);
            if (row is null)
                Rows.Add(new ManufacturerRow { MfgID = id, MfgName = dto.MfgName, Description = dto.Description });
            else
            {
                row.MfgName = dto.MfgName;
                row.Description = dto.Description;
            }
        }
        return Task.FromResult(id);
    }

    // Legacy DoesExist SP: -1 = active duplicate, >0 = soft-deleted id, 0 = free.
    public Task<int> CheckNameAsync(int id, ManufacturerWrite dto)
    {
        if (Rows.Any(r => r.MfgID != id && string.Equals(r.MfgName, dto.MfgName, StringComparison.OrdinalIgnoreCase)))
            return Task.FromResult(-1);
        if (id == 0 && SoftDeleted.TryGetValue(dto.MfgName, out var deletedId))
            return Task.FromResult(deletedId);
        return Task.FromResult(0);
    }

    public Task DeleteAsync(IEnumerable<int> ids, int userId)
    {
        Rows.RemoveAll(r => ids.Contains(r.MfgID));
        return Task.CompletedTask;
    }
}
