using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace HQSOFT.Common.Seed;

public class CommonAuthServerDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly CommonSampleIdentityDataSeeder _commonSampleIdentityDataSeeder;
    private readonly CommonAuthServerDataSeeder _commonAuthServerDataSeeder;
    private readonly ICurrentTenant _currentTenant;

    public CommonAuthServerDataSeedContributor(
        CommonAuthServerDataSeeder commonAuthServerDataSeeder,
        CommonSampleIdentityDataSeeder commonSampleIdentityDataSeeder,
        ICurrentTenant currentTenant)
    {
        _commonAuthServerDataSeeder = commonAuthServerDataSeeder;
        _commonSampleIdentityDataSeeder = commonSampleIdentityDataSeeder;
        _currentTenant = currentTenant;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        using (_currentTenant.Change(context?.TenantId))
        {
            await _commonSampleIdentityDataSeeder.SeedAsync(context!);
            await _commonAuthServerDataSeeder.SeedAsync(context!);
        }
    }
}
