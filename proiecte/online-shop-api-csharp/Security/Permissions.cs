namespace OnlineShopApi.Security;

public static class Permissions
{
    public const string ClaimType = "authorities";

    public const string UserRead = "USER_READ";
    public const string UserWrite = "USER_WRITE";
    public const string ProductRead = "PRODUCT_READ";
    public const string ProductWrite = "PRODUCT_WRITE";
    public const string OrderRead = "ORDER_READ";
    public const string OrderWrite = "ORDER_WRITE";
    public const string OrderDetailsRead = "ORDER_DETAILS_READ";
    public const string OrderDetailsWrite = "ORDER_DETAILS_WRITE";
    public const string PermissionRead = "PERMISSION_READ";
    public const string PermissionWrite = "PERMISSION_WRITE";

    public static readonly string[] All =
    [
        UserRead, UserWrite,
        ProductRead, ProductWrite,
        OrderRead, OrderWrite,
        OrderDetailsRead, OrderDetailsWrite,
        PermissionRead, PermissionWrite
    ];

    public static readonly string[] CustomerDefaults =
    [
        ProductRead,
        OrderRead, OrderWrite,
        OrderDetailsRead, OrderDetailsWrite
    ];
}
