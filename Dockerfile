# backend/Dockerfile

# ---- Стадия сборки ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Копируем .sln и .csproj для кеширования restore
COPY ProjectCenter.sln .
COPY ProjectCenter.API/ProjectCenter.API.csproj ProjectCenter.API/
COPY ProjectCenter.Application/ProjectCenter.Application.csproj ProjectCenter.Application/
COPY ProjectCenter.Core/ProjectCenter.Core.csproj ProjectCenter.Core/
COPY ProjectCenter.Infrastructure/ProjectCenter.Infrastructure.csproj ProjectCenter.Infrastructure/
COPY ProjectCenter.Test/ProjectCenter.Test.csproj ProjectCenter.Test/

# Восстанавливаем зависимости
RUN dotnet restore

# Копируем исходники
COPY . .

# Публикуем API
RUN dotnet publish ProjectCenter.API/ProjectCenter.API.csproj -c Release -o /app/publish


# ---- Стадия продакшена ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Создаём директории для статики (файлы проектов, изображения, документация)
RUN mkdir -p /app/wwwroot/Documentation /app/wwwroot/Projects /app/wwwroot/Images

WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

CMD ["dotnet", "ProjectCenter.API.dll"]
