using System.ComponentModel.DataAnnotations;
using OnlineSchoolApi.Users.Models;

namespace OnlineSchoolApi.Users.Permissions.Dtos;

public record UpdateUserPermissionsRequest([Required, MinLength(1)] List<Permission>? Permissions);
