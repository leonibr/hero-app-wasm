FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS base


WORKDIR /source

COPY src/HeroApp.Api/HeroApp.Api.csproj src/HeroApp.Api/HeroApp.Api.csproj
COPY src/HeroApp.App/HeroApp.App.csproj src/HeroApp.App/HeroApp.App.csproj
COPY src/HeroApp.AppShared/HeroApp.AppShared.csproj src/HeroApp.AppShared/HeroApp.AppShared.csproj
COPY src/HeroApp.Domain/HeroApp.Domain.csproj src/HeroApp.Domain/HeroApp.Domain.csproj
COPY src/HeroApp.Infra/HeroApp.Infra.csproj src/HeroApp.Infra/HeroApp.Infra.csproj
COPY src/HeroApp.Wasm/HeroApp.Wasm.csproj src/HeroApp.Wasm/HeroApp.Wasm.csproj
COPY HeroAppSolution.sln .

RUN dotnet restore

COPY . .

FROM base as back
WORKDIR /back-source
COPY --from=base /source /back-source
WORKDIR /back-source/src/HeroApp.Api

RUN dotnet publish --no-restore --no-build -c Release -o /build

WORKDIR /build

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as runtime
WORKDIR /app
COPY --from=back /build .

ENV VIRTUAL_HOST heroappwasm-back.marques.top
ENV LETSENCRYPT_HOST heroappwasm-back.marques.top
 
EXPOSE 5000

CMD ["dotnet", "HeroApp.Api.dll"]