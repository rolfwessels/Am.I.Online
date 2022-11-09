[![Github release](https://img.shields.io/github/v/release/rolfwessels/Am.I.Online)](https://github.com/rolfwessels/Am.I.Online/releases)
[![Dockerhub Status](https://img.shields.io/badge/dockerhub-ok-blue.svg)](https://hub.docker.com/r/rolfwessels/Am.I.Online/tags)
[![Dockerhub Version](https://img.shields.io/docker/v/rolfwessels/Am.I.Online?sort=semver)](https://hub.docker.com/r/rolfwessels/Am.I.Online/tags)
[![GitHub](https://img.shields.io/github/license/rolfwessels/Am.I.Online)](https://github.com/rolfwessels/Am.I.Online/licence.md)

<img src="./docs/logo.png" style=" margin-left: auto;margin-right: auto;display: block;"
     alt="Am i online">


# Am i online


This makes am i online happen

## Getting started

Open the docker environment to do all development and deployment

```bash
# bring up dev environment
make build up
# test the project
make test
# build the project ready for publish
make publish
```

## Available make commands

### Commands outside the container

- `make up` : brings up the container & attach to the default container
- `make down` : stops the container
- `make build` : builds the container

### Commands to run inside the container

- `make test` : Test the app
- `make start` : Run the console app
- `make publish` : Publish the packages to the dist folder

## Research

- <https://opensource.com/article/18/8/what-how-makefile> What is a Makefile and how does it work?
