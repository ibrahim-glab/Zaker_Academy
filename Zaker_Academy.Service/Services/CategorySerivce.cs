using AutoMapper;
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
        private readonly IMapper mapper;
        public CategorySerivce(IUnitOfWork unitOfWork ,IMapper mapper )
        {
            _unitofwork = unitOfWork;
            this.mapper = mapper;
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

        public async Task<ServiceResult<IEnumerable<CategoryDto>>> GetAll()
        {
            try
            {
              var categories =   await _unitofwork.CategoryRepository.GetAll();
              var categoryDtos = categories.Select(s => new CategoryDto { Id = s.Id, Name = s.Name }).ToList();
              return new ServiceResult<IEnumerable<CategoryDto>> { Data = categoryDtos, succeeded = true };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceResult<CategoryDto>> GetById(int id)
        {
            try
            {
                var category =mapper.Map<CategoryDto>( await _unitofwork.CategoryRepository.GetByIdAsync(id));
                
                return new ServiceResult<CategoryDto> { Data = category, succeeded = true };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceResult<CategoryDto>> Update(int id, CategoryCreationDto creationDto)
        {
            try
            {
                var category = await _unitofwork.CategoryRepository.GetByIdAsync(id);
                if (category is null)
                    return new ServiceResult<CategoryDto> { succeeded = false };
                category.Name = creationDto.Name;
                await _unitofwork.SaveChanges();
                return new ServiceResult<CategoryDto> { Data = mapper.Map<CategoryDto>(category), succeeded = true };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
