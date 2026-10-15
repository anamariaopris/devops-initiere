namespace OnlineSchoolApi.Users;

public enum Permission
{
    STUDENT_READ,
    STUDENT_WRITE,
    TEACHER_READ,
    TEACHER_WRITE,
    ADMINISTRATOR_READ,
    ADMINISTRATOR_WRITE,
    USER_READ,
    USER_WRITE,
    COURSE_READ,
    COURSE_WRITE,
    BOOK_READ,
    BOOK_WRITE,
    ENROLMENT_READ,
    ENROLMENT_WRITE,
    PROFILE_READ,
    PROFILE_WRITE,
    AUTH_READ,
    AUTH_WRITE,
    PASSWORD_READ,
    PASSWORD_WRITE,
    PERMISSION_READ,
    PERMISSION_WRITE,
    DASHBOARD_READ,
    DASHBOARD_WRITE,
    REPORT_READ,
    REPORT_WRITE,
    SETTINGS_READ,
    SETTINGS_WRITE
}

public static class PermissionExtensions
{
    public static string ToAuthority(this Permission permission) =>
        permission.ToString().ToLowerInvariant().Replace('_', ':');
}

public static class PermissionTemplates
{
    public static readonly Permission[] Student =
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

    public static readonly Permission[] Teacher =
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

    public static readonly Permission[] Administrator = Enum.GetValues<Permission>();
}
