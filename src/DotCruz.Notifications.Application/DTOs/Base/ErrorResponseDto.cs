namespace DotCruz.Notifications.Application.DTOs.Base;

public class ErrorResponseDto
{
    public IEnumerable<string> Errors { get; set; }

    public ErrorResponseDto(IEnumerable<string> errors) => Errors = errors;

    public ErrorResponseDto(string error) => Errors = [error];
    public ErrorResponseDto() { }
}
