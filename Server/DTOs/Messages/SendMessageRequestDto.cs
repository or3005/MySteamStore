namespace Server.DTOs
{
    // No SenderId here on purpose: the server decides who the sender is, the client can't claim it.
    public record SendMessageRequestDto(Guid ReceiverId, string Content);
}
