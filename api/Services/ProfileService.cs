using api.Data;
using api.Dtos;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;


namespace api.Services
{
    public class ProfileService : IProfileService
    {
        private readonly AppDbContext _context;
        private readonly INutritionCalculator _nutritionCalculator;

        public ProfileService(AppDbContext context, INutritionCalculator nutritionCalculator)
        {
            _context = context;
            _nutritionCalculator = nutritionCalculator;
        }

        public async Task<UserProfileDto?> GetAsync(string userId, CancellationToken cancellationToken = default)
        {
            var profile = await _context.UserProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(profile => profile.UserId == userId, cancellationToken);

            return profile?.ToUserProfileDto();
        }

        public async Task<UserProfileDto> SaveAsync(string userId, UpdateUserProfileDto dto, CancellationToken cancellationToken = default)
        {
            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(profile => profile.UserId == userId, cancellationToken);

            if (profile is null)
            {
                profile = new UserProfile { UserId = userId };

                profile.UpdateFromDto(dto);

                _context.UserProfiles.Add(profile);
            }
            else
            {
                profile.UpdateFromDto(dto);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return profile.ToUserProfileDto();
        }

        public async Task<NutritionTargetDto?> GetTargetsAsync(string userId, DateOnly calculationDate, CancellationToken cancellationToken = default)
        {
            var profile = await _context.UserProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(profile => profile.UserId == userId, cancellationToken);

            if (profile is null) return null;

            return _nutritionCalculator.Calculate(profile, calculationDate);
        }
    }
}