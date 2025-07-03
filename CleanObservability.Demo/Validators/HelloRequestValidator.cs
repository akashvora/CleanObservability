using CleanObservability.Demo.Models;
using FluentValidation;

namespace CleanObservability.Demo.Validators;

public class HelloRequestValidator : AbstractValidator<HelloRequest>
{
	public HelloRequestValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty()
			.WithMessage("Name must not be empty");
	}
}
