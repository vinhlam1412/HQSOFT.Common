using Volo.Abp.Application.Dtos;
using System;

namespace HQSOFT.Common.Comments
{
    public abstract class CommentExcelDownloadDtoBase
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public Guid? FromUserId { get; set; }
        public string? Content { get; set; }
        public Guid? DocId { get; set; }
        public string? Url { get; set; }

        public CommentExcelDownloadDtoBase()
        {

        }
    }
}