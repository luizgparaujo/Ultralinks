using AutoMapper;
using Ultralinks.Application.ViewModels;
using Ultralinks.Domain.DTOs;
using Ultralinks.Domain.Models;

namespace Ultralinks.Application.Mappings
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Usuario, UsuarioDTO>();
            CreateMap<Endereco, EnderecoDTO>();
            CreateMap<Transacao, TransacaoDTO>();
                      
            CreateMap<CreateEnderecoViewModel, Endereco>();
            CreateMap<CreateUsuarioViewModel, Usuario>();
            CreateMap<CreateTransacaoViewModel, Transacao>();

            CreateMap<UpdateEnderecoViewModel, Endereco>();
            CreateMap<UpdateUsuarioViewModel, Usuario>();
        }
    }
}
