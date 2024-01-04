using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.Common.Comments
{
    public abstract class CommentDtoBase : AuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public Guid FromUserId { get; set; }
        public string? Content { get; set; }
        public Guid DocId { get; set; }
        public string? Url { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

        public bool IsChanged { get; set; }
    }
}