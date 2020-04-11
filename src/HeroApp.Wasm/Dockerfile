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

FROM base as front
WORKDIR /front-source
COPY --from=base /source /front-source
WORKDIR /front-source/src/HeroApp.Wasm
RUN dotnet publish --no-restore -c Release -o /build
WORKDIR /build


FROM nginx:1.17.1-alpine
COPY --from=front /build/wwwroot /usr/share/nginx/html

ENV VIRTUAL_HOST heroappwasm.marques.top
ENV LETSENCRYPT_HOST heroappwasm.marques.top

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]


