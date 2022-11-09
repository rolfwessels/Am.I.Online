.DEFAULT_GOAL := help

# General Variables
date=$(shell date +'%y.%m.%d.%H.%M')
project := Am i online
container := dev
docker-filecheck := /.dockerenv
docker-warning := ""
RED=\033[0;31m
GREEN=\033[0;32m
NC=\033[0m # No Color
versionPrefix := 0.1
version := $(versionPrefix).$(shell git rev-list HEAD --count)
shorthash := $(shell git rev-parse --short=8 HEAD)
version-suffix := ''
dockerhub := dockerhub.com/am-i-online

release := 'release'
ifeq ($(env), dev)
	release := 'debug'
	version-suffix:= ""
endif

ifdef GITHUB_BASE_REF
	current-branch :=  $(patsubst refs/heads/%,%,${GITHUB_HEAD_REF})
else ifdef GITHUB_REF
	current-branch :=  $(patsubst refs/heads/%,%,${GITHUB_REF})
else
	current-branch :=  $(shell git rev-parse --abbrev-ref HEAD)
endif

ifeq ($(current-branch), main)
	docker-tags := -t $(dockerhub):alpha -t $(dockerhub):latest -t $(dockerhub):v$(version) -t $(dockerhub):$(shorthash)
else
	version := $(versionPrefix).$(shell git rev-list main --count).$(shell git rev-list main..HEAD --count)
	version-suffix := alpha
	docker-tags := -t $(dockerhub):$(version-suffix) -t $(dockerhub):$(shorthash) -t $(dockerhub):v$(version)-$(version-suffix)
endif

# Docker Warning
ifeq ("$(wildcard $(docker-filecheck))","")
	docker-warning = "⚠️  WARNING: Can't find /.dockerenv - it's strongly recommended that you run this from within the docker container."
endif

# Targets
help:
	@echo "The following commands can be used for building & running & deploying the the $(project) container"
	@echo "---------------------------------------------------------------------------------------------"
	@echo "Targets:"
	@echo "  Docker Targets (run from local machine)"
	@echo "   - up          : brings up the container & attach to the default container ($(container))"
	@echo "   - down        : stops the container"
	@echo "   - build       : (re)builds the container"
	@echo ""
	@echo "  Service Targets (should only be run inside the docker container)"
	@echo "   - version      : Set current version number $(project)"
	@echo "   - start        : Run the $(project)"
	@echo "   - test         : Test the $(project)"
	@echo "   - publish      : Publish the $(project)"
	@echo "   - deploy       : Deploy the $(project)"
	@echo ""
	@echo "Options:"
	@echo " - env    : sets the environment - supported environments are: dev | prod"
	@echo ""
	@echo "Examples:"
	@echo " - Start Docker Container              : make up"
	@echo " - Rebuild Docker Container            : make build"
	@echo " - Rebuild & Start Docker Container    : make build up"
	@echo " - publish and deploy                  : make publish deploy env=dev"

up:
	@echo "Starting containers..."
	@docker-compose up -d
	@echo "Attachig shell..."
	@docker-compose exec $(container) bash

down:
	@echo "Stopping containers..."
	@docker-compose down

build: down
	@echo "Stopping containers..."
	@docker-compose down
	@echo "Building containers..."
	@docker-compose build

version:
	@echo "${GREEN}Setting version number $(version) ${NC}"
	@echo '{ "version": "${version}" }' > src/version.json


start: docker-check
	@echo -e "${GREEN}Starting the $(release) release of $(project)${NC}"
	@cd src/Am.I.Online.API; dotnet run -- --help

test: docker-check
	@echo -e "${GREEN}Testing v${version} of $(release) release${NC}"
	@dotnet test Am.I.Online.sln --logger:trx --results-directory ../TestResults \
		 /p:CollectCoverage=true \
		 /p:CoverletOutput=../TestResults/ \
		 /p:MergeWith=../TestResults/coverlet.info \
		 /p:Exclude="[*.Tests]*" \
		 /p:CoverletOutputFormat="lcov"

deploy: publish
	@echo -e "${GREEN}Deploying v${version} of $(release) release${NC}"


publish: docker-check env-check
	@echo -e "${GREEN}Building the $(release) release of $(project)${NC}"

	@dotnet publish src/Am.I.Online.Api/Am.I.Online.Api.csproj -r linux-x64 -c $(release) \
		-p:DebugType=None \
		-p:DebugSymbols=false \
		-p:PublishSingleFile=false \
		-p:PublishTrimmed=false --self-contained false \
		-p:VersionSuffix=$(version-suffix) \
		-p:FileVersion=$(version) \
		-p:VersionPrefix=$(version) \
	--output ./dist/$(release)/linux-x64
	@dotnet publish src/Am.I.Online.Api/Am.I.Online.Api.csproj -r win-x64 -c $(release) \
		-p:DebugType=None \
		-p:DebugSymbols=false \
		-p:PublishSingleFile=false \
		-p:PublishTrimmed=false --self-contained false \
		-p:VersionSuffix=$(version-suffix) \
		-p:FileVersion=$(version) \
		-p:VersionPrefix=$(version) \
		--output ./dist/$(release)/win-x64

docker-check:
	$(call assert-file-exists,$(docker-filecheck), This step should only be run from Docker. Please run `make up` first.)

env-check:
	$(call check_defined, env, No environment set. Supported environments are: [ dev | prod ]. Please set the env variable. e.g. `make env=dev publish`)

# Check that given variables are set and all have non-empty values,
# die with an error otherwise.
#
# Params:
#   1. Variable name(s) to test.
#   2. (optional) Error message to print.
check_defined = \
    $(strip $(foreach 1,$1, \
    	$(call __check_defined,$1,$(strip $(value 2)))))
__check_defined = \
    $(if $(value $1),, \
    	$(error Undefined $1$(if $2, ($2))))

define assert
  $(if $1,,$(error Assertion failed: $2))
endef

define assert_warn
  $(if $1,,$(warn Assertion failed: $2))
endef

define assert-file-exists
  $(call assert,$(wildcard $1),$1 does not exist. $2)
endef
