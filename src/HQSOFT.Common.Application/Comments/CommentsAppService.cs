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
using HQSOFT.Common.Comments;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.Comments
{

    [Authorize(CommonPermissions.Comments.Default)]
    public abstract class CommentsAppServiceBase : ApplicationService
    {
        protected IDistributedCache<CommentExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        protected ICommentRepository _commentRepository;
        protected CommentManager _commentManager;

        public CommentsAppServiceBase(ICommentRepository commentRepository, CommentManager commentManager, IDistributedCache<CommentExcelDownloadTokenCacheItem, string> excelDownloadTokenCache)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _commentRepository = commentRepository;
            _commentManager = commentManager;
        }

        public virtual async Task<PagedResultDto<CommentDto>> GetListAsync(GetCommentsInput input)
        {
            var totalCount = await _commentRepository.GetCountAsync(input.FilterText, input.FromUserId, input.Content, input.DocId, input.Url);
            var items = await _commentRepository.GetListAsync(input.FilterText, input.FromUserId, input.Content, input.DocId, input.Url, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<CommentDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Comment>, List<CommentDto>>(items)
            };
        }

        public virtual async Task<CommentDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Comment, CommentDto>(await _commentRepository.GetAsync(id));
        }

        [Authorize(CommonPermissions.Comments.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _commentRepository.DeleteAsync(id);
        }

        [Authorize(CommonPermissions.Comments.Create)]
        public virtual async Task<CommentDto> CreateAsync(CommentCreateDto input)
        {

            var comment = await _commentManager.CreateAsync(
            input.FromUserId, input.DocId, input.Content, input.Url
            );

            return ObjectMapper.Map<Comment, CommentDto>(comment);
        }

        [Authorize(CommonPermissions.Comments.Edit)]
        public virtual async Task<CommentDto> UpdateAsync(Guid id, CommentUpdateDto input)
        {

            var comment = await _commentManager.UpdateAsync(
            id,
            input.FromUserId, input.DocId, input.Content, input.Url, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Comment, CommentDto>(comment);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(CommentExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _commentRepository.GetListAsync(input.FilterText, input.FromUserId, input.Content, input.DocId, input.Url);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Comment>, List<CommentExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Comments.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public virtual async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new CommentExcelDownloadTokenCacheItem { Token = token },
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