using System.Linq;
using System.Threading.Tasks;
using DCTMRestAPI.Controllers;
using DCTMRestAPI.Models;
using Xunit;

namespace DCTMRestAPI.UnitTests
{
    public class CountriesControllerTests
    {
        private static CountriesController ControllerWith(params TblCountry[] rows)
        {
            var ctx = TestDb.NewContext();
            ctx.TblCountry.AddRange(rows);
            ctx.SaveChanges();
            return new CountriesController(ctx);
        }

        [Fact]
        public async Task Get_returns_all_rows()
        {
            var controller = ControllerWith(
                new TblCountry { CountryId = 1, CountryName = "A", LastUpdatedTime = 100 },
                new TblCountry { CountryId = 2, CountryName = "B", LastUpdatedTime = 200 });

            var result = await controller.Get();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task Get_by_id_returns_only_the_match()
        {
            var controller = ControllerWith(
                new TblCountry { CountryId = 1, CountryName = "A", LastUpdatedTime = 100 },
                new TblCountry { CountryId = 2, CountryName = "B", LastUpdatedTime = 200 });

            var result = (await controller.Get(1)).ToList();

            Assert.Single(result);
            Assert.Equal("A", result[0].CountryName);
        }

        [Fact]
        public async Task Get_by_id_returns_empty_when_not_found()
        {
            var controller = ControllerWith(
                new TblCountry { CountryId = 1, CountryName = "A", LastUpdatedTime = 100 });

            var result = await controller.Get(999);

            Assert.Empty(result);
        }

        [Fact]
        public async Task Get_updated_filters_by_last_updated_time()
        {
            var controller = ControllerWith(
                new TblCountry { CountryId = 1, CountryName = "A", LastUpdatedTime = 100 },
                new TblCountry { CountryId = 2, CountryName = "B", LastUpdatedTime = 200 });

            var result = (await controller.Get(150L)).ToList();

            Assert.Single(result);
            Assert.Equal("B", result[0].CountryName);
        }
    }
}
