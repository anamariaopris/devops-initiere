using System.ComponentModel.DataAnnotations;

namespace OnlineSchoolApi.Books.Dtos;

public record BookCreateRequest(
    [Required, StringLength(50, MinimumLength = 3)] string? BookName,
    long? StudentId
);
