ARG NODE_VERSION=23.0.0

FROM node:${NODE_VERSION}-slim

ARG USERNAME=myuser
ARG GROUP=mygroup
ARG UID=1234
ARG GID=1234

# Install tools and create group/user
RUN apt-get update && apt-get install -y passwd && rm -rf /var/lib/apt/lists/* && \
    groupadd -g ${GID} ${GROUP} && \
    useradd -m -u ${UID} -g ${GROUP} ${USERNAME}

# Set working directory
WORKDIR /app

# Copy package files
COPY package*.json ./

# Run npm install
RUN npm install

# USER ${USERNAME}

COPY . .

EXPOSE 3000

CMD ["npm", "start"]

