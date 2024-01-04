using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using HQSOFT.Common.EntityFrameworkCore;

namespace HQSOFT.Common.Comments
{
    public class EfCoreCommentRepository : EfCoreCommentRepositoryBase, ICommentRepository
    {
        public EfCoreCommentRepository(IDbContextProvider<CommonDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}