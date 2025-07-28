import axios from "axios";

export const apiUrl = import.meta.env.VITE_APP_BACKEND_URL;

const api = axios.create({
    baseURL: apiUrl,
    headers: {
        "Content-Type": "application/json",
    },
    withCredentials: true
});

api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('accessToken');
    
    if (token) {
       config.headers.Authorization = `Bearer ${token}`    
    } else {
         config.headers.Authorization
    }
    return config;
  },
  (error) => Promise.reject(error)
);

export default api;