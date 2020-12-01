using FluentValidation;

namespace Blackdot.InvestigationSearch.Validators
{
	/// <summary>
	/// For validating a search term
	/// </summary>
    public class SearchTermValidator : AbstractValidator<string>
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public SearchTermValidator()
		{
			RuleFor(x => x).NotNull().NotEmpty().WithMessage("Please provide a search term. A search term cannot be null or empty.");
			RuleFor(x => x).MinimumLength(3).WithMessage("Please provide a search term with a minimum of 3 characters.");
		}
	}
}
