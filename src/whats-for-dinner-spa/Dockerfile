FROM node:20.13.1-alpine3.18

WORKDIR /app

COPY package*.json ./

RUN npm install

EXPOSE 4200 49153

CMD ["npm", "start"]    