# This is the Dockerfile for building a production image with Pizzly

# Build image
FROM node:14-slim

WORKDIR /app

# Copy in dependencies for building
COPY *.json ./
COPY yarn.lock ./
# COPY config ./config
COPY integrations ./integrations/
COPY src ./src/
COPY tests ./tests/
COPY views ./views/

RUN yarn install


# Actual image to run from.
FROM node:14-slim

# Make sure we have ca certs for TLS
RUN apt-get update && apt-get install -y \
    curl \
    wget \
    gnupg2 ca-certificates libnss3  \
    git

# Make a directory for the node user. Not running Pizzly as root.
RUN mkdir /home/app && chown -R root:root /home/app
WORKDIR /home/app

USER root

# Startup script
COPY --chown=root:root ./startup.sh ./startup.sh
RUN chmod +x ./startup.sh
# COPY from first container
COPY --chown=root:root --from=0 /app/package.json ./package.json
COPY --chown=root:root --from=0 /app/dist/ .
COPY --chown=root:root --from=0 /app/views ./views
COPY --chown=root:root --from=0 /app/node_modules ./node_modules

# Run the startup script
CMD ./startup.sh