using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HQSOFT.Common.Comments
{
    public abstract class CommentCreateDtoBase
    {
        public Guid FromUserId { get; set; }
        public string? Content { get; set; }
        public Guid DocId { get; set; }
        public string? Url { get; set; }
    }
}