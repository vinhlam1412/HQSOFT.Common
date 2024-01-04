using Volo.Abp.Application.Dtos;
using System;

namespace HQSOFT.Common.Comments
{
    public abstract class GetCommentsInputBase : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }

        public Guid? FromUserId { get; set; }
        public string? Content { get; set; }
        public Guid? DocId { get; set; }
        public string? Url { get; set; }

        public GetCommentsInputBase()
        {

        }
    }
}