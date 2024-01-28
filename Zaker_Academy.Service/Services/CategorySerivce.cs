using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.core.Interfaces;
using Zaker_Academy.infrastructure.Entities;
using Zaker_Academy.Service.DTO_s;
using Zaker_Academy.Service.ErrorHandling;
using Zaker_Academy.Service.Interfaces;

namespace Zaker_Academy.Service.Services
{
    public class CategorySerivce : ICategoryService
    {
        private readonly IUnitOfWork _unitofwork;
        public CategorySerivce(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }

        public async Task<ServiceResult<CategoryCreationDto>> Create(CategoryCreationDto categoryCreationDto)
        {
            try
            {
                await _unitofwork.CategoryRepository.Add(new Category { Name = categoryCreationDto.Name });
                return new ServiceResult<CategoryCreationDto> { Data = categoryCreationDto  , succeeded = true};
            }
            catch (Exception)
            {
                throw;
            }
          
          
        }
    }
}
