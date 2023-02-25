﻿using CooleWebapp.Core.Entities;

namespace CooleWebapp.Application.Users;

public interface IUserDataAccess
{
  Task<CoolUser> CreateUser(CoolUser user, CancellationToken ct);
  Task<CoolUser?> FindUserByWebappUserId(string webappUserId, CancellationToken ct);
}