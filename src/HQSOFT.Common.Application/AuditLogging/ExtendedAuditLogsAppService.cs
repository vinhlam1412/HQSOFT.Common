
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos; 
using Volo.Abp.AuditLogging;
using Volo.Abp.Authorization.Permissions; 
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json; 

namespace HQSOFT.Common.AuditLogging
{
	[ExposeServices(typeof(IAuditLogsAppService))]
	public class ExtendedAuditLogsAppService : AuditLogsAppService
	{
		public ExtendedAuditLogsAppService(IAuditLogRepository auditLogRepository, IJsonSerializer jsonSerializer, IPermissionChecker permissionChecker, IPermissionDefinitionManager permissionDefinitionManager) : base(auditLogRepository, jsonSerializer, permissionChecker, permissionDefinitionManager)
		{
		}

		public override Task<PagedResultDto<EntityChangeDto>> GetEntityChangesAsync(GetEntityChangesDto input)
		{ 
			input.MaxResultCount = int.MaxValue;
			return base.GetEntityChangesAsync(input);
		}

	} 
}
