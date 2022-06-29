# Solid Client
# FROM node:alpine AS builder
# WORKDIR /app
# COPY ./clients/solidclient/package.json ./
# COPY ./clients/solidclient/yarn.lock ./
# RUN yarn install
# COPY ./clients/solidclient/ ./
# RUN yarn build

# FROM nginx:alpine  
# COPY --from=builder /app/dist /usr/share/nginx/html
# COPY ./nginx.conf /etc/nginx/conf.d/default.conf
# ENTRYPOINT ["nginx","-g","daemon off;"]



# React Client
FROM node:alpine AS builder
WORKDIR /app
COPY ./clients/reactclient/package.json ./
COPY ./clients/reactclient/yarn.lock ./
RUN yarn install
COPY ./clients/reactclient/ ./
RUN yarn build

FROM nginx:alpine  
COPY --from=builder /app/build /usr/share/nginx/html
COPY ./nginx.conf /etc/nginx/conf.d/default.conf
ENTRYPOINT ["nginx","-g","daemon off;"]