using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using HQSOFT.Common.Comments;
using Volo.Abp.Content;
using HQSOFT.Common.Shared;
using Microsoft.AspNetCore.Authentication;

namespace HQSOFT.Common.Comments
{
    [RemoteService(Name = "Common")]
    [Area("common")]
    [ControllerName("Comment")]
    [Route("api/common/comments")]
    public class CommentController : AbpController, ICommentsAppService
    {
        private readonly ICommentsAppService _commentsAppService;

        public CommentController(ICommentsAppService commentsAppService)
        {
            _commentsAppService = commentsAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<CommentDto>> GetListAsync(GetCommentsInput input)
        {
            return _commentsAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<CommentDto> GetAsync(Guid id)
        {
            return _commentsAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<CommentDto> CreateAsync(CommentCreateDto input)
        {
            return _commentsAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<CommentDto> UpdateAsync(Guid id, CommentUpdateDto input)
        {
            return _commentsAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _commentsAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(CommentExcelDownloadDto input)
        {
            return _commentsAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public virtual Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _commentsAppService.GetDownloadTokenAsync();
        }

        [HttpGet]
        public virtual async Task<ActionResult> ChallengeAsync(string returnUrl = "", string returnUrlHash = "")
        {
            await HttpContext.SignOutAsync();
            return Challenge(new AuthenticationProperties { RedirectUri = await GetRedirectUrlAsync(returnUrl, returnUrlHash) }, ChallengeAuthenticationSchemas);
        }
    }
}