using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.Common.Comments
{
    public abstract class CommentUpdateDtoBase : AuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public Guid FromUserId { get; set; }
        public string? Content { get; set; }
        public Guid DocId { get; set; }
        public string? Url { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

        public bool IsChanged { get; set; }
    }
}