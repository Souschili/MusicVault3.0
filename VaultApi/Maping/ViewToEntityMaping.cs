
using AutoMapper;
using MusicVault.Data.Entity;
using VaultApi.ViewModels;

namespace VaultApi.Maping
{
    public class ViewToEntityMaping : Profile
    {
        public ViewToEntityMaping()
        {
            //обратный реверс потом , нам пока он ненужен 
            CreateMap<UserRegistrationModel, User>();
        }
    }
}
