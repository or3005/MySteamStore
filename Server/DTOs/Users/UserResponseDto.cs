using System;
using System.ComponentModel.DataAnnotations;


namespace Server.DTOs
{
    public record UserResponseDto(Guid Id, string UserName);
}