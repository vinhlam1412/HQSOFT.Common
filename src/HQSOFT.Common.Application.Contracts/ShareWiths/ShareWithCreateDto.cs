using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HQSOFT.Common.ShareWiths
{
    public class ShareWithCreateDto
    {
        public Guid DocId { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanSubmit { get; set; }
        public bool CanShare { get; set; }
        public string? Url { get; set; }
        public Guid SharedToUserId { get; set; }
    }
}