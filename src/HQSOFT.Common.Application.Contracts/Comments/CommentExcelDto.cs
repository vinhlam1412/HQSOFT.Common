using System;

namespace HQSOFT.Common.Comments
{
    public abstract class CommentExcelDtoBase
    {
        public Guid FromUserId { get; set; }
        public string? Content { get; set; }
        public Guid DocId { get; set; }
        public string? Url { get; set; }
    }
}