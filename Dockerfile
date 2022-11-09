FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine

# Base Development Packages
RUN apk update \
  && apk upgrade \
  && apk add ca-certificates wget && update-ca-certificates \
  && apk add --no-cache --update \
  git \
  curl \
  wget \
  bash \
  make \
  rsync \
  nano \
  && git config --global --add safe.directory /am-i-online


# Speed up restore
WORKDIR /am-i-online
COPY src/Am.I.Online.Api/*.csproj ./src/Am.I.Online.Api/
COPY src/Am.I.Online.Core/*.csproj ./src/Am.I.Online.Core/
WORKDIR /am-i-online/src/Am.I.Online.Api/
RUN dotnet restore

# Set environmentdot
WORKDIR /am-i-online
ENV PATH="/root/.dotnet/tools:${PATH}"
ENV TERM xterm-256color
RUN printf 'export PS1="\[\e[0;34;0;33m\][DCKR]\[\e[0m\] \\t \[\e[40;38;5;28m\][\w]\[\e[0m\] \$ "' >> ~/.bashrc
