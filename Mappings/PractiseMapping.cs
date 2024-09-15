using AutoMapper;
using PractiseEfCoreWIthSP.Models.Domains;
using PractiseEfCoreWIthSP.Models.ViewModels;

namespace PractiseEfCoreWIthSP.Mappings
{
    public class PractiseMapping :Profile
    {
        public PractiseMapping() {
            CreateMap<AddProductModel, Product>();
            CreateMap<Product, ProductDisplayModel>();
            CreateMap<UserAddModel, User>();
            CreateMap<User, UserDisplayModel>();
        }
    }
}
