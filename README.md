
# Be The Hero App - .net version

 This is a sample application inspired from 'Be The Hero App' from Rocktseat's workshop. The original app was made with the javascript stack (nodejs, express, react and react-native).

In this sample I use:
- .Net Core 3.1
- NSwagg for documenting the api
- JWt token authentication
- Blazor WebAssembly (preview)
  

# Project Structure

Tha Solution is based in DDD/SOLID principals, but using CQRS as design pattern. This approach is based on Jason Taylor

1. Domain - *HeroApp.Domain*
2. Infraestructure - *HeroApp.Infra*
3. Application
   - Commands and Queries *HeroApp.AppShared*
   - The App itself *HeroApp.App*
4. Presentation
   - Blazor WebAssembly *HeroApp.Wasm*