FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build /app/out ./
COPY --from=build /app/Views ./Views
COPY --from=build /app/wwwroot ./wwwroot
ENTRYPOINT ["dotnet", "Library.Web.dll"]