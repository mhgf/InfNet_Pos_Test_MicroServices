using Core.Entities;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Infra;

namespace Infra.Repostitories;

public class UserRepository(UserContext context) : SharedRepository<User>(context), IUserRepository
{
}