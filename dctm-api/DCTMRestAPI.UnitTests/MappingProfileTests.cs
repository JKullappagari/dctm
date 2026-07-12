using DCTMRestAPI;
using DCTMRestAPI.Models;
using DCTMRestAPI.Models.Custom;
using Xunit;

namespace DCTMRestAPI.UnitTests
{
    /// <summary>Tests for the Mapperly source-generated <see cref="DctmMapper"/> (formerly AutoMapper).</summary>
    public class DctmMapperTests
    {
        private readonly DctmMapper _mapper = new();

        [Fact]
        public void Maps_TblLocation_to_LocationDTO()
        {
            var dto = _mapper.ToLocationDto(new TblLocation
            {
                LocationId = 5,
                Location = "Rack-1",
                FloorNo = "3",
            });

            Assert.Equal(5, dto.LocationId);
            Assert.Equal("Rack-1", dto.Location);
            Assert.Equal("3", dto.FloorNo);
        }

        [Fact]
        public void Maps_TblAsset_to_AssetDTO()
        {
            // Regression guard: this map was missing under AutoMapper and broke AssetsController.AssetPatch.
            var dto = _mapper.ToAssetDto(new TblAsset
            {
                AssetId = 9,
                RefNumber = "SN-9",
                ModelId = 4,
            });

            Assert.Equal(9, dto.AssetId);
            Assert.Equal("SN-9", dto.RefNumber);
            Assert.Equal(4, dto.ModelId);
        }

        [Fact]
        public void ApplyTo_updates_existing_entity_in_place()
        {
            var entity = new TblLocation { LocationId = 1, Location = "old", FloorNo = "1" };
            var dto = new LocationDTO { LocationId = 1, Location = "new", FloorNo = "2" };

            _mapper.ApplyTo(dto, entity);

            Assert.Equal("new", entity.Location);
            Assert.Equal("2", entity.FloorNo);
        }
    }
}
