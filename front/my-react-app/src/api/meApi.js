// src/api/meApi.js
import http from './http';

export const meApi = {
  getName: () => http.get('/me/name'),
  changeName: (newName) => http.put('/me/name', { newName }),
};