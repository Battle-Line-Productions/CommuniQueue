#region Copyright
// ---------------------------------------------------------------------------
// Copyright (c) 2024 Battleline Productions LLC. All rights reserved.
// 
// Licensed under the Battleline Productions LLC license agreement.
// See LICENSE file in the project root for full license information.
// 
// Author: Michael Cavanaugh
// Company: Battleline Productions LLC
// Date: 11/23/2024
// Solution Name: CommuniQueue
// Project Name: CommuniQueue.DataAccess
// File: UserRepository.cs
// File Path: C:\git\battleline\CommuniQueue\CommuniQueue.DataAccess\Services\UserRepository.cs
// ---------------------------------------------------------------------------
#endregion

using CommuniQueue.Contracts.Interfaces.Repositories;
using CommuniQueue.Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace CommuniQueue.DataAccess.Services;

public class UserRepository(AppDbContext context) : BaseRepository<User>(context), IUserRepository
{
    private readonly AppDbContext _context = context;

    public async Task<User?> GetBySsoIdAsync(string ssoId)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.SsoId == ssoId);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> ExistsBySsoIdAsync(string ssoId)
    {
        return await _context.Users
            .AnyAsync(u => u.SsoId == ssoId);
    }
}
