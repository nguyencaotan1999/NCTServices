using NCTServices.API.Common.Extentions;
using NCTServices.Application.Common.Extentions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.RegisterSwagger();
var _configuration = builder.Configuration;
builder.Services.AddCommonCors(_configuration);
builder.Services.AddDatabase(_configuration);
builder.Services.AddApplicationCommonLayer();
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.ConfigureSwagger();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCommonCors();
app.UseExceptionHandling(app.Environment);
app.Run();
