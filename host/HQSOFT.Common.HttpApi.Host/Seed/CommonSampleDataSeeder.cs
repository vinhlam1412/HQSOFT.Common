using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace HQSOFT.Common.Seed;

/* You can use this file to seed some sample data
 * to test your module easier.
 *
 * This class is shared among these projects:
 * - HQSOFT.Common.AuthServer
 * - HQSOFT.Common.Web.Unified (used as linked file)
 */
public class CommonSampleDataSeeder : ITransientDependency
{
    public async Task SeedAsync(DataSeedContext context)
    {

    }
}
