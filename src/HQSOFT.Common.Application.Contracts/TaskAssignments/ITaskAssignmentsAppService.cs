using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.TaskAssignments
{
    public interface ITaskAssignmentsAppService : IApplicationService
    {
        Task<PagedResultDto<TaskAssignmentDto>> GetListAsync(GetTaskAssignmentsInput input);

        Task<TaskAssignmentDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<TaskAssignmentDto> CreateAsync(TaskAssignmentCreateDto input);

        Task<TaskAssignmentDto> UpdateAsync(Guid id, TaskAssignmentUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(TaskAssignmentExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}