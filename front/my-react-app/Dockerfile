# -------- build stage --------
FROM node:20-alpine AS build

WORKDIR /app

# 1️⃣ зависимости
COPY package*.json ./
RUN npm install

# 2️⃣ код
COPY . .

# 3️⃣ билд
RUN npm run build


# -------- runtime stage --------
FROM node:20-alpine

WORKDIR /app

# preview сервер Vite
RUN npm install -g serve

COPY --from=build /app/dist ./dist

EXPOSE 5173

CMD ["serve", "-s", "dist", "-l", "5173"]
