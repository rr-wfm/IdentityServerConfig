using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddIdentityServer()
                .AddConfigurationStore(options =>
                {
                    options.DefaultSchema = "Configuration";
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer("Data Source=.;Initial Catalog=Identity;Integrated Security=True;Encrypt=false");
                });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
