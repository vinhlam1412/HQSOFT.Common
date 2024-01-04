namespace HQSOFT.Common.TaskAssignments
{
    public static class TaskAssignmentConsts
    {
        private const string DefaultSorting = "{0}DocId asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "TaskAssignment." : string.Empty);
        }

    }
}