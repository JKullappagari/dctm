namespace MasterData.Api.Common;

public interface ILookupStore<TRow, TWrite>
{
    /// <summary>Lists all rows, or a single row when <paramref name="id"/> &gt; 0 (legacy List SP semantics).</summary>
    Task<IReadOnlyList<TRow>> ListAsync(int id = 0);

    /// <summary>Insert (id = 0) or update via the legacy upsert SP. Returns the entity id.</summary>
    Task<int> UpsertAsync(int id, TWrite dto, int userId);

    /// <summary>
    /// Legacy DoesExist SP semantics: -1 = an active record with this name exists (block);
    /// 0 = name is free; &gt;0 = a soft-deleted record with this name exists — upsert with
    /// that id to reactivate it (behavior confirmed in Manufacturer.aspx.cs).
    /// </summary>
    Task<int> CheckNameAsync(int id, TWrite dto);

    /// <summary>Soft-deletes by CSV of ids (legacy Delete SP: status 0 + modified-by).</summary>
    Task DeleteAsync(IEnumerable<int> ids, int userId);
}
