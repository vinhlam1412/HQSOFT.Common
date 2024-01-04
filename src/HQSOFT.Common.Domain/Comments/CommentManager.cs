using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace HQSOFT.Common.Comments
{
    public abstract class CommentManagerBase : DomainService
    {
        protected ICommentRepository _commentRepository;

		public CommentManagerBase(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public virtual async Task<Comment> CreateAsync(
        Guid fromUserId, Guid docId, string? content = null, string? url = null)
        {

            var comment = new Comment(
             GuidGenerator.Create(),
             fromUserId, docId, content, url
             );

            return await _commentRepository.InsertAsync(comment);
        }

        public virtual async Task<Comment> UpdateAsync(
            Guid id,
            Guid fromUserId, Guid docId, string? content = null, string? url = null, [CanBeNull] string? concurrencyStamp = null
        )
        {

            var comment = await _commentRepository.GetAsync(id);

            comment.FromUserId = fromUserId;
            comment.DocId = docId;
            comment.Content = content;
            comment.Url = url;

            comment.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _commentRepository.UpdateAsync(comment);
        }

    }
}