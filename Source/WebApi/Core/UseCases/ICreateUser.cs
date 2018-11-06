﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrakeLambert.Peerra.WebApi.Core.UseCases
{
    public interface ICreateUser
    {
        Task<(bool Succeeded, IEnumerable<string> Errors)> HandleAsync(string username, string password);
    }
}