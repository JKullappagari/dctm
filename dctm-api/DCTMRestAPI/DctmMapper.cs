using DCTMRestAPI.Models;
using DCTMRestAPI.Models.Custom;
using Riok.Mapperly.Abstractions;

namespace DCTMRestAPI
{
    /// <summary>
    /// Source-generated object mapper (Mapperly). Replaces the former AutoMapper profile.
    /// The mapper is stateless; register it as a singleton.
    /// </summary>
    [Mapper]
    public partial class DctmMapper
    {
        /// <summary>Maps a location entity to its DTO.</summary>
        public partial LocationDTO ToLocationDto(TblLocation source);

        /// <summary>Applies DTO values onto an existing tracked location entity (in-place update).</summary>
        public partial void ApplyTo(LocationDTO source, TblLocation target);

        /// <summary>Maps an asset entity to its DTO.</summary>
        public partial AssetDTO ToAssetDto(TblAsset source);
    }
}
