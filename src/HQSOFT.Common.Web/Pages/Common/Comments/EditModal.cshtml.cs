using HQSOFT.Common.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using HQSOFT.Common.Comments;

namespace HQSOFT.Common.Web.Pages.Common.Comments
{
    public abstract class EditModalModelBase : CommonPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public CommentUpdateViewModel Comment { get; set; }

        protected ICommentsAppService _commentsAppService;

        public EditModalModelBase(ICommentsAppService commentsAppService)
        {
            _commentsAppService = commentsAppService;

            Comment = new();
        }

        public virtual async Task OnGetAsync()
        {
            var comment = await _commentsAppService.GetAsync(Id);
            Comment = ObjectMapper.Map<CommentDto, CommentUpdateViewModel>(comment);

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {

            await _commentsAppService.UpdateAsync(Id, ObjectMapper.Map<CommentUpdateViewModel, CommentUpdateDto>(Comment));
            return NoContent();
        }
    }

    public class CommentUpdateViewModel : CommentUpdateDto
    {
    }
}