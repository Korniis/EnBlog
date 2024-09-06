#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EBlog.WebApi/EBlog.WebApi.csproj", "EBlog.WebApi/"]
COPY ["BaseService/EBlog.BaseService.csproj", "BaseService/"]
COPY ["EBlog.Repository/EBlog.IBaseRepository.csproj", "EBlog.Repository/"]
COPY ["EBlog.Domain/EBlog.Domain.csproj", "EBlog.Domain/"]
COPY ["IBaseService/EBlog.IBaseService.csproj", "IBaseService/"]
COPY ["EBlog.BaseRespository/EBlog.BaseRepository.csproj", "EBlog.BaseRespository/"]
COPY ["EBlog.Utility/EBlog.Utility.csproj", "EBlog.Utility/"]
RUN dotnet restore "./EBlog.WebApi/EBlog.WebApi.csproj"
COPY . .
WORKDIR "/src/EBlog.WebApi"
RUN dotnet build "./EBlog.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EBlog.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EBlog.WebApi.dll"]