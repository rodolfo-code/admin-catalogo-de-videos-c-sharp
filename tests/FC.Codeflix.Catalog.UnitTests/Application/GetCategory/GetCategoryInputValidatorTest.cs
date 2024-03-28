using FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FC.Codeflix.Catalog.UnitTests.Application.GetCategory;

[Collection(nameof(GetCategoryTestFixture))]
public class GetCategoryInputValidatorTest
{
    private readonly GetCategoryTestFixture _fixture;

    public GetCategoryInputValidatorTest(GetCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ValidationOK))]
    [Trait("Application", "GetCategoryInputValidation - UseCases")]
    public void ValidationOK()
    {
        var validInput = new GetCategoryInput(Guid.NewGuid());
        var validator = new GetCategoryInputValidator();

        var validationResult = validator.Validate(validInput);

        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeTrue();
        validationResult.Errors.Should().HaveCount(0);
    }

    [Fact(DisplayName = nameof(InvalidWhenEmptyGuidId))]
    [Trait("Application", "GetCategoryInputValidation - UseCases")]
    public void InvalidWhenEmptyGuidId()
    {
        var invalidIalidInput = new GetCategoryInput(Guid.Empty);
        var validator = new GetCategoryInputValidator();

        var validationResult = validator.Validate(invalidIalidInput);

        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().HaveCount(1);
        validationResult.Errors[0].ErrorMessage.Should().Be("'Id' deve ser informado.");
    }
}
