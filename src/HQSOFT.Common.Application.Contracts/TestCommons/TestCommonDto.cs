using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace HQSOFT.Common.TestCommons
{
    public class TestCommonDto : AuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int Idx { get; set; }
        public string ConcurrencyStamp { get; set; }
        public bool IsChanged { get; set; }
    }
}