using Reporting.Api;

namespace Reporting.Api.Tests;

/// <summary>
/// Guards the report catalog. The legacy report SPs build dynamic SQL and treat a
/// sentinel ("NULL"/"0") as "no filter"; a blank text/location filter without a sentinel
/// produces invalid SQL like `IN ()` and a 500. These tests keep every such filter
/// safe against "run with all defaults".
/// </summary>
public sealed class ReportRegistryTests
{
    [Fact]
    public void Report_keys_are_unique()
    {
        var keys = ReportRegistry.All.Select(r => r.Key).ToList();
        Assert.Equal(keys.Count, keys.Distinct(StringComparer.OrdinalIgnoreCase).Count());
    }

    [Fact]
    public void Asset_data_excel_report_is_registered()
    {
        var report = ReportRegistry.Find("asset-data-excel");
        Assert.NotNull(report);
        Assert.Equal("iAssetTrack_sp_Report_AssetDataExcel", report!.Sp);
    }

    [Fact]
    public void Location_list_filters_default_to_the_no_filter_sentinel()
    {
        // Every @LocList filter must send "NULL" (the SP's all-locations sentinel) when blank.
        var locListParams = ReportRegistry.All
            .SelectMany(r => r.Params)
            .Where(p => p.Type == ParamType.LocList);

        Assert.All(locListParams, p => Assert.Equal("NULL", p.EmptyValue));
    }

    [Fact]
    public void Date_filters_have_an_all_inclusive_default_range()
    {
        foreach (var report in ReportRegistry.All)
            foreach (var p in report.Params.Where(p => p.Type == ParamType.Date))
                Assert.False(string.IsNullOrEmpty(p.EmptyValue),
                    $"{report.Key}.{p.Key} needs a default date bound so blank = all dates");
    }

    [Theory]
    [InlineData("transaction-list")]
    [InlineData("asset-history")]
    [InlineData("inventory")]
    public void Dynamic_sql_reports_never_leave_a_text_filter_without_a_sentinel(string key)
    {
        var report = ReportRegistry.Find(key)!;
        var textFilters = report.Params.Where(p => p.Type is ParamType.Text or ParamType.LocList);
        Assert.All(textFilters, p =>
            Assert.False(string.IsNullOrEmpty(p.EmptyValue),
                $"{key}.{p.Key} would build `IN ()` / dangling SQL when left blank"));
    }
}
