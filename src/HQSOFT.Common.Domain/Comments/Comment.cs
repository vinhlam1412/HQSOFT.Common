using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace HQSOFT.Common.Comments
{
    public abstract class CommentBase : AuditedAggregateRoot<Guid>
    {
        public virtual Guid FromUserId { get; set; }

        [CanBeNull]
        public virtual string? Content { get; set; }

        public virtual Guid DocId { get; set; }

        [CanBeNull]
        public virtual string? Url { get; set; }

        protected CommentBase()
        {

        }

        public CommentBase(Guid id, Guid fromUserId, Guid docId, string? content = null, string? url = null)
        {

            Id = id;
            FromUserId = fromUserId;
            DocId = docId;
            Content = content;
            Url = url;
        }

    }
}