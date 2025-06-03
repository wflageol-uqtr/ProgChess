import axios from "axios";

const api = axios.create({
    
    baseURL: "http://localhost:5290",
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