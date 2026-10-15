namespace OneModelApi.Exceptions;

public class EmptyPatchRequestException()
    : ApiException("Patch request must contain at least one field", 400);
