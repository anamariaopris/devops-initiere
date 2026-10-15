namespace OnlineSchoolApi.Common.Extensions;

public static class IdExtensions
{
    public static int? ToEntityId(this long id) => id is >= int.MinValue and <= int.MaxValue ? (int)id : null;
}
