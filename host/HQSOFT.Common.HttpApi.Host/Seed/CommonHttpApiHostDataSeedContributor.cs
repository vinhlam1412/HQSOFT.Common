using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace HQSOFT.Common.Seed;

public class CommonHttpApiHostDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly CommonSampleDataSeeder _commonSampleDataSeeder;
    private readonly ICurrentTenant _currentTenant;

    public CommonHttpApiHostDataSeedContributor(
        CommonSampleDataSeeder commonSampleDataSeeder,
        ICurrentTenant currentTenant)
    {
        _commonSampleDataSeeder = commonSampleDataSeeder;
        _currentTenant = currentTenant;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        using (_currentTenant.Change(context?.TenantId))
        {
            await _commonSampleDataSeeder.SeedAsync(context!);
        }
    }
}
