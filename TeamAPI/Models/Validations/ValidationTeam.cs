using FluentValidation;
using TeamAPI.Models;

public class ValidationTeam : AbstractValidator<Team>
{
    public ValidationTeam()
    {
        RuleFor(team => team.Name).NotNull();
        RuleFor(team => team.LeagueId).GreaterThan(0);
    }
}