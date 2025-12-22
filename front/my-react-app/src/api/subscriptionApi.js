// src/api/subscriptionApi.js
import http from './http';

export const subscriptionApi = {
  getMySubscriptions: () => http.get('/subscriptions/my'),
  createSubscription: (data) => http.post('/subscriptions', data),
  updateSubscription: (id, data) => http.put(`/subscriptions/${id}`, data),
  deleteSubscription: (id) => http.delete(`/subscriptions/${id}`),
};