using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Zaker_Academy.core.Entities;
namespace Zaker_Academy.Controllers;

public static class SubCategoryEndpoints
{
    public static void MapSubCategoryEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/SubCategory").WithTags(nameof(SubCategory));

        group.MapGet("/", () =>
        {
            return new [] { new SubCategory() };
        })
        .WithName("GetAllSubCategories")
        .WithOpenApi();

        group.MapGet("/{id}", (int id) =>
        {
            //return new SubCategory { ID = id };
        })
        .WithName("GetSubCategoryById")
        .WithOpenApi();

        group.MapPut("/{id}", (int id, SubCategory input) =>
        {
            return TypedResults.NoContent();
        })
        .WithName("UpdateSubCategory")
        .WithOpenApi();

        group.MapPost("/", (SubCategory model) =>
        {
            //return TypedResults.Created($"/api/SubCategories/{model.ID}", model);
        })
        .WithName("CreateSubCategory")
        .WithOpenApi();

        group.MapDelete("/{id}", (int id) =>
        {
            //return TypedResults.Ok(new SubCategory { ID = id });
        })
        .WithName("DeleteSubCategory")
        .WithOpenApi();
    }
}
