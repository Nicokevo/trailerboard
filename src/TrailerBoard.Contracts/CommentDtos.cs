namespace TrailerBoard.Contracts;

public record AddCommentRequest(string Content);
public record CommentDto(string Author, string Content, DateTime CreatedAt);
