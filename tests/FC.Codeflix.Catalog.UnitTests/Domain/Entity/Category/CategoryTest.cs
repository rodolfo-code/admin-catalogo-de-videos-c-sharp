using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using System.Xml.Linq;
using Xunit;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        //Arrange
        var validData = new
        {
            Name = "Category name",
            Description = "category description"
        };

        var datetimeBefore = DateTime.Now;

        //Act
        var category = new DomainEntity.Category(validData.Name, validData.Description);
        var datetimeAfter = DateTime.Now;

        //Assert

        //Assert.NotNull(category);
        //Assert.Equal(validData.Name, category.Name);
        //Assert.Equal(validData.Description, category.Description);
        //Assert.NotEqual(default(Guid), category.Id);
        //Assert.NotEqual(default(DateTime), category.CreatedAt);
        //Assert.True(category.CreatedAt > datetimeBefore);
        //Assert.True(category.CreatedAt < datetimeAfter);
        //Assert.True(category.IsActive);

        //Fluent Assertions

        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));

        (category.CreatedAt > datetimeBefore).Should().BeTrue();
        (category.CreatedAt < datetimeAfter).Should().BeTrue();
        (category.IsActive).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        //Arrange
        var validData = new 
        {
            Name = "Category name",
            Description = "category description"
        };

        var datetimeBefore = DateTime.Now;

        //Act
        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
        var datetimeAfter = DateTime.Now;

        //Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt > datetimeBefore).Should().BeTrue();
        (category.CreatedAt < datetimeAfter).Should().BeTrue();
        (category.IsActive).Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(InstantiateThrowWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void InstantiateThrowWhenNameIsEmpty(string? name)
    {
        Action action = () => new DomainEntity.Category(name!, "Category Description");


        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");

        //var exception = Assert.Throws<EntityValidationException>(action);

        //Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        Action action = () => new DomainEntity.Category("Name", null!);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Description should not be null");

        //var exception = Assert.Throws<EntityValidationException>(action);
        //Assert.Equal("Description should not be null", exception.Message);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregate")]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData("1")]
    [InlineData("12")]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        Action action = () => new DomainEntity.Category(invalidName, "Category ok Description");

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should be at least 3 characters long");

        //var exception = Assert.Throws<EntityValidationException>(action);
        //Assert.Equal("Name should be at least 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregate")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        Action action = () => new DomainEntity.Category(invalidName, "Category ok Description");

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescritionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregate")]
    public void InstantiateErrorWhenDescritionIsGreaterThan10_000Characters()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        Action action = () => new DomainEntity.Category("Category name", invalidDescription);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Description should be less or equal 10.000 characters long");
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        //Arrange
        var validData = new
        {
            Name = "Category name",
            Description = "category description"
        };

    
        //Act
        var category = new DomainEntity.Category(validData.Name, validData.Description, false);
        category.Activate();

        //Assert
        category.IsActive.Should().BeTrue();
        //Assert.True(category.IsActive);
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        //Arrange
        var validData = new
        {
            Name = "Category name",
            Description = "category description"
        };


        //Act
        var category = new DomainEntity.Category(validData.Name, validData.Description, true);
        category.Deactivate();

        //Assert
        category.IsActive.Should().BeFalse();
        //Assert.False(category.IsActive);
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var category = new DomainEntity.Category("Category name", "category description");

        var newValues = new { Name = "New name", Description = "New description" };

        category.Update(newValues.Name, newValues.Description);

        category.Name.Should().Be(newValues.Name);
        category.Description.Should().Be(newValues.Description);

        //Assert.Equal(category.Name, newValues.Name);
        //Assert.Equal(category.Description, newValues.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var category = new DomainEntity.Category("Category name", "category description");

        var newValues = new { Name = "New name"};
        var currentDescription = category.Description;

        category.Update(newValues.Name);

        category.Name.Should().Be(newValues.Name);
        category.Description.Should().Be(currentDescription);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var category = new DomainEntity.Category("Category name", "category description");
        Action action = () => category.Update(name!);

        //var exception = Assert.Throws<EntityValidationException>(action);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregate")]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData("1")]
    [InlineData("12")]
    public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var category = new DomainEntity.Category("Category name", "category description");
        Action action = () => category.Update(invalidName);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should be at least 3 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregate")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        var category = new DomainEntity.Category("Category name", "category description");
        Action action = () => category.Update(invalidName);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescritionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregate")]
    public void UpdateErrorWhenDescritionIsGreaterThan10_000Characters()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        var category = new DomainEntity.Category("Category name", "category description");
        Action action = () => category.Update("Category name", invalidDescription);

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
    }
}
