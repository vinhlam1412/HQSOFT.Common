using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace HQSOFT.Common.ShareWiths
{
    public class ShareWith : AuditedAggregateRoot<Guid>
    {
        public virtual Guid DocId { get; set; }

        public virtual bool CanRead { get; set; }

        public virtual bool CanWrite { get; set; }

        public virtual bool CanSubmit { get; set; }

        public virtual bool CanShare { get; set; }

        [CanBeNull]
        public virtual string? Url { get; set; }

        public virtual Guid SharedToUserId { get; set; }

        public ShareWith()
        {

        }

        public ShareWith(Guid id, Guid docId, bool canRead, bool canWrite, bool canSubmit, bool canShare, string url, Guid sharedToUserId)
        {

            Id = id;
            DocId = docId;
            CanRead = canRead;
            CanWrite = canWrite;
            CanSubmit = canSubmit;
            CanShare = canShare;
            Url = url;
            SharedToUserId = sharedToUserId;
        }

    }
}