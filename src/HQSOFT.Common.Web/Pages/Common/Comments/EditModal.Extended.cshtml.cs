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
    public class EditModalModel : EditModalModelBase
    {
        public EditModalModel(ICommentsAppService commentsAppService)
            : base(commentsAppService)
        {
        }
    }
}