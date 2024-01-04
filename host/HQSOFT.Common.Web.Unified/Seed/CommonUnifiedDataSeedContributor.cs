using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace HQSOFT.Common.Seed;

public class CommonUnifiedDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly CommonSampleIdentityDataSeeder _sampleIdentityDataSeeder;
    private readonly CommonSampleDataSeeder _commonSampleDataSeeder;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly ICurrentTenant _currentTenant;

    public CommonUnifiedDataSeedContributor(
        CommonSampleIdentityDataSeeder sampleIdentityDataSeeder,
        IUnitOfWorkManager unitOfWorkManager,
        CommonSampleDataSeeder commonSampleDataSeeder,
        ICurrentTenant currentTenant)
    {
        _sampleIdentityDataSeeder = sampleIdentityDataSeeder;
        _unitOfWorkManager = unitOfWorkManager;
        _commonSampleDataSeeder = commonSampleDataSeeder;
        _currentTenant = currentTenant;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await _unitOfWorkManager.Current.SaveChangesAsync();

        using (_currentTenant.Change(context?.TenantId))
        {
            await _sampleIdentityDataSeeder.SeedAsync(context);
            await _unitOfWorkManager.Current.SaveChangesAsync();
            await _commonSampleDataSeeder.SeedAsync(context);
        }
    }
}
