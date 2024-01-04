using HQSOFT.Common.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HQSOFT.Common.Comments;

namespace HQSOFT.Common.Web.Pages.Common.Comments
{
    public abstract class CreateModalModelBase : CommonPageModel
    {
        [BindProperty]
        public CommentCreateViewModel Comment { get; set; }

        protected ICommentsAppService _commentsAppService;

        public CreateModalModelBase(ICommentsAppService commentsAppService)
        {
            _commentsAppService = commentsAppService;

            Comment = new();
        }

        public virtual async Task OnGetAsync()
        {
            Comment = new CommentCreateViewModel();

            await Task.CompletedTask;
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {

            await _commentsAppService.CreateAsync(ObjectMapper.Map<CommentCreateViewModel, CommentCreateDto>(Comment));
            return NoContent();
        }
    }

    public class CommentCreateViewModel : CommentCreateDto
    {
    }
}