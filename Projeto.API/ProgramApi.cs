using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjetoApi.Dtos;
using System.Text;
using QuestPDF.Infrastructure;
using Projeto.Infrastructure.Infrascture.Repositories.Interfaces;
using Projeto.Application;
using Projeto.Infrastructure.Infrascture.Repositories.MySql;



var builder = WebApplication.CreateBuilder(args);
var key = builder.Configuration["Jwt:Key"];
// Swagger (vem do template)
QuestPDF.Settings.License = LicenseType.Community;
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey =true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))  
    };
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite: Bearer {seu token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("MinhaPolitica", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(
        new System.Text.Json.Serialization.JsonStringEnumConverter());
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SomenteAdmin",
        policy => policy.RequireRole("Admin"));

    options.AddPolicy("AdminOuUser",
        policy => policy.RequireRole("Admin", "User"));
});

// Seus serviços
builder.Services.AddScoped<MySqlConnectionFactory>();

builder.Services.AddScoped<IEmpresaClienteRepository, EmpresaClienteMySql>();
builder.Services.AddScoped<IPlanoRepository, PlanoMySql>();
builder.Services.AddScoped<IContratoRepository, ContratoMySql>();
builder.Services.AddScoped<IFaturaRepository, FaturaMySql>();
builder.Services.AddScoped<IPagamentosRepository, PagamentoMySql>();

builder.Services.AddScoped<EmpresaClienteService>();
builder.Services.AddScoped<PlanoService>();
builder.Services.AddScoped<ContratoService>();
builder.Services.AddScoped<FaturaService>();
builder.Services.AddScoped<PagamentoService>();
// 🔥 SOMENTE UMA VEZ
var app = builder.Build();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API V1");
    c.RoutePrefix = string.Empty; // abre direto na raiz
});
app.UseStaticFiles();
app.UseDefaultFiles();
app.UseAuthentication();
app.UseAuthorization();
// Swagger middleware 
if (app.Environment.IsDevelopment())  
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/login", (LoginDto dto, IConfiguration config) =>
{
    string role = null;
    if (dto.Usuario == "andre" && dto.Senha == "1234")
        role = "User";
    else if (dto.Usuario == "Lucas" && dto.Senha == "3214")
        role = "Admin";
    else
        return Results.Unauthorized();
    var claims = new[]
    {
        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role,role)
    };
    var key = builder.Configuration["Jwt:Key"];
    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

    var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
        expires: DateTime.Now.AddHours(1),
        claims: claims,
        signingCredentials: new SigningCredentials(
            signingKey,
            SecurityAlgorithms.HmacSha256)
    );
    var tokenString = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler()
       .WriteToken(token);

    return Results.Ok(new { token = tokenString });
}).WithTags("Login");

app.MapPost("/empresas/Adicionar", (EmpresaClienteService service, EmpresaCreatedDto dto) =>
{
    try
    {
        service.Adicionar(dto.RazaoSocial, dto.Cnpj, dto.Email);
        return Results.Ok("Empresa Criada com Sucesso");
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }

}).RequireAuthorization("SomenteAdmin").WithTags("Empresa");
app.MapPut("/empresas/{id}/Atualizar", (EmpresaClienteService service, int id, EmpresaUpdateDto dto) =>
{
    try
    {
        service.AtualizarEmpresa(id, dto.RazaoSocial, dto.Cnpj, dto.Email);
        return Results.Ok("Empresa Atualizada com Sucesso");
    }
    catch (Exception ex)
    {
        return Results.NotFound(ex.Message);
    }
}).RequireAuthorization("SomenteAdmin").WithTags("Empresa"); 
app.MapGet("/empresas/ListarTodos", (EmpresaClienteService service) =>
{
    var lista = service.ListarTodos();

    if (lista.Count == 0)
        return Results.NotFound("Nenhuma empresa cadastrada");

    return Results.Ok(lista);
}).RequireAuthorization("AdminOuUser").WithTags("Empresa");
app.MapGet("/empresa/ListarAtivos", (EmpresaClienteService service) =>
{
    var ativos = service.ListarAtivos();
    if (ativos.Count == 0)
        return Results.NotFound("Empresas Ativas não Encontradas");
    return Results.Ok(ativos);
}).RequireAuthorization("AdminOuUSer").WithTags("Empresa");
app.MapGet("/empresas/{id}/BuscarPorID", (EmpresaClienteService service, int id) =>
{
    var empresa = service.BuscarPorId(id);

    if (empresa is null)
        return Results.NotFound("Empresa não encontrada");

    return Results.Ok(empresa);
}).RequireAuthorization("AdminOuUser").WithTags("Empresa");
app.MapDelete("/empresas/{id}/Desativar", (EmpresaClienteService service, int id) =>
{
    try
    {
        service.Desativar(id);
        return Results.Ok("Empresa desativada com Sucesso");
    }
    catch(Exception ex)
    {
        return Results.NotFound(ex.Message);
    }
}).RequireAuthorization("SomenteAdmin").WithTags("Empresa");
app.MapPut("/empresas/{id}/Ativar", (EmpresaClienteService service, int id) =>
{
    try
    {
        service.Ativar(id);
        return Results.Ok("Empresa Ativada com Sucesso");
    }
    catch (Exception ex)
    {
        return Results.NotFound(ex.Message);
    }
}).RequireAuthorization("SomenteAdmin").WithTags("Empresa");

app.MapPost("/planos/Adicionar", (PlanoService service, PlanoCreatedDto dto) =>
{
    try
    {
        service.Adicionar(dto.Nome,dto.ValorMensal,dto.LimiteUsuario);
        return Results.Ok("Plano Adicionado com Sucesso");
    }
    catch(Exception ex)
    {
        return Results.NotFound(ex.Message);
    }
}).RequireAuthorization("SomenteAdmin").WithTags("Plano");
app.MapPut("/planos/{id}/Atualizar", (PlanoService service,int id, PlanoUpdateDto dto) =>
{
    try
    {
        service.AtualizarPlano(id, dto.Nome, dto.ValorMensal, dto.LimiteUsuario);
        return Results.Ok("Plano Atualizado com Sucesso");
    }
    catch (Exception ex)
    {
        return Results.NotFound(ex.Message);
    }
}).RequireAuthorization("SomenteAdmin").WithTags("Plano");
app.MapGet("/planos/ListarPlanos", (PlanoService service) =>
{
    var lista = service.ListarTudo();

    if (lista.Count == 0)
        return Results.NotFound("Nenhum Plano Encontrado");
    return Results.Ok(lista);
}).RequireAuthorization("AdminOuUSer").WithTags("Plano");
app.MapGet("/planos/ListarAtivos", (PlanoService service) =>
{
    var ativos = service.ListarAtivos();
    if (ativos.Count == 0)
        return Results.NotFound("Nenhum Plano Ativo Encntrado");
    return Results.Ok(ativos);
}).RequireAuthorization("AdminOuUSer").WithTags("Plano");
app.MapPut("/planos/{id}/Ativar", (PlanoService service, int id) =>
{
    try
    {
        service.Ativar(id);
        return Results.Ok("Plano Ativado com Sucesso");
    }
    catch(Exception ex)
    {
        return Results.NotFound(ex.Message);
    }
}).RequireAuthorization("SomenteAdmin").WithTags("Plano");
app.MapDelete("/planos/{id}/Desativar", (PlanoService service, int id) =>
{
    try
    {
        service.Desativar(id);
        return Results.Ok("Plano Desativado com Sucesso");
    }
    catch (Exception ex)
    {
        return Results.NotFound(ex.Message);
    }
}).RequireAuthorization("SomenteAdmin").WithTags("Plano");

app.MapPost("/contratos/CriarContrato", (ContratoService service, ContratoCreatedDto dto) => 
{ 
    try 
    { 
        service.CriarContrato(dto.EmpresaId, dto.PlanoId, dto.DiaVencimento); 
        return Results.Ok("Contrato Criado"); 
    } 
    catch (Exception ex) 
    { 
        return Results.NotFound(ex.Message); 
    }
}).RequireAuthorization("SomenteAdmin").WithTags("Contrato");
app.MapPut("/contratos/{id}/SuspenderContrato", (ContratoService service, int id) =>
{
    try
    {
        service.SuspenderContrato(id);
        return Results.Ok("Contrato Suspendido");
    }
    catch(Exception ex)
    {
        return Results.NotFound(ex.Message);
    }
}).RequireAuthorization("SomenteAdmin").WithTags("Contrato");
app.MapPut("/contratos/{id}/ReativarContrato", (ContratoService service, int id) =>
{
    try
    {
        service.ReativarContrato(id);
        return Results.Ok("Contrato Reativado com Sucesso");
    }
    catch(Exception ex)
    {
        return Results.NotFound(ex.Message);
    }

}).RequireAuthorization("SomenteAdmin").WithTags("Contrato");
app.MapDelete("/contratos/{id}/CancelarContrato", (ContratoService service, int id) =>
{
    try
    {
        service.CancelarContrato(id);
        return Results.Ok("Contrato Removido com Sucesso");
    }
    catch(Exception ex)
    {
        return Results.NotFound(ex.Message);
    }
}).RequireAuthorization("SomenteAdmin").WithTags("Contrato");
app.MapGet("/contratos/ListarContratos", (ContratoService service) =>
{
    var listar = service.ListarContrato();
    if (listar.Count == 0)
        return Results.NotFound("Não há contratos Cadastrados");
    return Results.Ok(listar);
}).RequireAuthorization("AdminOuUSer").WithTags("Contrato");

app.MapPost("/faturas/GerarFatura", (FaturaService service) =>
{
    try
    {
        service.GerarFaturasMensais();
        return Results.Ok("Fatura Criado com Sucesso");
    }
    catch(Exception ex)
    {
        return Results.NotFound(ex.Message);
    }
}).RequireAuthorization("AdminOuUSer").WithTags("Fatura");
app.MapPut("/faturas/{id}/VencimentoFatura", (FaturaService service,int id) =>
{
    try
    {
        service.MarcarFaturaComoVencida(id);
        return Results.Ok("A Fatura foi marcada como Vencida");
    }
    catch(Exception ex)
    {
        return Results.NotFound(ex.Message);
    }
}).RequireAuthorization("SomenteAdmin").WithTags("Fatura");
app.MapGet("/faturas/{id}/ListarFaturaContrato", (FaturaService service, int id) =>
{
    var listar = service.ListarFaturasPorContrato(id);
    if(listar.Count == 0)
        return Results.NotFound("Contrato não Encontrada");
    return Results.Ok(listar);
}).RequireAuthorization("AdminOuUSer").WithTags("Fatura");
app.MapGet("/faturas/ListarFaturaEmAberto", (FaturaService service) =>
{
    var lista = service.ListarFaturasEmAberto();
    if (lista.Count == 0)
        return Results.NotFound("Não tem faturas em Aberto");
    return Results.Ok(lista);
}).RequireAuthorization("AdminOuUSer").WithTags("Fatura");

app.MapPost("/pagamento/{id}/RegistrarPagamento", (PagamentoService service, int id,PagamentoCreatedDto dto) =>
{
    try
    {
        service.RegistrarPagamento(id, dto.Valor, dto.Forma);
        return Results.Ok("Pagamento Registrado com Sucesso");
    }
    catch(Exception ex)
    {
        return Results.NotFound(ex.Message);
    }
}).RequireAuthorization("AdminOuUSer").WithTags("Pagamento");
app.MapGet("/pagamento/ListarPagamento", (PagamentoService service) =>
{
    var listar = service.Listar();
    if (listar.Count == 0)
        return Results.NotFound("Não Há Pagamentos Registrados");
    return Results.Ok(listar);
}).RequireAuthorization("AdminOuUSer").WithTags("Pagamento");
app.MapGet("/pagamento/{id}/BaixarFatura", (PagamentoService service, int id) =>
{
    var pdfBytes = service.GerarBoletoPdf(id);
    return Results.File(pdfBytes, "application/pdf", $"fatura_{id}.pdf");

}).RequireAuthorization("AdminOuUSer").WithTags("Pagamento");

app.Run();