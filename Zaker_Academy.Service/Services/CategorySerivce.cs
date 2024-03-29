﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.core.Entities;
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

        public async Task<ServiceResult<SubCategoryCreationDto>> CreateSubCategory(int id, SubCategoryCreationDto categoryCreationDto)
        {
            try
            {
                var category = await _unitofwork.CategoryRepository.GetByIdAsync(id);
                if (category is null)
                    return new ServiceResult<SubCategoryCreationDto> { succeeded = false };
                
                await _unitofwork.SubCategoryRepository.Add(new SubCategory { Name = categoryCreationDto.Name, Category = category, CategoryId = id });
                return new ServiceResult<SubCategoryCreationDto> { succeeded = true , Data =  categoryCreationDto };
            }
            catch (Exception)
            {

                throw;
            }
               
        }
            

        public async Task<ServiceResult<string>> Delete(int id)
        {
            try
            {
                var category = await _unitofwork.CategoryRepository.GetByIdAsync(id);
                if (category is null)
                    return new ServiceResult<string> { succeeded = false };
                await _unitofwork.CategoryRepository.Delete(category);
                await _unitofwork.SaveChanges();
                return new ServiceResult<string> { succeeded = true };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceResult<string>> DeleteSubCategoryById(int id)
        {
            try
            {
                var category = await _unitofwork.SubCategoryRepository.GetByIdAsync(id);
                if (category is null)
                    return new ServiceResult<string> { succeeded = false };
                await _unitofwork.SubCategoryRepository.Delete(category);
                await _unitofwork.SaveChanges();
                return new ServiceResult<string> { succeeded = true };
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
              var categories =   await _unitofwork.CategoryRepository.GetByCondition(s=>1==1 ,new string[] { "SubCategories" });
              var categoryDtos = categories.Select(s => new CategoryDto { Id = s.Id, Name = s.Name , subCategories = s.SubCategories?.Select(s=>new SubCategoryDto { Id = s.Id , Name = s.Name} ).ToList() }).ToList();
               
               
              return new ServiceResult<IEnumerable<CategoryDto>> { Data = categoryDtos, succeeded = true };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceResult<List<SubCategoryDto>>> GetAllSubCategories(int CategoryId)
        {
            var categories = await _unitofwork.SubCategoryRepository.GetByCondition(s => s.CategoryId == CategoryId);
            if (categories.Count() == 0)
                return new ServiceResult<List<SubCategoryDto>> { succeeded = false };
            var subcategoriesDto = categories.Select(s=> mapper.Map<SubCategoryDto>(s)).ToList();
            return new ServiceResult<List<SubCategoryDto>> { Data = subcategoriesDto, succeeded = true };
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

        public async Task<ServiceResult<SubCategoryDto>> GetSubCategoryById(int id)
        {
            try
            {
                var subcategory = mapper.Map<SubCategoryDto>(await _unitofwork.SubCategoryRepository.GetByIdAsync(id));

                return new ServiceResult<SubCategoryDto> { Data = subcategory, succeeded = true };
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

        public async Task<ServiceResult<SubCategoryDto>> Update(int id, SubCategoryCreationDto creationDto)
        {
            try
            {
                var category = await _unitofwork.SubCategoryRepository.GetByIdAsync(id);
                if (category is null)
                    return new ServiceResult<SubCategoryDto> { succeeded = false };
                category.Name = creationDto.Name;
                await _unitofwork.SaveChanges();
                return new ServiceResult<SubCategoryDto> { Data = mapper.Map<SubCategoryDto>(category), succeeded = true };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
