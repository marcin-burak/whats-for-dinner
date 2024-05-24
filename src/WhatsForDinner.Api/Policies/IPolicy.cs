namespace WhatsForDinner.Api.Policies;

public interface IPolicy<TInput, TOutput>
{
    TOutput Enforce(TInput input);
}