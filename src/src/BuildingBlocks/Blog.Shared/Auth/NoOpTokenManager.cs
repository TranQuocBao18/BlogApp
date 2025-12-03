using System;

namespace Blog.Shared.Auth;

public class NoOpTokenManager : ITokenManager
{
    public Task<bool> IsCurrentActiveTokenAsync()
        {
            return Task.FromResult(true);
        }

        public Task DeactivateCurrentAsync()
        {
            return Task.CompletedTask;
        }

        public Task<bool> IsActiveAsync(string token)
        {
            return Task.FromResult(true);
        }

        public Task DeactivateAsync(string token)
        {
            return Task.CompletedTask;
        }

        public Task<bool> IsCurrentActiveUserAsync()
        {
            return Task.FromResult(true);
        }

        public Task ActivateUserAsync(string userId)
        {
            return Task.CompletedTask;
        }

        public Task DeactivateUserAsync(string userId)
        {
            return Task.CompletedTask;
        }

        public Task SetInvalidFromCurentAsync(string userId)
        {
            return Task.CompletedTask;
        }
}
