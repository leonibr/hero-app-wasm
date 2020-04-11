FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS base


WORKDIR /source

COPY src/HeroApp.Api/*.csproj HeroApp.Api/.
COPY src/HeroApp.App/*.csproj HeroApp.App/.
COPY src/HeroApp.AppShared/*.csproj HeroApp.AppShared/.
COPY src/HeroApp.Domain/*.csproj HeroApp.Domain/.
COPY src/HeroApp.Infra/*.csproj HeroApp.Infra/.
COPY src/HeroApp.Wasm/*.csproj HeroApp.Wasm/.
COPY src/HeroAppSolution.sln .

RUN dotnet restore

COPY . .

FROM base as back
WORKDIR /back-source
COPY --from=base /source /back-source
WORKDIR /back-source/HeroApp.Api

RUN dotnet publish -c Release -o /build

WORKDIR /build

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as runtime
WORKDIR /app
COPY --from=back /build .

ENV VIRTUAL_HOST heroappwasm-back.marques.top
ENV LETSENCRYPT_HOST heroappwasm-back.marques.top
 
EXPOSE 5000

CMD ["dotnet", "HeroApp.Api.dll"]