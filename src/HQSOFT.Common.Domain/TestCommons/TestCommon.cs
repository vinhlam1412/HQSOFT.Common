using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace HQSOFT.Common.TestCommons
{
    public class TestCommon : AuditedAggregateRoot<Guid>
    {
        [CanBeNull]
        public virtual string? Code { get; set; }

        [CanBeNull]
        public virtual string? Name { get; set; }

        public virtual int Idx { get; set; }

        public TestCommon()
        {

        }

        public TestCommon(Guid id, string code, string name, int idx)
        {

            Id = id;
            Code = code;
            Name = name;
            Idx = idx;
        }

    }
}