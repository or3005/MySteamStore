namespace Server.DTOs
{
    // What the client receives - never send the EF entity itself (it drags User navigations with it).
    public record MessageDto(Guid Id, Guid SenderId, Guid ReceiverId, string Content, DateTime CreateAt);
}
