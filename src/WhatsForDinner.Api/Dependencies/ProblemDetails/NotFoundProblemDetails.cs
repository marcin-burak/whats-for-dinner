namespace WhatsForDinner.Api.Dependencies.ProblemDetails;

public sealed class NotFoundProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
{
    public NotFoundProblemDetails(string title, string detail)
    {
        Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.4";
        Status = 404;
        Title = title;
        Detail = detail;
    }
}
