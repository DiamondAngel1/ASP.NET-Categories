//using AutoMapper;
//using WorkingMVC.Areas.Admin.Interfaces;
//using WorkingMVC.Areas.Admin.Models.Product;
//using WorkingMVC.Data.Entitys;
//using WorkingMVC.Interfaces;

//namespace WorkingMVC.Areas.Admin.Services
//{
//    public class ProductService(
//        IProductRepository productRepository,
//        IImageService imageService,
//        IMapper mapper) : IProductService
//    {
//        public async Task CreateAsync(ProductCreateModel model)
//        {
//            var imageUrls = new List<string>();

//            foreach (var file in model.Images)
//            {
//                var url = await imageService.UploadImageAsync(file);
//                if (!string.IsNullOrEmpty(url))
//                    imageUrls.Add(url);
//            }

//            if (imageUrls.Count == 0)
//                throw new Exception("Потрібно завантажити хоча б одне фото");

//            var entity = mapper.Map<ProductEntity>(model);
//            entity.Images = imageUrls;

//            await productRepository.AddAsync(entity);
//        }

//        public async Task<List<ProductItemModel>> GetAllAsync()
//        {
//            var products = await productRepository.GetAllAsync();
//            return products.Select(mapper.Map<ProductItemModel>).ToList();
//        }

//        public async Task<ProductEditModel> GetEditModelAsync(int id)
//        {
//            var product = await productRepository.GetByIdAsync(id)
//                ?? throw new Exception("Товар не знайдено");

//            return new ProductEditModel
//            {
//                Id = product.Id,
//                Name = product.Name,
//                Price = product.Price,
//                Description = product.Description,
//                CategoryId = product.CategoryId,
//                ExistingImages = product.Images
//            };
//        }

//        public async Task UpdateAsync(ProductEditModel model)
//        {
//            var product = await productRepository.GetByIdAsync(model.Id)
//                ?? throw new Exception("Товар не знайдено");

//            product.Name = model.Name;
//            product.Price = model.Price;
//            product.Description = model.Description ?? string.Empty;
//            product.CategoryId = model.CategoryId;

//            var updatedImages = model.ExistingImages
//        .Where(img => !model.ImagesToDelete.Contains(img))
//        .ToList();


//            foreach (var file in model.NewImages)
//            {
//                var url = await imageService.UploadImageAsync(file);
//                if (!string.IsNullOrEmpty(url))
//                    updatedImages.Add(url);
//            }

//            if (updatedImages.Count == 0)
//                throw new Exception("Потрібно залишити хоча б одне фото");

//            product.Images = updatedImages;
//            await productRepository.UpdateAsync(product);
//        }

//        public async Task DeleteAsync(int id)
//        {
//            var product = await productRepository.GetByIdAsync(id)
//                ?? throw new Exception("Товар не знайдено");

//            await productRepository.DeleteAsync(product);
//        }
//    }
//}