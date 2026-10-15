namespace OnlineShopApi.Exceptions;

public class AdminNotFoundException() : ApiException(404, "Admin not found", "Admin does not exist in the database.");
