using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.Common.ShareWiths
{
    public class ShareWithDto : AuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public Guid DocId { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanSubmit { get; set; }
        public bool CanShare { get; set; }
        public string? Url { get; set; }
        public Guid SharedToUserId { get; set; }
        public string ConcurrencyStamp { get; set; }
        public bool IsChanged { get; set; }
    }
}