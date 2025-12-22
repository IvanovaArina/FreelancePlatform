// src/api/portfolioApi.js
import http from './http';

export const portfolioApi = {
  // Получить все свои проекты
  getMyPortfolio: () => http.get('/portfolio'),

  // Создать новый проект
  createProject: (data) =>
    http.post('/portfolio', {
      title: data.title,
      description: data.description,
      tempImageKey: data.tempImageKey || null   // ← главное изменение
    }),

  // Обновить проект
  updateProject: (id, data) =>
    http.put(`/portfolio/${id}`, {
      title: data.title,
      description: data.description,
      tempImageKey: data.tempImageKey || null   // ← главное изменение
    }),

  // Удалить проект
  deleteProject: (id) => http.delete(`/portfolio/${id}`),
};
