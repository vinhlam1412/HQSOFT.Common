using Volo.Abp.Application.Dtos;
using System;

namespace HQSOFT.Common.ShareWiths
{
    public class GetShareWithsInput : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }

        public Guid? DocId { get; set; }
        public bool? CanRead { get; set; }
        public bool? CanWrite { get; set; }
        public bool? CanSubmit { get; set; }
        public bool? CanShare { get; set; }
        public string? Url { get; set; }
        public Guid? SharedToUserId { get; set; }

        public GetShareWithsInput()
        {

        }
    }
}