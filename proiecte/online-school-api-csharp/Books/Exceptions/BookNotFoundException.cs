using OnlineSchoolApi.Common.Exceptions;

namespace OnlineSchoolApi.Books.Exceptions;

public class BookNotFoundException(long bookId) : NotFoundException($"Book not found with id={bookId}");
