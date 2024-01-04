using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using HQSOFT.Common.TaskAssignments;
using Volo.Abp.Content;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.TaskAssignments
{
    [RemoteService(Name = "Common")]
    [Area("common")]
    [ControllerName("TaskAssignment")]
    [Route("api/common/task-assignments")]
    public class TaskAssignmentController : AbpController, ITaskAssignmentsAppService
    {
        private readonly ITaskAssignmentsAppService _taskAssignmentsAppService;

        public TaskAssignmentController(ITaskAssignmentsAppService taskAssignmentsAppService)
        {
            _taskAssignmentsAppService = taskAssignmentsAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<TaskAssignmentDto>> GetListAsync(GetTaskAssignmentsInput input)
        {
            return _taskAssignmentsAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<TaskAssignmentDto> GetAsync(Guid id)
        {
            return _taskAssignmentsAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<TaskAssignmentDto> CreateAsync(TaskAssignmentCreateDto input)
        {
            return _taskAssignmentsAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<TaskAssignmentDto> UpdateAsync(Guid id, TaskAssignmentUpdateDto input)
        {
            return _taskAssignmentsAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _taskAssignmentsAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(TaskAssignmentExcelDownloadDto input)
        {
            return _taskAssignmentsAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _taskAssignmentsAppService.GetDownloadTokenAsync();
        }
    }
}