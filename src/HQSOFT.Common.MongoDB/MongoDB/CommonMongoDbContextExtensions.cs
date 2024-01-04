using Volo.Abp;
using Volo.Abp.MongoDB;

namespace HQSOFT.Common.MongoDB;

public static class CommonMongoDbContextExtensions
{
    public static void ConfigureCommon(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
    }
}
