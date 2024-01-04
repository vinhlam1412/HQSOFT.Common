using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace HQSOFT.Common.TestCommons
{
    public class TestCommonManager : DomainService
    {
        private readonly ITestCommonRepository _testCommonRepository;

        public TestCommonManager(ITestCommonRepository testCommonRepository)
        {
            _testCommonRepository = testCommonRepository;
        }

        public async Task<TestCommon> CreateAsync(
        string code, string name, int idx)
        {

            var testCommon = new TestCommon(
             GuidGenerator.Create(),
             code, name, idx
             );

            return await _testCommonRepository.InsertAsync(testCommon);
        }

        public async Task<TestCommon> UpdateAsync(
            Guid id,
            string code, string name, int idx, [CanBeNull] string concurrencyStamp = null
        )
        {

            var testCommon = await _testCommonRepository.GetAsync(id);

            testCommon.Code = code;
            testCommon.Name = name;
            testCommon.Idx = idx;

            testCommon.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _testCommonRepository.UpdateAsync(testCommon);
        }

    }
}