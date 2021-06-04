using AutoMapper;
using CatalogoAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoAPI.DTOs.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile (){

            CreateMap<Produto, ProdutoDTO>().ReverseMap();
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
        }
    }
}
