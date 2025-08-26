# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY chat.sln .
COPY chat.api/Chat.Api.csproj ./chat.api/
COPY chat.data/Chat.Data.csproj ./chat.data/
COPY chat.message/Chat.Message.csproj ./chat.message/
COPY chat.shared/Chat.Shared.csproj ./chat.shared/
# Repeat for all projects

RUN dotnet restore chat.sln

COPY . .
WORKDIR /app/chat.api
RUN dotnet publish Chat.Api.csproj -c Release -o /out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

COPY chat.api/configs/kafka.config.json ./configs/

ENTRYPOINT ["dotnet", "Chat.Api.dll"]