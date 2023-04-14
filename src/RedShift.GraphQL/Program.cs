using RedShift.GraphQL.Extensions;
using RedShift.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services
    .AddGraphQL(builder.Configuration)
    .AddAuthentication(builder.Configuration)
    .AddAuthorization()
    .AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseWebSockets();
app.UseGraphQL();

app.Run();