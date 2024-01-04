using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using HQSOFT.Common.Permissions;
using HQSOFT.Common.TaskAssignments;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.TaskAssignments
{

    [Authorize(CommonPermissions.TaskAssignments.Default)]
    public class TaskAssignmentsAppService : ApplicationService, ITaskAssignmentsAppService
    {
        private readonly IDistributedCache<TaskAssignmentExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly ITaskAssignmentRepository _taskAssignmentRepository;
        private readonly TaskAssignmentManager _taskAssignmentManager;

        public TaskAssignmentsAppService(ITaskAssignmentRepository taskAssignmentRepository, TaskAssignmentManager taskAssignmentManager, IDistributedCache<TaskAssignmentExcelDownloadTokenCacheItem, string> excelDownloadTokenCache)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _taskAssignmentRepository = taskAssignmentRepository;
            _taskAssignmentManager = taskAssignmentManager;
        }

        public virtual async Task<PagedResultDto<TaskAssignmentDto>> GetListAsync(GetTaskAssignmentsInput input)
        {
            var totalCount = await _taskAssignmentRepository.GetCountAsync(input.FilterText, input.DocId, input.Url, input.DueDateMin, input.DueDateMax, input.Priority, input.Comment, input.AssignedUserId);
            var items = await _taskAssignmentRepository.GetListAsync(input.FilterText, input.DocId, input.Url, input.DueDateMin, input.DueDateMax, input.Priority, input.Comment, input.AssignedUserId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<TaskAssignmentDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<TaskAssignment>, List<TaskAssignmentDto>>(items)
            };
        }

        public virtual async Task<TaskAssignmentDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<TaskAssignment, TaskAssignmentDto>(await _taskAssignmentRepository.GetAsync(id));
        }

        [Authorize(CommonPermissions.TaskAssignments.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _taskAssignmentRepository.DeleteAsync(id);
        }

        [Authorize(CommonPermissions.TaskAssignments.Create)]
        public virtual async Task<TaskAssignmentDto> CreateAsync(TaskAssignmentCreateDto input)
        {

            var taskAssignment = await _taskAssignmentManager.CreateAsync(
            input.DocId, input.Url, input.DueDate, input.Priority, input.Comment, input.AssignedUserId
            );

            return ObjectMapper.Map<TaskAssignment, TaskAssignmentDto>(taskAssignment);
        }

        [Authorize(CommonPermissions.TaskAssignments.Edit)]
        public virtual async Task<TaskAssignmentDto> UpdateAsync(Guid id, TaskAssignmentUpdateDto input)
        {

            var taskAssignment = await _taskAssignmentManager.UpdateAsync(
            id,
            input.DocId, input.Url, input.DueDate, input.Priority, input.Comment, input.AssignedUserId, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<TaskAssignment, TaskAssignmentDto>(taskAssignment);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(TaskAssignmentExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _taskAssignmentRepository.GetListAsync(input.FilterText, input.DocId, input.Url, input.DueDateMin, input.DueDateMax, input.Priority, input.Comment, input.AssignedUserId);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<TaskAssignment>, List<TaskAssignmentExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "TaskAssignments.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new TaskAssignmentExcelDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}