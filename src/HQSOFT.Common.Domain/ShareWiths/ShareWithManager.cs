using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace HQSOFT.Common.ShareWiths
{
    public class ShareWithManager : DomainService
    {
        private readonly IShareWithRepository _shareWithRepository;

        public ShareWithManager(IShareWithRepository shareWithRepository)
        {
            _shareWithRepository = shareWithRepository;
        }

        public async Task<ShareWith> CreateAsync(
        Guid docId, bool canRead, bool canWrite, bool canSubmit, bool canShare, string url, Guid sharedToUserId)
        {

            var shareWith = new ShareWith(
             GuidGenerator.Create(),
             docId, canRead, canWrite, canSubmit, canShare, url, sharedToUserId
             );

            return await _shareWithRepository.InsertAsync(shareWith);
        }

        public async Task<ShareWith> UpdateAsync(
            Guid id,
            Guid docId, bool canRead, bool canWrite, bool canSubmit, bool canShare, string url, Guid sharedToUserId, [CanBeNull] string concurrencyStamp = null
        )
        {

            var shareWith = await _shareWithRepository.GetAsync(id);

            shareWith.DocId = docId;
            shareWith.CanRead = canRead;
            shareWith.CanWrite = canWrite;
            shareWith.CanSubmit = canSubmit;
            shareWith.CanShare = canShare;
            shareWith.Url = url;
            shareWith.SharedToUserId = sharedToUserId;

            shareWith.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _shareWithRepository.UpdateAsync(shareWith);
        }

    }
}