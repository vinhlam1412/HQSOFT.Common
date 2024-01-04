namespace HQSOFT.Common.TestCommons
{
    public static class TestCommonConsts
    {
        private const string DefaultSorting = "{0}Code asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "TestCommon." : string.Empty);
        }

    }
}