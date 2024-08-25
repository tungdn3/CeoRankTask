using FluentValidation;
using SeoRankTask.Core.Enums;

namespace CeoRankTask.Core.Dtos;

public class SeoRankRequestDto
{
    public string Keyword { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public SearchEngine SearchEngine { get; set; }
}

public class CeoRankRequestDtoValidator : AbstractValidator<SeoRankRequestDto>
{
    private const string UrlOptionalSchemeReg = @"^(http:\/\/|https:\/\/)?([a-zA-Z0-9-_]+\.)*[a-zA-Z0-9][a-zA-Z0-9-_]+(\.[a-zA-Z]{2,11})";

    public CeoRankRequestDtoValidator()
    {
        RuleFor(x => x.Keyword)
            .NotEmpty();

        RuleFor(x => x.Url)
            .NotEmpty()
            .Matches(UrlOptionalSchemeReg)
                .WithMessage("Url is not valid.");

        RuleFor(x => x.SearchEngine)
            .IsInEnum();
    }
}