using AutoMapper;
using LoginUpLevel.DTOs;
using LoginUpLevel.Models;
using LoginUpLevel.Repositories.Interface;
using LoginUpLevel.Services.Interface;
using Microsoft.AspNetCore.Identity;

namespace LoginUpLevel.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDTO> CreateAsync(ProductDTO productDto, IFormFile? image)
        {
            try
            {
                var newProduct = _mapper.Map<Product>(productDto);                
                if(newProduct == null)
                {
                    throw new Exception("Failed to create product");
                }

                if(image != null)
                {
                    var folderName = Path.Combine("wwwroot", "uploads", "images", "products");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var fileName = Guid.NewGuid().ToString() + "_" + newProduct.Name + ".png";
                    var fullPath = Path.Combine(pathToSave, fileName);

                    newProduct.Image = "/uploads/images/images/products" + fileName;
                }
                else
                {
                    newProduct.Image = "/uploads/images/images/products/default.png";
                }

                foreach(var productColorDto in productDto.ProductColors)
                {
                    var productColor = _mapper.Map<ProductColor>(productColorDto);
                    if(productColor == null)
                    {
                        throw new Exception("Invalid ProductColor");
                    }
                    productColor.ProductId = productDto.Id;
                    await _unitOfWork.ProductColorRepository.Add(productColor);
                    await _unitOfWork.SaveChangesAsync();
                }

                await _unitOfWork.ProductRepository.Add(newProduct);
                await _unitOfWork.SaveChangesAsync();
                return productDto;
                
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create product", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetById(id);
            if (product == null)
            {
                throw new Exception($"Product with ID {id} not found");
            }
            var productColorsDto = await _unitOfWork.ProductColorRepository.GetProductColorsByProductIdAsync(id);
            foreach(var item in productColorsDto)
            {
                await _unitOfWork.ProductColorRepository.Delete(item);
                await _unitOfWork.SaveChangesAsync();
            }

            await _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAll();
            if(products == null)
            {
                throw new Exception("No products found");
            }

            ICollection<ProductDTO> productsDTO = [];

            foreach(var item in products)
            {
                var productColors = await _unitOfWork.ProductColorRepository.GetProductColorsByProductIdAsync(item.Id);
                item.NameColors = [];
                foreach (var productColor in productColors)
                {
                    var color = await _unitOfWork.ColorRepository.GetById(productColor.ColorId);
                    item.NameColors.Add(color.NameColor);
                }
                var productDTO = new ProductDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    AMount = item.AMount,
                    Gender = item.Gender,
                    Weight = item.Weight,
                    Size = item.Size,
                    Category = item.Category,
                    NameColors = item.NameColors,
                    Image = item.Image,
                };

                productsDTO.Add(productDTO);
            }
            return productsDTO;
        }

        public async Task<ProductDTO> GetByIdAsync(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetById(id);
                if (product == null)
                {
                    throw new Exception($"Product with ID {id} not found");
                }

                var productColors = await _unitOfWork.ProductColorRepository.GetProductColorsByProductIdAsync(product.Id);
                product.NameColors = [];
                foreach (var productColor in productColors)
                {
                    var color = await _unitOfWork.ColorRepository.GetById(productColor.ColorId);
                    product.NameColors.Add(color.NameColor);
                }

                var productDTO = new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    AMount = product.AMount,
                    Gender = product.Gender,
                    Weight = product.Weight,
                    Size = product.Size,
                    Category = product.Category,
                    NameColors = product.NameColors,
                    Image = product.Image,
                };

                return productDTO;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetProductByCategoryAsync(string category)
        {
            try
            {
                return await _unitOfWork.ProductRepository.GetProductByCatagoryAsync(category)
                .ContinueWith(task => task.Result.Select(p => _mapper.Map<ProductDTO>(p)));
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get products by category {category}", ex);
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetproductByNameAsync(string name)
        {
            try
            {
                return await _unitOfWork.ProductRepository.SearchByNameAsync(name)
                    .ContinueWith(task => task.Result.Select(p => _mapper.Map<ProductDTO>(p)));
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateAsync(int id, ProductDTO updateDto, IFormFile? image)
        {
            try
            {
                var oldProduct = await _unitOfWork.ProductRepository.GetById(id);
                if (oldProduct == null)
                {
                    throw new Exception($"Product with ID {id} not found");
                }
                MapProductData(updateDto, oldProduct);
                //Xu ly anh
                if (image != null)
                {
                    var folderName = Path.Combine("wwwroot", "uploads", "images", "products");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var fileName = Guid.NewGuid().ToString() + "_" + oldProduct.Name + ".png";
                    var fullPath = Path.Combine(pathToSave, fileName);

                    oldProduct.Image = "/uploads/images/images/products" + fileName;
                }

                // Xu ly color
                var oldProductColors = await _unitOfWork.ProductColorRepository.GetProductColorsByProductIdAsync(id);

                var newColorIds = updateDto.ProductColors?.Select(c => c.ColorId).ToList() ?? new List<int>();

                var currentColorIds = oldProductColors.Select(pc => pc.ColorId).ToList();

                var colorIdsToRemove = currentColorIds.Except(newColorIds).ToList();

                var colorIdsToAdd = newColorIds.Except(currentColorIds).ToList();

                foreach (var item in colorIdsToRemove) // Xóa mau
                {
                    var productColor = await _unitOfWork.ProductColorRepository.GetProductColorByProductAndColorAsync(id, item);
                    await _unitOfWork.ProductColorRepository.Delete(productColor);
                    await _unitOfWork.SaveChangesAsync();
                }
                
                foreach (var item in colorIdsToAdd)//Them mau
                {
                    var productColor = new ProductColor
                    {
                        ProductId = id,
                        ColorId = item
                    };
                    await _unitOfWork.ProductColorRepository.Add(productColor);
                    await _unitOfWork.SaveChangesAsync();
                }

                await _unitOfWork.ProductRepository.Update(oldProduct);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update product with ID {id}", ex);
            }
        }
        private void MapProductData(ProductDTO dto, Product entity)
        {
            if (dto.Name != null && dto.Name != "") entity.Name = dto.Name;
            if (dto.Price != 0) entity.Price = dto.Price;
            if (dto.Description != null && dto.Description != "") entity.Description = dto.Description;
            if (dto.AMount != 0) entity.AMount = dto.AMount ?? 0;
            if (dto.Gender != null && dto.Gender != "") entity.Gender = dto.Gender;
            if (dto.Weight != null && dto.Weight != "") entity.Weight = dto.Weight;
            if (dto.Size != null && dto.Size != "") entity.Size = dto.Size;
            if (dto.Category != null && dto.Category != "") entity.Category = dto.Category;
        }
    }
}
