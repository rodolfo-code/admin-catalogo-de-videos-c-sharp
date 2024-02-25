using FC.Codeflix.Catalog.Domain.Entity;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Moq;
using System.Threading;
using Xunit;
using UseCases =  FC.Codeflix.Catalog.Application.UseCases.CreateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory;
public class CreateCategoryTest
{
    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategory()
    {
        var repositoryMock = new Mock<ICategoryRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var useCase = new UseCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        var input = new CreateCategoryInput(
                "category Name",
                "category description",
                true
            );

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Create(
                It.IsAny<Category>(), 
                It.IsAny<CancellationToken>()
                ), 
            Times.Once
            );

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.Commit(
                It.IsAny<CancellationToken>()
                ), 
            Times.Once
            );

        output.Should().NotBeNull();
        output.Name.Should().Be("category Name");
        output.Description.Should().Be("category description");
        (output.Id != null && output.Id != Guid.Empty).Should().BeTrue();
        (output.CreatedAt != null && output.CreatedAt != default(DateTime)).Should().BeTrue();
    }
}
