using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.Service.DTO_s;
using Zaker_Academy.Service.ErrorHandling;

namespace Zaker_Academy.Service.Interfaces
{
    public interface ICategoryService
    {
        Task<ServiceResult<CategoryCreationDto>> Create(CategoryCreationDto categoryCreationDto);
        Task<ServiceResult<SubCategoryCreationDto>> CreateSubCategory(int id, SubCategoryCreationDto categoryCreationDto);
        Task<ServiceResult<List<SubCategoryDto>>> GetAllSubCategories(int CategoryId);
        Task<ServiceResult<string>> Delete(int id);
        Task<ServiceResult<IEnumerable<CategoryDto>>> GetAll();
        Task<ServiceResult<CategoryDto>> GetById(int id);
        Task<ServiceResult<SubCategoryDto>> GetSubCategoryById(int id);
        Task<ServiceResult<string>> DeleteSubCategoryById(int id);

        Task<ServiceResult<CategoryDto>> Update(int id , CategoryCreationDto creationDto);
        Task<ServiceResult<SubCategoryDto>> Update(int id , SubCategoryCreationDto creationDto);
    }
}
