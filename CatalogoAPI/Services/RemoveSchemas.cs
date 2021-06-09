using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoAPI.Services
{
    public class RemoveSchemas : ISchemaFilter
    {
        private readonly string[] VisibleSchemas = { "ProdutoDTO", "CategoriaDTO",
            "UsuarioDTO" };

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            foreach (var key in context.SchemaRepository.Schemas.Keys) 
            {

                if (!VisibleSchemas.Contains(key))
                    context.SchemaRepository.Schemas.Remove(key);
            }

                    }
    }
}
