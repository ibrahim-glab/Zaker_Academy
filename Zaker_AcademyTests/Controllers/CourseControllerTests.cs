using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaker_AcademyTests.Controllers
{
    internal class CourseControllerTests
    {
        [Fact]
public async Task Post_NullCategory_ReturnsBadRequest()
{
    // Arrange
    var controller = new YourController();
    
    // Act
    var result = await controller.Post(1, null);
    
    // Assert
    Assert.IsType<BadRequestResult>(result);
}

[Fact]
public async Task Post_UserNotInRoleAdmin_ReturnsForbid()
{
    // Arrange
    var controller = new YourController();
    controller.User.AddIdentity(new ClaimsIdentity(new List<Claim>()));
    
    // Act
    var result = await controller.Post(1, new SubCategoryCreationDto());
    
    // Assert
    Assert.IsType<ForbidResult>(result);
}

[Fact]
public async Task Post_InvalidCategoryName_ReturnsBadRequest()
{
    // Arrange
    var controller = new YourController();
    
    // Act
    var result = await controller.Post(1, new SubCategoryCreationDto { Name = "Invalid" });
    
    // Assert
    Assert.IsType<BadRequestResult>(result);
}

[Fact]
public async Task Post_CreateSubCategoryFailed_ReturnsNotFound()
{
    // Arrange
    var controller = new YourController();
    CategoryService.Setup(c => c.CreateSubCategory(It.IsAny<int>(), It.IsAny<SubCategoryCreationDto>()))
                   .ReturnsAsync(new OperationResult { Succeeded = false });
    
    // Act
    var result = await controller.Post(1, new SubCategoryCreationDto());
    
    // Assert
    Assert.IsType<NotFoundResult>(result);
}

[Fact]
public async Task Post_ExceptionThrown_ReturnsStatusCode500()
{
    // Arrange
    var controller = new YourController();
    CategoryService.Setup(c => c.CreateSubCategory(It.IsAny<int>(), It.IsAny<SubCategoryCreationDto>()))
                   .Throws(new Exception());
    
    // Act
    var result = await controller.Post(1, new SubCategoryCreationDto());
    
    // Assert
    Assert.IsType<StatusCodeResult>(result);
    var statusCodeResult = (StatusCodeResult)result;
    Assert.Equal(500, statusCodeResult.StatusCode);
}
    }
}
