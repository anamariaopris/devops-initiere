using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace OnlineSchoolApi.Books;

[ApiController]
[Route("books")]
public class BooksController(BookService service) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "book:read")]
    public Task<List<BookResponse>> GetAll(CancellationToken cancellationToken) =>
        service.GetAllAsync(cancellationToken);

    [HttpGet("{bookId}")]
    [Authorize(Policy = "book:read")]
    public Task<BookResponse> GetById([Range(1, long.MaxValue)] long bookId, CancellationToken cancellationToken) =>
        service.GetByIdAsync(bookId, cancellationToken);

    [HttpGet("find-by-student")]
    [Authorize(Policy = "book:read")]
    public Task<List<BookResponse>> FindByStudent([BindRequired, Range(1, long.MaxValue)] long studentId, CancellationToken cancellationToken) =>
        service.FindByStudentAsync(studentId, cancellationToken);

    [HttpGet("search")]
    [Authorize(Policy = "book:read")]
    public Task<List<BookResponse>> Search([FromQuery(Name = "q")] string? query, CancellationToken cancellationToken) =>
        service.SearchAsync(query, cancellationToken);

    [HttpPost]
    [Authorize(Policy = "book:write")]
    public async Task<ActionResult<BookResponse>> Create(BookCreateRequest request, CancellationToken cancellationToken)
    {
        var book = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { bookId = book.Id }, book);
    }

    [HttpPut("{bookId}")]
    [Authorize(Policy = "book:write")]
    public Task<BookResponse> Update([Range(1, long.MaxValue)] long bookId, BookUpdateRequest request, CancellationToken cancellationToken) =>
        service.UpdateAsync(bookId, request, cancellationToken);

    [HttpPatch("{bookId}")]
    [Authorize(Policy = "book:write")]
    public Task<BookResponse> Patch([Range(1, long.MaxValue)] long bookId, BookUpdateRequest request, CancellationToken cancellationToken) =>
        service.UpdateAsync(bookId, request, cancellationToken);

    [HttpDelete("{bookId}")]
    [Authorize(Policy = "book:write")]
    public Task<BookResponse> Delete([Range(1, long.MaxValue)] long bookId, CancellationToken cancellationToken) =>
        service.DeleteAsync(bookId, cancellationToken);
}
