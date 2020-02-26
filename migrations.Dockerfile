# SDK Build configuration
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS reminders-sdk

ARG WAIT_FOR_SCRIPT
ARG WAIT_FOR_TIMEOUT
ARG DOCKER_DATABASE_CONTAINER
ARG DOTNET_PROJECT_PATH
ENV ENV_WAIT_FOR_SCRIPT=$WAIT_FOR_SCRIPT
ENV ENV_WAIT_FOR_TIMEOUT=$WAIT_FOR_TIMEOUT
ENV ENV_DOCKER_DATABASE_CONTAINER=$DOCKER_DATABASE_CONTAINER
ENV ENV_DOTNET_PROJECT_PATH=$DOTNET_PROJECT_PATH

# Add dotnet tool for migration
RUN dotnet tool install --global dotnet-ef --version 3.1.1 
ENV PATH="$PATH:/root/.dotnet/tools"

# Set up the work directory 
WORKDIR /app

# Copy all items to the work directory
COPY . ./

# Give script permissions
RUN chmod +x ${ENV_WAIT_FOR_SCRIPT}

# Update databse
CMD ${ENV_WAIT_FOR_SCRIPT} ${ENV_DOCKER_DATABASE_CONTAINER} -t ${ENV_WAIT_FOR_TIMEOUT} -- dotnet ef database update --project ${ENV_DOTNET_PROJECT_PATH}
