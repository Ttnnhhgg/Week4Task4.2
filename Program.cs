using Microsoft.EntityFrameworkCore;
using Course4.Data;
var builder = WebApplication.CreateBuilder(args);
// ����������� �������� � ���������� DI 
builder.Services.AddDbContext<LibraryContext>(options =>
{    
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");    
    options.UseSqlServer(connectionString);    
    options.EnableDetailedErrors();    
    options.EnableSensitiveDataLogging();
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Library API", Version = "v1" });
});
var app = builder.Build();
//  ��������� ��������� HTTP �������� 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API v1");
        c.RoutePrefix = "swagger";
    });   
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<LibraryContext>();            
            if (context.Database.CanConnect())
            {
                Console.WriteLine("���� ������ ��� ����������.");
            }
            else
            {
                Console.WriteLine("���� ������ �� ����������. �������...");
            }          
            context.Database.Migrate();
            Console.WriteLine("�������� ������� ���������.");           
            var authorCount = context.Authors.Count();
            var bookCount = context.Books.Count();
            Console.WriteLine($"� ���� ������: {authorCount} �������, {bookCount} ����");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"������ ��� ��������� ���� ������: {ex.Message}");
        }
    }
}
else
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();
        context.Database.Migrate();
    }
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
Console.WriteLine("���������� �����������...");
app.Run();
