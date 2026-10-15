namespace OneModelApi.Exceptions;

public class InvalidYearRangeException(int minYear, int maxYear)
    : ApiException($"Invalid year range: minYear ({minYear}) must be <= maxYear ({maxYear})", 400);
