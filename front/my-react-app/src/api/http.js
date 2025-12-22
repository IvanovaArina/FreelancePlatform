//src/api/http.js

import axios from "axios";

const http = axios.create({
  // main-api проброшен на 5000 порт
  baseURL: "http://localhost:5000/api",
  withCredentials: false
});

// Логирование запросов
http.interceptors.request.use((config) => {
  console.log(`[HTTP] → ${config.method.toUpperCase()} ${config.url}`);
  return config;
});

// Логирование ответов
http.interceptors.response.use(
  (res) => {
    console.log("[HTTP SUCCESS]", res.data);
    return res;
  },
  (err) => {
    console.error("[HTTP ERROR]", err.response?.data || err.message);
    throw err;
  }
);

http.interceptors.request.use((config) => {
  console.log(`[HTTP] → ${config.method.toUpperCase()} ${config.url}`);

  // передаём токен
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

export default http;
