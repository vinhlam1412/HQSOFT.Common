using Volo.Abp.Reflection;

namespace HQSOFT.Common.Permissions;

public class CommonPermissions
{
    public const string GroupName = "Common";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(CommonPermissions));
    }

    public static class TestCommons
    {
        public const string Default = GroupName + ".TestCommons";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class AssignedTos
    {
        public const string Default = GroupName + ".AssignedTos";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class TaskAssignments
    {
        public const string Default = GroupName + ".TaskAssignments";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class ShareWiths
    {
        public const string Default = GroupName + ".ShareWiths";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class Notifications
    {
        public const string Default = GroupName + ".Notifications";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class Comments
    {
        public const string Default = GroupName + ".Comments";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
}