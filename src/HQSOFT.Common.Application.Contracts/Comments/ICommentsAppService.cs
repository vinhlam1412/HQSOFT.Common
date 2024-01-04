using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using HQSOFT.Common.Shared;

namespace HQSOFT.Common.Comments
{
    public partial interface ICommentsAppService : IApplicationService
    {
        Task<PagedResultDto<CommentDto>> GetListAsync(GetCommentsInput input);

        Task<CommentDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<CommentDto> CreateAsync(CommentCreateDto input);

        Task<CommentDto> UpdateAsync(Guid id, CommentUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(CommentExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}