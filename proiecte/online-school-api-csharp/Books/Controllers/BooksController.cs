using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnlineSchoolApi.Books.Dtos;
using OnlineSchoolApi.Books.Services;
using OnlineSchoolApi.Common.Validation;

namespace OnlineSchoolApi.Books.Controllers;

[ApiController]
[Route("books")]
[Tags("Books")]
public class BooksController(IBookCommandService bookCommandService, IBookQueryService bookQueryService) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Create book")]
    [EndpointDescription("Required permission: book:write")]
    [ProducesResponseType(typeof(BookResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<BookResponse>> CreateBook([FromBody] BookCreateRequest request, CancellationToken cancellationToken)
        => StatusCode(StatusCodes.Status201Created, await bookCommandService.CreateBookAsync(request, cancellationToken));

    [HttpGet]
    [EndpointSummary("Get all books")]
    [EndpointDescription("Required permission: book:read")]
    public async Task<ActionResult<List<BookResponse>>> GetAllBooks(CancellationToken cancellationToken)
        => Ok(await bookQueryService.GetAllBooksAsync(cancellationToken));

    [HttpGet("{bookId}")]
    [EndpointSummary("Get book by id")]
    [EndpointDescription("Required permission: book:read")]
    public async Task<ActionResult<BookResponse>> GetBookById([FromRoute, Positive] long bookId, CancellationToken cancellationToken)
        => Ok(await bookQueryService.GetBookByIdAsync(bookId, cancellationToken));

    [HttpGet("find-by-student")]
    [EndpointSummary("Find books by student")]
    [EndpointDescription("Required permission: book:read")]
    public async Task<ActionResult<List<BookResponse>>> FindByStudent([FromQuery, BindRequired, Positive] long studentId, CancellationToken cancellationToken)
        => Ok(await bookQueryService.FindByStudentIdAsync(studentId, cancellationToken));

    [HttpGet("search")]
    [EndpointSummary("Search books")]
    [EndpointDescription("Required permission: book:read")]
    public async Task<ActionResult<List<BookResponse>>> SearchBooks([FromQuery(Name = "q")] string? query, CancellationToken cancellationToken)
        => Ok(await bookQueryService.SearchBooksAsync(query, cancellationToken));

    [HttpPut("{bookId}")]
    [EndpointSummary("Update book")]
    [EndpointDescription("Required permission: book:write")]
    public async Task<ActionResult<BookResponse>> UpdateBook([FromRoute, Positive] long bookId, [FromBody] BookUpdateRequest request, CancellationToken cancellationToken)
        => Ok(await bookCommandService.UpdateBookAsync(bookId, request, cancellationToken));

    [HttpPatch("{bookId}")]
    [EndpointSummary("Patch book")]
    [EndpointDescription("Required permission: book:write")]
    public async Task<ActionResult<BookResponse>> PatchBook([FromRoute, Positive] long bookId, [FromBody] BookUpdateRequest request, CancellationToken cancellationToken)
        => Ok(await bookCommandService.UpdateBookAsync(bookId, request, cancellationToken));

    [HttpDelete("{bookId}")]
    [EndpointSummary("Delete book")]
    [EndpointDescription("Required permission: book:write")]
    public async Task<ActionResult<BookResponse>> DeleteBook([FromRoute, Positive] long bookId, CancellationToken cancellationToken)
        => Ok(await bookCommandService.DeleteBookAsync(bookId, cancellationToken));
}
