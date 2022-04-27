﻿using API.DTOs;
using Entities.App;

namespace API.Interfaces;

public interface IUserRepository
{
    void Update(User user);
    Task<User> GetUserByIdAsync(int id);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<List<UserDto>> GetUsersProfilesAsync();
    Task<List<User>> GetUsersAsync();
    Task<UserDto?> GetUserProfileAsync(string username,bool? owner=false);
    Task<bool> SaveChangesAsync();
}