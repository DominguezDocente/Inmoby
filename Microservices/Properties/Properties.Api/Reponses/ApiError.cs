namespace Properties.Api.Reponses
{
    public sealed record ApiError(string Code, string Description, string Type);
}
