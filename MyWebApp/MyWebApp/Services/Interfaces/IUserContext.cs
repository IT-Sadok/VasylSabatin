using MyWebApp.DTO;

namespace MyWebApp.Services.Interfaces;
using System.Security.Claims;

public interface IUserContext
{
    public int UserId { get; }
}