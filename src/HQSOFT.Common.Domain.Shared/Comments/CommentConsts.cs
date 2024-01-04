namespace HQSOFT.Common.Comments
{
    public static class CommentConsts
    {
        private const string DefaultSorting = "{0}FromUserId asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Comment." : string.Empty);
        }

    }
}