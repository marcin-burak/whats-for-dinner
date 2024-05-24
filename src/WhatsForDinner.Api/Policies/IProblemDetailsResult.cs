using Microsoft.AspNetCore.Mvc;

namespace WhatsForDinner.Api.Policies;

public interface IProblemDetailsResult
{
    ProblemDetails ToProblemDetails();
}
