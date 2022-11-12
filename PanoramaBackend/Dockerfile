#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["PanoramaBackend/PanoramaBackend.Api.csproj", "PanoramaBackend/"]
COPY ["NukesLab.Core.Repository/NukesLab.Core.Repository.csproj", "NukesLab.Core.Repository/"]
COPY ["NukesLab.Core.Common/NukesLab.Core.Common.csproj", "NukesLab.Core.Common/"]
COPY ["NukesLab.Core.Api/NukesLab.Core.Api.csproj", "NukesLab.Core.Api/"]
COPY ["ArcherMicroFinanceBackend.Services/PanoramaBackend.Services.csproj", "ArcherMicroFinanceBackend.Services/"]
COPY ["ArcherMicroFinanceBackend.Data/PanoramaBackend.Data.csproj", "ArcherMicroFinanceBackend.Data/"]
COPY ["NukesLab.Core.Services/NukesLab.Core.Services.csproj", "NukesLab.Core.Services/"]
RUN dotnet restore "PanoramaBackend/PanoramaBackend.Api.csproj"
COPY . .
WORKDIR "/src/PanoramaBackend"
RUN dotnet build "PanoramaBackend.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PanoramaBackend.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PanoramaBackend.Api.dll"]