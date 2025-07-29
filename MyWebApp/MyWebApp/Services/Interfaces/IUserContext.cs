using MyWebApp.DTO;

namespace MyWebApp.Services.Interfaces;
using System.Security.Claims;

public interface IUserContext
{
    RequesterContextModel GetRequesterContext();
}