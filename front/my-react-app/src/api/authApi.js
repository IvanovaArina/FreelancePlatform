//src/api/authHttp.js


import http from "./http";

export const authApi = {
  signin: (data) => http.post("/auth/signin", data),
  signup: (data) => http.post("/auth/signup", data)
};
