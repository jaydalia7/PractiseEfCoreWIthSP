using AutoMapper;
using FluentValidation.Results;
using PractiseEfCoreWIthSP.Models.Domains;
using PractiseEfCoreWIthSP.Models.ViewModels;
using PractiseEfCoreWIthSP.Repositories.IRepositories;
using PractiseEfCoreWIthSP.Services.IService;
using PractiseEfCoreWIthSP.Validations;

namespace PractiseEfCoreWIthSP.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<WebApiReponseModel> CreateProduct(AddProductModel addProductModel, CancellationToken cancellationToken)
        {
			WebApiReponseModel response = new WebApiReponseModel();
			try
			{
                //Validation
                ProductValidation validationRules = new ProductValidation();
                ValidationResult validationResult = validationRules.Validate(addProductModel);
                if (!validationResult.IsValid)
                {
                    string errorMessage = string.Empty;
                    foreach (ValidationFailure error in validationResult.Errors)
                    {
                        errorMessage += error.ErrorMessage;
                        break;
                        
                    }
                    response.Status = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.ErrorMessage = errorMessage;
                    return response;
                }
                var checkProductExists = await _productRepository.CheckProductByName(addProductModel.Name,cancellationToken);
                if (checkProductExists)
                {
                    response.ErrorMessage = "Product Already Exist";
                    response.Status = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }
                
                var productData = _mapper.Map<Product>(addProductModel);
                var newProductData = await _productRepository.AddProduct(productData, cancellationToken);
                if (newProductData.Id <=0)
                { 
                    response.Status=false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.ErrorMessage= "Error while Adding Product";
                    return response;
                }

                var newProduct = _mapper.Map<ProductDisplayModel>(newProductData);
                response.Status = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.Data = newProduct;
                return response;

            }
			catch (Exception ex)
			{
                response.Status = false;
                response.ErrorMessage = ex.Message.ToString();
                response.StatusCode = StatusCodes.Status400BadRequest;
                return response;
			}
        }

        public async Task<WebApiReponseModel> GetAllProduct(CancellationToken cancellationToken)
        {
            WebApiReponseModel response = new WebApiReponseModel();
            try
            {
                var products = await _productRepository.GetAllProducts(cancellationToken);
                
                if (products.Count <= 0)
                {
                    response.Status = false;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.ErrorMessage = "No Product Found";
                    
                }
                else
                {
                    var productData = _mapper.Map<List<ProductDisplayModel>>(products);
                    response.Status = true;
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Data = productData;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = ex.Message.ToString();
                response.StatusCode = StatusCodes.Status400BadRequest;
                return response;
            }
        }

        public async Task<WebApiReponseModel> RemoveProduct(int productId, CancellationToken cancellationToken)
        {
            WebApiReponseModel response = new WebApiReponseModel();
            try
            {
                //Getting Product Data
                var productData = await _productRepository.GetProductById(productId, cancellationToken);
                if (productData == null)
                {
                    response.Status = false;
                    response.ErrorMessage = $"Product Not found By the Id :{productId}";
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }
                //Removing Product Data
                var IsProductRemove = await _productRepository.RemoveProduct(productData, cancellationToken);
                if (!IsProductRemove)
                {
                    response.Status = false;
                    response.ErrorMessage = "Error while Removing Product";
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }
                else
                {
                    response.Status = true;
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Data = new String("Product Deleted Succesfully");
                    return response;
                }
            }
            catch (Exception ex)
            {

                response.Status = false;
                response.ErrorMessage = ex.Message.ToString();
                response.StatusCode = StatusCodes.Status400BadRequest;
                return response;
            }
        }

        public async Task<WebApiReponseModel> UpdateProduct(int productId, AddProductModel updateProductModel, CancellationToken cancellationToken)
        {
            WebApiReponseModel response = new WebApiReponseModel();
            try
            {
                //Validation
                ProductValidation validationRules = new ProductValidation();
                ValidationResult validationResult = validationRules.Validate(updateProductModel);
                if (!validationResult.IsValid)
                {
                    string errorMessage = string.Empty;
                    foreach (ValidationFailure error in validationResult.Errors)
                    {
                        errorMessage += error.ErrorMessage;
                        break;

                    }
                    response.Status = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.ErrorMessage = errorMessage;
                    return response;
                }

               //Getting Product Data
                var productData = await _productRepository.GetProductById(productId, cancellationToken);
                if (productData == null)
                {
                    response.ErrorMessage = $"Product Not found By the Id :{productId}";
                    response.Status = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }
                //Update Product Data
                var updateProduct = _mapper.Map(updateProductModel, productData);
                var updatedProduct = await _productRepository.UpdateProductData(updateProduct, cancellationToken);

                if (updatedProduct == null)
                {
                    response.Status = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.ErrorMessage = "Error While Updating the Product";
                    return response;
                }
                else
                {
                    var getUpdatedProductData = _mapper.Map<ProductDisplayModel>(updatedProduct);
                    response.Status = true;
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Data = getUpdatedProductData;
                    return response;
                }

            }
            catch (Exception ex)
            {

                response.Status = false;
                response.ErrorMessage = ex.Message.ToString();
                response.StatusCode = StatusCodes.Status400BadRequest;
                return response;
            }
        }
    }
}
