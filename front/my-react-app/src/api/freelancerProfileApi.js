import http from "./http";

export const freelancerProfileApi = {
  getMyProfile: () => http.get("/freelancer/profile/me"),
  updateMyProfile: (dto) => http.put("/freelancer/profile/me", dto),
};
