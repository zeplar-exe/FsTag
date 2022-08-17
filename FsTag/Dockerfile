FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FsTag/FsTag.csproj", "FsTag/"]
RUN dotnet restore "FsTag/FsTag.csproj"
COPY . .
WORKDIR "/src/FsTag"
RUN dotnet build "FsTag.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FsTag.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FsTag.dll"]
