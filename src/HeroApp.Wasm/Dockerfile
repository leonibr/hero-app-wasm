FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS base


WORKDIR /source

COPY HeroApp.Api/*.csproj HeroApp.Api/.
COPY HeroApp.App/*.csproj HeroApp.App/.
COPY HeroApp.AppShared/*.csproj HeroApp.AppShared/.
COPY HeroApp.Domain/*.csproj HeroApp.Domain/.
COPY HeroApp.Infra/*.csproj HeroApp.Infra/.
COPY HeroApp.Wasm/*.csproj HeroApp.Wasm/.
COPY HeroAppSolution.sln .

RUN dotnet restore

COPY . .

FROM base as front
WORKDIR /front-source
COPY --from=base /source /front-source
WORKDIR /front-source/HeroApp.Wasm
RUN dotnet publish -c Release -o /build
WORKDIR /build


FROM nginx:1.17.1-alpine
COPY --from=front /build/wwwroot /usr/share/nginx/html

ENV VIRTUAL_HOST heroappwasm.marques.top
ENV LETSENCRYPT_HOST heroappwasm.marques.top

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]


