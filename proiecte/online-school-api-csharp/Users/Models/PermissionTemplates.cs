namespace OnlineSchoolApi.Users.Models;

public static class PermissionTemplates
{
    private static readonly Permission[] StudentPermissions =
    [
        Permission.STUDENT_READ,
        Permission.COURSE_READ,
        Permission.BOOK_READ,
        Permission.BOOK_WRITE,
        Permission.ENROLMENT_READ,
        Permission.PROFILE_READ,
        Permission.PROFILE_WRITE,
        Permission.AUTH_READ,
        Permission.AUTH_WRITE,
        Permission.PASSWORD_READ,
        Permission.PASSWORD_WRITE,
        Permission.DASHBOARD_READ
    ];

    private static readonly Permission[] TeacherPermissions =
    [
        Permission.TEACHER_READ,
        Permission.TEACHER_WRITE,
        Permission.STUDENT_READ,
        Permission.COURSE_READ,
        Permission.COURSE_WRITE,
        Permission.BOOK_READ,
        Permission.ENROLMENT_READ,
        Permission.ENROLMENT_WRITE,
        Permission.PROFILE_READ,
        Permission.PROFILE_WRITE,
        Permission.AUTH_READ,
        Permission.AUTH_WRITE,
        Permission.PASSWORD_READ,
        Permission.PASSWORD_WRITE,
        Permission.DASHBOARD_READ,
        Permission.DASHBOARD_WRITE,
        Permission.REPORT_READ
    ];

    public static IReadOnlyList<Permission> StudentDefaults() => StudentPermissions;

    public static IReadOnlyList<Permission> TeacherDefaults() => TeacherPermissions;

    public static IReadOnlyList<Permission> AdministratorDefaults() => Enum.GetValues<Permission>();
}
