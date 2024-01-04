namespace HQSOFT.Common.ShareWiths
{
    public static class ShareWithConsts
    {
        private const string DefaultSorting = "{0}DocId asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "ShareWith." : string.Empty);
        }

    }
}