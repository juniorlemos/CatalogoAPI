# CatalogoAPI

API RESTFUL de catálogos de produtos  utilizando todos os conceitos do Framework Net.Core  aprendidos nos  meus estudos



Tecnologias  Utilizadas

- Framework dotnet core 5 
- Banco de dados Mysql 
- Entity framework
- Identity
- Token JWT
- Swagger
- AutoMapper

 Requisições da API
 
  Autorização 
 
 POST /api/Autoriza/Register   -Registra um novo usuário
 POST /api/Autoriza/Login      -Verifica as credenciais do usuário

obs: Para ter acesso as outras requisições é preciso se registrar e fazer o login com as requisições acima

 Categorias
 
 GET     /api/Categorias               -Obtém todas as Categorias.
 POST    /api/Categorias               -Insere uma nova Categoria
 GET     /api/Categorias/page/{page}   -Obtém as Categorias através de Paginação
 GET     /api/Categorias/{id}          -Obtém uma Categoria através do ID
 PUT     /api/Categorias/{id}          -Modifica uma Categoria através do ID
 DELETE  /api/Categorias/{id}          -Deleta uma Categoria
 
  Produtos
 
 GET     /api/Produtos               -Obtém todos os Produtos.
 POST    /api/Produtos               -Insere um novo Produto
 GET     /api/Produtos/page/{page}   -Obtém os Produtos através de Paginação
 GET     /api/Produtos/{id}          -Obtém um Produto através do ID
 PUT     /api/Produtos/{id}          -Modifica um Produto através do ID
 DELETE  /api/Produtos/{id}          -Deleta um Produto


 
