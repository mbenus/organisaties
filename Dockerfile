FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Organisations/*.csproj ./Organisations/
#COPY utils/*.csproj ./utils/
COPY Organisations.Tests/*.csproj ./Organisations.Tests/
RUN dotnet restore

# copy everything else and build app
COPY . .
WORKDIR /app/Organisations
RUN dotnet build


FROM build AS testrunner
WORKDIR /app/Organisations.Tests
ENTRYPOINT ["dotnet", "test", "--logger:trx"]


FROM build AS test
WORKDIR /app/Organisations.Tests
RUN dotnet test

# publiceer (self contained)
FROM build AS publish
WORKDIR /app/Organisations
RUN dotnet publish -c Release -o out -r linux-x64

# Draai Organisations
FROM microsoft/dotnet:2.0-runtime AS runtime
WORKDIR /app
COPY --from=publish /app/Organisations/out ./
ENTRYPOINT ["dotnet", "Organisations.dll"]