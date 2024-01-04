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
    public class CreateModalModel : CreateModalModelBase
    {
        public CreateModalModel(ICommentsAppService commentsAppService)
            : base(commentsAppService)
        {
        }
    }
}