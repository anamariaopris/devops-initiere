namespace OnlineSchoolApi.Users.Models;

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
    public static string ToAuthority(this Permission permission)
        => permission.ToString().ToLowerInvariant().Replace('_', ':');
}
