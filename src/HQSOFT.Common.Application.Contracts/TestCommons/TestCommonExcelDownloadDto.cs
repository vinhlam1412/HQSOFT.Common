using Volo.Abp.Application.Dtos;
using System;

namespace HQSOFT.Common.TestCommons
{
    public class TestCommonExcelDownloadDto
    {
        public string DownloadToken { get; set; }

        public string? FilterText { get; set; }

        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? IdxMin { get; set; }
        public int? IdxMax { get; set; }

        public TestCommonExcelDownloadDto()
        {

        }
    }
}