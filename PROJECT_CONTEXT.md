# CalorieAI - Project Context

## Project overview

CalorieAI is a learning full-stack calorie and nutrition tracking application.

- Backend: ASP.NET Core Web API, .NET 10
- Database: PostgreSQL
- ORM: Entity Framework Core with Npgsql
- Authentication: ASP.NET Core Identity bearer access and refresh tokens
- Frontend: React 19, Vite, Tailwind CSS, shadcn/Base UI
- API URL during development: `http://localhost:5077`
- Frontend URL during development: `http://localhost:5173`

The repository contains two applications:

```text
api/       ASP.NET Core backend
frontend/  React frontend
```

## Current status

Both applications build successfully. The project has been committed and pushed to GitHub. Generated files and secrets are excluded through the root `.gitignore`.

Implemented user workflow:

1. A user can register and sign in.
2. Identity returns an access token and refresh token.
3. The frontend stores both tokens in `localStorage`.
4. Protected requests include a bearer access token.
5. An expired access token is refreshed automatically.
6. If the refresh token is invalid, the frontend logs out and shows the sign-in form.
7. Each food diary entry belongs to one user.
8. Users can only read, update, and delete their own entries.
9. Users can search foods and add them to a selected date and meal.
10. Quantity, calculated nutrients, daily totals, editing, and deletion work in the UI.

## Backend structure

Important files:

```text
api/Program.cs
api/Data/AppDbContext.cs
api/Models/AppUser.cs
api/Models/Food.cs
api/Models/FoodEntry.cs
api/Controllers/FoodController.cs
api/Controllers/FoodEntriesController.cs
api/Repository/FoodEntryRepository.cs
api/Interfaces/IFoodEntryRepository.cs
api/Mappers/FoodMapper.cs
api/Mappers/FoodEntryMapper.cs
api/Dtos/
api/Migrations/
```

### Models

`Food` is the product catalog. It stores nutrition values per 100 grams.

`FoodEntry` is a diary record and contains:

- `FoodId` and the related `Food`
- `QuantityGrams`
- `EatenAt`
- `MealType`
- nullable `UserId` and related `AppUser`

`UserId` is currently nullable because entries created before authentication may still have no owner. All new entries created through the protected API receive the current user's ID.

`AppUser` extends `IdentityUser` and has a collection of food entries.

### Authentication

Identity is configured in `Program.cs` with:

```csharp
builder.Services
    .AddIdentityApiEndpoints<AppUser>()
    .AddEntityFrameworkStores<AppDbContext>();
```

Middleware and endpoints are enabled:

```csharp
app.UseAuthentication();
app.UseAuthorization();
app.MapIdentityApi<AppUser>();
```

Identity endpoints used by the frontend:

```http
POST /register
POST /login
POST /refresh
```

Example registration body:

```json
{
  "email": "user@example.com",
  "password": "Test123!"
}
```

Passwords must satisfy the default Identity requirements. A successful registration returns `200 OK` with an empty response body.

Example login body:

```json
{
  "email": "user@example.com",
  "password": "Test123!"
}
```

Login returns `accessToken`, `refreshToken`, `tokenType`, and `expiresIn`.

### Food endpoints

The product catalog remains public. It supports retrieving/creating foods and searching by product name. Duplicate food names are rejected.

The frontend search request is:

```http
GET /api/Food/search?query=apple
```

### FoodEntry endpoints

`FoodEntriesController` has `[Authorize]`. The repository filters operations using the current Identity user ID.

Main endpoints:

```http
GET    /api/FoodEntries
GET    /api/FoodEntries/{id}
POST   /api/FoodEntries/{foodId}
PUT    /api/FoodEntries/{id}
DELETE /api/FoodEntries/{id}
GET    /api/FoodEntries/daily?date=YYYY-MM-DD
GET    /api/FoodEntries/by-date?date=YYYY-MM-DD
```

POST body:

```json
{
  "quantityGrams": 180,
  "eatenAt": "2026-08-28T12:00:00Z",
  "mealType": "Lunch"
}
```

PUT currently changes only quantity:

```json
{
  "quantityGrams": 250
}
```

Valid meal values currently used by the frontend:

```text
Breakfast
Lunch
Dinner
Snacks
```

Daily and grouped endpoints calculate calories, protein, fat, carbohydrates, and sugar from the food values and entry quantity.

## Database migrations

Existing migrations:

```text
InitialCreate
FoodEntry
AddIdentity
AddUserToFoodEntry
```

Identity tables and the nullable `FoodEntries.UserId` column have been created in the development database.

The database itself and ASP.NET User Secrets are not stored in Git.

## Frontend structure

Important files:

```text
frontend/src/App.jsx
frontend/src/api/auth.js
frontend/src/api/foods.js
frontend/src/api/foodEntries.js
frontend/src/components/LoginForm.jsx
frontend/src/components/RegisterForm.jsx
frontend/src/components/DashBoard.jsx
frontend/src/components/Meals.jsx
frontend/src/components/AddFoodDialog.jsx
frontend/src/components/EditFoodEntryDialog.jsx
frontend/src/components/DeleteFoodEntryDialog.jsx
frontend/src/components/Macronutrients.jsx
frontend/src/components/ui/
```

### Authentication UI

`App.jsx` switches between authentication forms and the dashboard.

- `LoginForm` calls `login()` and opens the dashboard.
- `RegisterForm` calls `register()` and returns to sign-in.
- The dashboard has a sign-out button.
- `auth.js` stores/removes tokens and implements `authorizedFetch()`.
- `authorizedFetch()` retries a request once after refreshing an expired access token.
- `auth:logout` is dispatched when the full session expires.

Tokens are stored in `localStorage`. This is acceptable for the current learning MVP but should be reviewed before a production release because XSS can expose local storage.

### Dashboard and diary UI

- The default selected date is the user's current local date.
- Daily totals are loaded from `/daily`.
- Meal groups are loaded from `/by-date`.
- The product dialog has debounced product search.
- Adding an entry refreshes daily totals and meal groups.
- Entries can be edited through a dialog.
- Entries can be deleted through a confirmation dialog.
- Loading, request error, and empty-day states exist.
- Visible frontend text is currently English.

## Running on another computer

Requirements:

- Git
- .NET 10 SDK
- Node.js/npm
- PostgreSQL, or access to an existing PostgreSQL server

Clone and install frontend dependencies:

```powershell
git clone REPOSITORY_URL
cd CaloriesCalc\frontend
npm install
```

Restore backend packages:

```powershell
cd ..\api
dotnet restore
```

Configure the connection string locally. Do not commit it:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=DATABASE;Username=USER;Password=PASSWORD"
```

Create/update the database:

```powershell
dotnet ef database update
```

Start the backend:

```powershell
dotnet watch
```

Start the frontend in a separate terminal:

```powershell
cd frontend
npm run dev
```

## Verification commands

Backend:

```powershell
dotnet build api/api.csproj
```

Frontend:

```powershell
cd frontend
npm.cmd run build
npm.cmd run lint
```

The complete frontend lint command may include issues in generated UI components. Focused linting has been used for application files when necessary.

## Next planned feature

The next feature is a user nutrition profile with automatically calculated calorie and macronutrient targets.

Planned `UserProfile` data:

- weight in kilograms
- height in centimeters
- date of birth
- biological sex
- activity level
- goal: lose, maintain, or gain weight
- optional body-fat percentage

The profile should be a separate one-to-one model related to `AppUser`, rather than placing all profile fields directly on the Identity model.

Planned calculations:

- If body-fat percentage is provided: Katch-McArdle BMR based on lean body mass.
- Otherwise: Mifflin-St Jeor BMR based on weight, height, age, and sex.
- TDEE: BMR multiplied by an activity coefficient.
- Calorie adjustment based on lose/maintain/gain goal.
- Protein and fat based on body weight or lean mass.
- Carbohydrates calculated from remaining calories.

The next concrete implementation steps are:

1. Create `UserProfile` and enums/value constraints.
2. Add the one-to-one relationship and EF migration.
3. Create profile request/response DTOs.
4. Implement protected `GET /api/profile` and `PUT /api/profile` endpoints.
5. Create a testable `NutritionCalculator` service.
6. Replace hard-coded dashboard goals with calculated profile targets.
7. Add the profile form to the frontend.

## Known follow-up work

- Parse Identity validation responses so registration displays exact password or duplicate-email errors.
- Consider moving production authentication from local storage to secure HttpOnly cookies.
- Decide what to do with old `FoodEntry` rows where `UserId` is null, then make the relationship required.
- Add automated backend and frontend tests.
- Move development API URLs into frontend environment configuration.
- Remove unused starter assets if they are no longer needed.
- Replace any remaining hard-coded nutrition targets after the profile calculator is implemented.
