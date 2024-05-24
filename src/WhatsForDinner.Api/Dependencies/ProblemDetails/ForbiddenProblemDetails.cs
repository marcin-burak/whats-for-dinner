namespace WhatsForDinner.Api.Dependencies.ProblemDetails;

public sealed class ForbiddenProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
{
    public ForbiddenProblemDetails(string title, string detail)
    {
        Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.3";
        Status = 403;
        Title = title;
        Detail = detail;
    }
}